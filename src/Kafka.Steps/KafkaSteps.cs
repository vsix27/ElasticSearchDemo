using KafkaNet;
using KafkaNet.Common;
using KafkaNet.Model;
using KafkaNet.Protocol;
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
        [Given(@"I have expression Random GUID")]
        public void Given_I_have_expression_Random_GUID()
        {
            ScenarioContext.Current["kafkaMessage"] = Guid.NewGuid().ToString( "N");
        }

        [When(@"I send it to kafka (.*) server to (.*) topic")]
        public void When_I_send_it_to_kafka(string kafkaServer, string topic)
        {
            ScenarioContext.Current["kafkaTopic"] = topic;
            ScenarioContext.Current["kafkaServer"] = kafkaServer;

            var options = new KafkaOptions(new Uri("http://" + kafkaServer));
            //create a producer to send messages with
            var producer = new Producer(new BrokerRouter(options))
            {
                BatchSize = 100,
                BatchDelayTime = TimeSpan.FromMilliseconds(2000)
            };

            // get offset:
            string testGuid = Guid.NewGuid().ToString("N");
            var responseG = producer.SendMessageAsync(topic, new List<Message> { new Message(testGuid) }).Result;
            long offsetG = responseG[0].Offset;
            ScenarioContext.Current["offsetG"] = offsetG;

            Console.WriteLine($" ==================  offsetG {offsetG} ==================");
            int someMessages = 2;
            ScenarioContext.Current["someMessages"] = someMessages;
            SendRandomBatch(producer, topic, someMessages);

            string kafkaJson = $@"'main':{{
                    'GUID': '{ScenarioContext.Current["kafkaMessage"]}',
                    'date': '{DateTime.Now}',
                    'person': {{ 'fistname':'Jason', 'lastname':'Consolo' }}
                }}";
                       
            // send single main message                      
            var response = producer.SendMessageAsync(topic, new[] { new Message(kafkaJson) }).Result;
            
            Console.WriteLine($"Completed send of batch: producer Buffered: {producer.BufferCount}; AsyncCount: {producer.AsyncCount};");
            foreach (var result in response.OrderBy(x => x.PartitionId))
                Console.WriteLine($"Topic: {result.Topic}; PartitionId: {result.PartitionId}; Offset: {result.Offset}");

            ScenarioContext.Current["kafkaResult"] = Tuple.Create(kafkaJson, response.ToList());

            using (producer)
            {
            }
        }

        private static async void SendRandomBatch(Producer producer, string topic, int count)
        {
            Func<string, string> kafkaJson = (info) => $@"'some':{{
                    'id{info}': '{DateTime.Now.Ticks}',
                    'date': '{DateTime.Now}',
                    'guid': '{Guid.NewGuid().ToString("N")}',
                    'person': {{'fistname':'{Path.GetRandomFileName().Replace(".", "")}', 'lastname':'{Path.GetRandomFileName().Replace(".", "")}' }}
                }}";
            if (count < 1) count = 1;
            var messages = new List<Message>();
            for (int k = 0; k < count; k++) messages.Add(new Message(kafkaJson($"_{k + 1}_of_{count}")));

            Console.WriteLine(" ==================  SendRandomBatch Start ==================");

            //send multiple messages
            //var sendTask = producer.SendMessageAsync(topic, Enumerable.Range(0, count).Select(x => new Message(x.ToString())));
            var sendTask = producer.SendMessageAsync(topic, messages);

            Console.WriteLine("Posted #{0} messages.  Buffered:{1} AsyncCount:{2}", count, producer.BufferCount, producer.AsyncCount);

            var response = await sendTask;

            Console.WriteLine("Completed send of batch: {0}. Buffered:{1} AsyncCount:{2}", count, producer.BufferCount, producer.AsyncCount);

            foreach (var result in response.OrderBy(x => x.PartitionId))
                Console.WriteLine($"Topic: {result.Topic}; PartitionId:{result.PartitionId}; Offset:{result.Offset}");

            Console.WriteLine(" ==================  SendRandomBatch End ==================");
        }


        [Then(@"I should consume it in (.*) seconds")]
        public void Then_I_should_consume_it_in_P0_seconds(int seconds)
        {
            string topic = ScenarioContext.Current["kafkaTopic"].ToString();
            var options = new KafkaOptions(new Uri("http://" + ScenarioContext.Current["kafkaServer"]));

            var someMessages  =  ScenarioContext.Current["someMessages"] ;
            Console.WriteLine($" ==================  Expect {someMessages}+1 messages Start ==================");
            long offsetG = long.Parse(ScenarioContext.Current["offsetG"].ToString());
            var consumerG = new Consumer(new ConsumerOptions(topic, new BrokerRouter(options)), new OffsetPosition { Offset = offsetG });
            int k = 0;
            foreach (var data in consumerG.Consume(new CancellationTokenSource(seconds * 1000).Token))
            {
                var message = data.Value.ToUtf8String();
                Console.WriteLine($"{k++}. consumerG: PartitionId {data.Meta.PartitionId}, Offset {data.Meta.Offset} : {message}");               
            }
            Console.WriteLine($" ==================  Expect {someMessages}+1 messages End   ==================");


            var result = (Tuple<string, List<ProduceResponse>>)ScenarioContext.Current["kafkaResult"];

            var consumer = new Consumer(new ConsumerOptions(topic, new BrokerRouter(options)), new OffsetPosition { Offset = result.Item2[0].Offset });       
            foreach (var data in consumer.Consume(new CancellationTokenSource(seconds * 1000).Token))
            {
                var message = data.Value.ToUtf8String();
                Console.WriteLine($"Response: PartitionId {data.Meta.PartitionId}, Offset {data.Meta.Offset} : {message}");
                if (result.Item1 == message)
                {
                    Assert.IsTrue(true);
                    return;
                }
            }
            Assert.IsTrue(false);
        }


        private void ConsumeMessages(string topic,KafkaOptions options)
        {
            //start an out of process thread that runs a consumer that will write all received messages to the console
            Task.Run(() =>
            {
                var consumer = new Consumer(new ConsumerOptions(topic, new BrokerRouter(options)) );
                foreach (var data in consumer.Consume())
                {
                    var message = data.Value.ToUtf8String();
                    Console.WriteLine($"Response: PartitionId {data.Meta.PartitionId}, Offset {data.Meta.Offset} : {message}");
                }
            });
        }


        [Given(@"I have kafka brokers")]
        public void Given_I_have_kafka_brokers(Table table)
        {
            string zookeeper = null;
            var zooDict = new Dictionary<string, string>();
            foreach (var row in table.Rows)
            {
                if (zookeeper != row["zookeeper"])
                {
                    zookeeper = row["zookeeper"];
                    zooDict.Add(zookeeper, "");
                }
                zooDict[zookeeper] += ";" + row["kafka broker"];
            }
            //    |  zookeeper | kafka broker  |
            ScenarioContext.Current["kafkaBrokers"] = zooDict;
        }

        [When(@"I call kafka server")]
        public void When_I_call_kafka_server()
        {
            // placeholder
        }

        [Given(@"I have kafka topics")]
        public void Given_I_have_kafka_topics(Table table)
        {
            var list = new List<KeyValuePair<string, string>>();
            foreach (var row in table.Rows)
                list.Add(new KeyValuePair<string, string>(row[0], row[1]));
            ScenarioContext.Current["kafkaTopics"] = list;
            // | kafka broker  | topic  |
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
                //try { var lcount = messages.Count(); } catch (Exception ex1) { Console.WriteLine(ex1); }
                //try { var lcount = messages.LongCount(); } catch (Exception ex1) { Console.WriteLine(ex1); }
                int k = 0;
                foreach (var data in messages)
                {
                    var message = data.Value.ToUtf8String();
                    Console.WriteLine($"{k++}  Response: PartitionId {data.Meta.PartitionId}, Offset{data.Meta.Offset} : {message}");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

        [Then(@"I should retreive it in (.*) seconds")]
        public void Then_I_should_retreive_it_in_P0_seconds(int seconds)
        {
            //    | kafka broker  | zookeeper |
            var zooBrokers = ScenarioContext.Current["kafkaBrokers"] as Dictionary<string, string>;
            Assert.IsNotNull(zooBrokers);

            var listTopics = ScenarioContext.Current["kafkaTopics"] as List<KeyValuePair<string, string>>;
            Assert.IsNotNull(listTopics);

            foreach (var row in zooBrokers)
            {
                string zoo = row.Key;

                var topics = listTopics.Where(o => o.Key.Equals(zoo)).Select(o => o.Value).ToList();

                var brokers = row.Value.Split("; ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                var options = new KafkaOptions();
                foreach (string broker in brokers)
                    options.KafkaServerUri.Add(new Uri("http://" + broker));

            
                foreach (string topic in topics)
                {
                    try
                    {                       
                        //var cons0 = new Consumer(new ConsumerOptions(topic, new BrokerRouter(options)));
                        //DebugKafkaConsumer(cons0, seconds, topic, zoo, "");

                        //var cons1 = new Consumer(new ConsumerOptions(topic, new BrokerRouter(options)), new OffsetPosition { Offset = -1 });
                        //DebugKafkaConsumer(cons1, seconds, topic, zoo, "-1");

                        //var cons2 = new Consumer(new ConsumerOptions(topic, new BrokerRouter(options)), new OffsetPosition { Offset = -2 });
                        //DebugKafkaConsumer(cons2, seconds, topic, zoo, "-2");

                        //var cons12 = new Consumer(new ConsumerOptions(topic, new BrokerRouter(options)), new OffsetPosition { Offset = -1 }, new OffsetPosition { Offset = -2 });
                        //DebugKafkaConsumer(cons12, seconds, topic, zoo, "-1,-2");

                        //var cons3 = new Consumer(new ConsumerOptions(topic, new BrokerRouter(options)), new OffsetPosition { Offset = 14 });
                        //DebugKafkaConsumer(cons3, seconds, topic, zoo, "14");

                        var cons4 = new Consumer(new ConsumerOptions(topic, new BrokerRouter(options)), new OffsetPosition { PartitionId=0 });
                        DebugKafkaConsumer(cons4, seconds, topic, zoo, "10000");

                        ZooKeeperNet.IWatcher watcher = null;
                        var zkeeper = new ZooKeeperNet.ZooKeeper("http://" + zoo, new TimeSpan(0, 0, seconds), watcher);
                        var watchermanager = new ZooKeeperNet.ZKWatchManager();
                        var zconn = new ZooKeeperNet.ClientConnection("http://" + zoo, new TimeSpan(0, 0, seconds), zkeeper, watchermanager);
                        zconn.Start();
                        var zooClient = new ZooKeeperNet.ClientConnectionEventConsumer(zconn);
                        //var offsets = cons.GetOffsetPosition();
                        //var tcache = cons.GetTopicFromCache(topic);                        
                    }
                    catch (Exception ex) { Console.WriteLine($" -----------  zoo: {zoo}  topic:{topic}. {ex.Message}"); }
                }
            }
        }
    }
}
