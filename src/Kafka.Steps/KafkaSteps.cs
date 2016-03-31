﻿using KafkaNet;
using KafkaNet.Common;
using KafkaNet.Model;
using KafkaNet.Protocol;

using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Kafka.Steps
{
    [Binding]
    public class KafkaSteps
    {
        /// <summary>
        /// put random list of messages into ScenarioContext.Current["kafkaMessages"]
        /// </summary>
        [Given(@"I have Random expressions")]
        public void Given_I_have_Random_expressions()
        {
            Func<int, string> randomJson = (i) => { return $@"'main_{i}_{Guid.NewGuid().ToString("N")}':{{   
                    'member':{i},  'date': '{DateTime.Now.AddDays(new Random().Next(-100, 100))}',
                    'person': {{'fistname':'{Path.GetRandomFileName().Replace(".", "")}', 'lastname':'{Path.GetRandomFileName().Replace(".", "")}' }}
                }}"; };

            var lst = new List<Message>();
            for (int k = 0; k < new Random().Next(2, 7); k++)
                lst.Add(new Message(value: randomJson(k), key: $"random_{Guid.NewGuid().ToString("N") }"));

            Console.WriteLine($" ==================  Random_expressions saved in Context; Items {lst.Count}");
            Console.WriteLine($" ==================  Random_expressions saved in Context; first {lst.First().Value.ToUtf8String()}");
            Console.WriteLine($" ==================  Random_expressions saved in Context; last {lst.Last().Value.ToUtf8String()}");

            ScenarioContext.Current["kafkaMessages"] = lst;
        }

        [When(@"I send it to kafka (.*) server to (.*) topic")]
        public void When_I_send_it_to_kafka(string kafkaServer, string topic)
        {
            ScenarioContext.Current["kafkaTopic"] = topic;
            ScenarioContext.Current["kafkaServer"] = kafkaServer;

            Console.WriteLine($" ==================  kafkaServer {kafkaServer}; topic {topic} ==================");

            var options = new KafkaOptions(new Uri("http://" + kafkaServer));
            //create a producer to send messages with
            var producer = new Producer(new BrokerRouter(options))
            {
                BatchSize = 100,
                BatchDelayTime = TimeSpan.FromMilliseconds(2000)
            };

            // get offset:
            var to = new TopicOffset(producer.GetTopicOffsetAsync(topic).Result);
            ScenarioContext.Current["kafkaOffsetBeforeProduce"] = to;

            var kafkaMessages = ScenarioContext.Current["kafkaMessages"] as List<Message>;

            Console.WriteLine($" ==================  FirstOffset {to.FirstOffset}; Items {to.Items} ==================");

            // send messages                      
            var response = SendRandomBatch(producer, topic, kafkaMessages).Result;

            //Console.WriteLine($"Completed send of batch: producer Buffered: {producer.BufferCount}; AsyncCount: {producer.AsyncCount};");
            //foreach (var result in response.OrderBy(x => x.PartitionId))
            //    Console.WriteLine($"Topic: {result.Topic}; PartitionId: {result.PartitionId}; Offset: {result.Offset}");

            // Tuple with string - first message jason, list of messages
            ScenarioContext.Current["kafkaResult"] = Tuple.Create(kafkaMessages[0].Value.ToUtf8String(), response.ToList());

            using (producer)
            {
            }
        }

        private static async Task<ProduceResponse[]> SendRandomBatch(Producer producer, string topic, List<Message> messages)
        {
            //Console.WriteLine(" ==================  SendRandomBatch Start ==================");

            var sendTask = producer.SendMessageAsync(topic, messages);
            Console.WriteLine("Posted #{0} messages.  Buffered:{1} AsyncCount:{2}", messages.Count, producer.BufferCount, producer.AsyncCount);
            var response = await sendTask;
            Console.WriteLine("Completed send of batch: {0}. await Buffered:{1} AsyncCount:{2}", messages.Count, producer.BufferCount, producer.AsyncCount);

            //foreach (var result in response.OrderBy(x => x.PartitionId))
            //    Console.WriteLine($"Topic: {result.Topic}; PartitionId:{result.PartitionId}; Offset:{result.Offset}");
            //Console.WriteLine(" ==================  SendRandomBatch End ==================");
            return response;
        }


        [Then(@"I should consume it in (.*) seconds")]
        public void Then_I_should_consume_it_in_P0_seconds(int seconds)
        {
            string topic = ScenarioContext.Current["kafkaTopic"].ToString();
            var options = new KafkaOptions(new Uri("http://" + ScenarioContext.Current["kafkaServer"]));


            var to = (TopicOffset)ScenarioContext.Current["kafkaOffsetBeforeProduce"];
            if (to.Topic == null) to = GetProducerTopicOffset(options, topic, 5);
            OffsetPosition offsetPosition = new OffsetPosition() { Offset = to.FirstOffset + to.Items, PartitionId = to.PartitionId };

            var kafkaMessages = ScenarioContext.Current["kafkaMessages"] as List<Message>;

            Console.WriteLine($" ==================  Expect messages Start - befofe consuming from Offset = {offsetPosition.Offset}; Count = {kafkaMessages.Count()} ");


            //v1
            //var dict = ConsumeMessagesTaskRun(topic, options, seconds, offsetPosition);
            //var dict = ConsumeMessagesTaskRun(topic, options, 0, offsetPosition);
            var dict = ConsumeMessages(topic, options, seconds, offsetPosition);
            //var dict4 = ConsumeMessages(topic, options, 0, offsetPosition);
            Console.WriteLine($" ==================  Expect messages End   ==================");

            // Tuple - string - first message
            var result = (Tuple<string, List<ProduceResponse>>)ScenarioContext.Current["kafkaResult"];

            Console.WriteLine($" ============ looking to match result.Item1 and \n\t{result.Item1}");

            foreach (var kvp in dict)
            {
                Console.WriteLine($" ======= loop to match result.Item1 to kvp.Value: \n\t{kvp.Value }");
                if (result.Item1 == kvp.Value)
                {
                    Assert.IsTrue(true);
                    return;
                }
            }
            Assert.IsTrue(false);
        }

        /// <summary>
        /// returns dictionary with kvp: key - offset, value - json
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="options"></param>
        /// <param name="delay"></param>
        /// <param name="offsetPosition"></param>
        /// <returns></returns>
        private Dictionary<long, string> ConsumeMessagesTaskRun(string topic, KafkaOptions options, int delay = 0, OffsetPosition offsetPosition = null)
        {
            var list = new Dictionary<long, string>();

            //start an out of process thread that runs a consumer that will write all received messages to the console
            Task.Run(() =>
            {
                int k = 0;
                var consumer = new Consumer(new ConsumerOptions(topic, new BrokerRouter(options)) { Log = new DefaultTraceLog() }, offsetPosition);
                var messages = (delay < 1) ? consumer.Consume(null) : consumer.Consume(new CancellationTokenSource(delay * 1000).Token);
                try
                {
                    foreach (var data in messages)
                    {
                        var message = data.Value.ToUtf8String();
                        list.Add(data.Meta.Offset, message);
                        Console.WriteLine($"{k++} [{topic}] TaskRun: PartitionId {data.Meta.PartitionId}, Offset {data.Meta.Offset}: {message}");
                    }
                }
                catch (Exception ex) { Console.WriteLine($" -----------  topic:{topic}. {ex.Message}"); }
            });
            return list;
        }

        /// <summary>
        /// returns dictionary with kvp: key - offset, value - json
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="options"></param>
        /// <param name="delay"></param>
        /// <param name="offsetPosition"></param>
        /// <returns></returns>
        private Dictionary<long, string> ConsumeMessages(string topic, KafkaOptions options, int delay, OffsetPosition offsetPosition)
        {
            var list = new Dictionary<long, string>();
            int k = 0;
            var consumer = new Consumer(new ConsumerOptions(topic, new BrokerRouter(options)) { Log = new DefaultTraceLog() }, offsetPosition);

            try
            {
                var messages = (delay < 1) ? consumer.Consume(null) : consumer.Consume(new CancellationTokenSource(delay * 1000).Token);

                foreach (var data in messages)
                {
                    var message = data.Value.ToUtf8String();
                    var key = data.Key.ToUtf8String();
                    list.Add(data.Meta.Offset, message);
                    Console.WriteLine($"{k++} {DateTime.Now.ToString()} [{topic}] PartitionId {data.Meta.PartitionId}, Offset {data.Meta.Offset}, key: {key}, message {message}");
                }
            }
            catch (Exception ex) { Console.WriteLine($" -----------  topic:{topic}. {ex.Message}"); }
            finally
            {
                consumer.Dispose();
            }
            return list;
        }


        [Given(@"I have kafka brokers")]
        public void Given_I_have_kafka_brokers(Table table)
        {
            TableToContextList("kafkaBrokers", table);
        }

        [When(@"I call kafka server")]
        public void When_I_call_kafka_server()
        {
            // placeholder
        }

        [Given(@"I have kafka topics")]
        public void Given_I_have_kafka_topics(Table table)
        {
            TableToContextList("kafkaTopics", table);
        }

        [Given("I have kafka (.*)")]
        public void Given_I_have_kafka_Organization(string topic)
        {
            ScenarioContext.Current["kafkaTopic"] = topic;
        }

        public void TableToContextList(string curContextName, Table table)
        {
            var list = new List<string>();
            foreach (var row in table.Rows)
                list.Add(row[0]);
            ScenarioContext.Current[curContextName] = list;
        }

        private void DebugKafkaConsumer(Consumer cons, int seconds, string topic, string zoo, string offset)
        {
            try
            {
                Console.WriteLine($"================ zoo {zoo}; topic {topic}; offset {offset}");

                try
                {
                    var offsets = cons.GetOffsetPosition();
                    if (offsets == null || offsets.Count == 0) Console.WriteLine($"cons.GetOffsetPosition() empty");
                    else foreach (var off in offsets) Console.WriteLine($"cons.GetOffsetPosition() PartitionId {off.PartitionId}, Offset {off.Offset}");

                }
                catch (Exception ex1) { Console.WriteLine(ex1); }

                //try
                //{
                //    var t = cons.GetTopicFromCache(topic);
                //    foreach (var p in t.Partitions )
                //    Console.WriteLine($"cons.GetTopicFromCache({topic}) ErrorCode {p.ErrorCode}; Isrs {p.Isrs}; LeaderId {p.LeaderId} PartitionId {p.PartitionId} Replicas Count {p.Replicas.Count}");

                //    //var ts = cons.GetTopicOffsetAsync (topic,1, 1000).Result;
                //    //Console.WriteLine($"ts ({ts}");
                //}
                //catch (Exception ex1) { Console.WriteLine(ex1); }

                var messages = cons.Consume(new CancellationTokenSource(seconds * 1000).Token);

                DebugKafkaMessages(messages, 10, $"for topic {topic} with delay cancel token seconds {seconds}");
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

        private void DebugKafkaMessages(IEnumerable<Message> messages, int maxMessages = 10, string info = "")
        {
            if (maxMessages < 1) maxMessages = 10000;
            //System.Collections.Concurrent.BlockingCollection
            int k = 0;
            Console.WriteLine(" ================== start of DebugKafkaMessages " + info);
            try
            {
                foreach (var data in messages)
                {
                    var message = data.Value.ToUtf8String();
                    //Console.WriteLine($"{k++}  Response: PartitionId {data.Meta.PartitionId}, Offset{data.Meta.Offset} : \n{message}");
                    Console.WriteLine($"{k++}  Response: PartitionId {data.Meta.PartitionId}, \n{message}");
                    if (k > maxMessages)
                        break;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            Console.WriteLine(" ================== end of DebugKafkaMessages " + info);
        }

        private void DebugOffsets(List<OffsetResponse> offsets)
        {
            foreach (var offs in offsets)
            {
                {
                    string strOffsets = "";
                    foreach (var off in offs.Offsets) strOffsets += off + ";";
                    Console.WriteLine($" PartitionId {offs.PartitionId}; Offsets: {strOffsets}");
                }
            }
        }

        public struct TopicOffset
        {
            public long FirstOffset;
            public long Items;
            public int PartitionId;
            public string Topic;
            public TopicOffset(List<OffsetResponse> offsets)
            {
                FirstOffset = offsets[0].Offsets[1];
                Items = offsets[0].Offsets[0] - offsets[0].Offsets[1];
                PartitionId = offsets[0].PartitionId;
                Topic = offsets[0].Topic;
            }
        }

        private TopicOffset GetProducerTopicOffset(KafkaOptions options, string topic, int seconds)
        {
            var to = new TopicOffset();
            try
            {
                using (var producer = new Producer(new BrokerRouter(options))
                {
                    BatchSize = 100,
                    BatchDelayTime = TimeSpan.FromMilliseconds(2000)
                })
                {
                    var offsets = producer.GetTopicOffsetAsync(topic).Result;
                    to = new TopicOffset(offsets);
                    Console.WriteLine($" GetProducerOffset {topic} : PartitionId: {to.PartitionId }; FirstOffset: {to.FirstOffset }; Items: {to.Items} ");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return to;
        }

        private long GetProducerOffset(Producer producer, string topic)
        {
            var offsets = producer.GetTopicOffsetAsync(topic).Result;
            return offsets[0].Offsets[0];
        }

        /*
        you can get offset by calling: 
            var responsePing = producer.SendMessageAsync(topic, new List<Message> { new Message(topic) }).Result;
            long offsetPing = responsePing[0].Offset;
                        
        //var offsets = producer.GetTopicOffsetAsync(topic).Result;
        //DebugOffsets(offsets);                                       

         */

        [Then(@"I should retrieve last (.*) messages in (.*) seconds")]
        public void Then_I_should_retrieve_it_in_P0_seconds(int lastMessages, int seconds)
        {
            if (lastMessages < 0) lastMessages = 10;

            var brokers = ScenarioContext.Current["kafkaBrokers"] as List<string>;
            Assert.IsNotNull(brokers);

            var topics = ScenarioContext.Current["kafkaTopics"] as List<string>;
            if (topics == null && ScenarioContext.Current["kafkaTopic"] != null)
                topics = new List<string> { ScenarioContext.Current["kafkaTopic"].ToString() };
            Assert.IsNotNull(topics);

            var options = new KafkaOptions();
            foreach (string broker in brokers)
                options.KafkaServerUri.Add(new Uri("http://" + broker));

            int topicCount = 0;
            foreach (string topic in topics)
            {
                topicCount++;
                try
                {
                    TopicOffset to = GetProducerTopicOffset(options, topic, seconds);
                    if (to.Items == 0)
                    {
                        Console.WriteLine($" GetProducerOffset {topic} does not have mesasges... ");
                        continue;
                    }

                    // get last 5 messages, otherwise it chokes..
                    long foffs = to.FirstOffset;
                    if (to.Items > lastMessages) foffs += to.Items - lastMessages;

                    OffsetPosition offsetPosition = new OffsetPosition() { Offset = foffs, PartitionId = to.PartitionId };

                    //v1
                    //Console.WriteLine($" ---- start 1 ------- {DateTime.Now.ToString()}  topic:{topic}. {topicCount} of {topics.Count}");
                    //ConsumeMessagesTaskRun(topic, options, seconds, offsetPosition);

                    //v2 no results:
                    //Console.WriteLine($" ---- start 2 ------- {DateTime.Now.ToString()}  topic:{topic}. {topicCount} of {topics.Count}");
                    //ConsumeMessagesTaskRun(topic, options, 0, offsetPosition);

                    // v3 no results:
                    Console.WriteLine($" ---- start 3 ------- {DateTime.Now.ToString()}  topic:{topic}. {topicCount} of {topics.Count}");
                    ConsumeMessages(topic, options, seconds, offsetPosition);

                    //v4
                    //Console.WriteLine($" ---- start 4 ------- {DateTime.Now.ToString()}  topic:{topic}. {topicCount} of {topics.Count}");
                    //ConsumeMessages(topic, options, 0, offsetPosition);

                    //v5
                    //var cancellationToken = new CancellationTokenSource(seconds * 1000);
                    //Task.Factory.StartNew(() => ConsumeMessages(topic, options, 0, offsetPosition), cancellationToken.Token);
                    //Thread.Sleep(5000); //simulate some other work
                    //cancellationToken.Cancel(false); //this stops the Task??  false indicates that no exceptions will be thrown.

                    Console.WriteLine($" ---- end ------- {DateTime.Now.ToString()}  topic:{topic}. {topicCount} of {topics.Count}");
                    continue;

                }
                catch (Exception ex) { Console.WriteLine($" -----------  topic:{topic}. {ex.Message}"); }
            }
        }

        [Given(@"I have json file")]
        public void Given_I_have_json_file()
        {
        }

        [When(@"I parse it")]
        public void When_I_parse_it()
        {
        }

        [Then(@"I should be find in file (.*) matching json values")]
        public void Then_I_should_be_find_in_file_P0_matching_json_values(string p0, Table table)
        {
            p0 = p0.Replace(">", "").Replace("<", "");
            string jfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p0);
            Assert.IsTrue(File.Exists(jfile));

            var obj = JObject.Parse(File.ReadAllText(jfile));

            foreach (var r in table.Rows)
            {
                var jpath = r["jsonpath"];
                var jval = r["value"];
                var found = (string)obj.SelectToken(jpath);
                if (jval.Equals("null", StringComparison.OrdinalIgnoreCase)) jval = null;
                Assert.AreEqual(found, jval);
            }
        }

    }
}
