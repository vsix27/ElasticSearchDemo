using KafkaNet;
using KafkaNet.Common;
using KafkaNet.Model;
using KafkaNet.Protocol;

using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using TechTalk.SpecFlow;

namespace Kafka.Steps
{
    public class BackgroundWorkerErrorEventArgs : EventArgs
    {
        public BackgroundWorkerErrorEventArgs(Exception error) { this.Error = error; }
        public Exception Error;
    }

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
            ScenarioContext.Current["kafkaTopics"] = new List<string> { topic };

            var brokers = new List<string> { kafkaServer };
            ScenarioContext.Current["kafkaBrokers"] = brokers;
            Assert.IsNotNull(brokers);

            var options = GetKafkaOptions(brokers);

            //create a producer to send messages with
            var producer = new Producer(new BrokerRouter(options))
            {
                BatchSize = 100,
                BatchDelayTime = TimeSpan.FromMilliseconds(2000)
            };

            // get offset:
            var to = GetProducerTopicOffset(options, topic);

            ScenarioContext.Current["kafkaOffsetBeforeProduce"] = to;

            var kafkaMessages = ScenarioContext.Current["kafkaMessages"] as List<Message>;

            Console.WriteLine($" ==================  FirstOffset {to.FirstOffset}; Items {to.Items} ==================");

            // send messages                      
            var response = SendRandomBatch(producer, topic, kafkaMessages).Result;

            // Tuple with string - first message json, list of messages
            ScenarioContext.Current["kafkaResult"] = Tuple.Create(kafkaMessages[0].Value.ToUtf8String(), response.ToList());

            response = null;
            producer.Dispose();
        }

        private static async Task<ProduceResponse[]> SendRandomBatch(Producer producer, string topic, List<Message> messages)
        {
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
            var brokers = ScenarioContext.Current["kafkaBrokers"] as List<string>;

            var options = GetKafkaOptions(brokers);

            var to = (TopicOffset)ScenarioContext.Current["kafkaOffsetBeforeProduce"];
            if (to.Topic == null)
            {
                to = GetProducerTopicOffset(options, topic);
                if (to.Items == 0)
                {
                    topicJsons[topic] = new List<string> {to.PartitionId == -1
                        ? "cannot connect to the Kafka server, exiting..."
                        : "topic does not have messages" };
                    LogJobs();
                    Assert.IsTrue(false);
                    return;
                }
            }

            OffsetPosition offsetPosition = new OffsetPosition() { Offset = to.FirstOffset + to.Items, PartitionId = to.PartitionId };

            var kafkaMessages = ScenarioContext.Current["kafkaMessages"] as List<Message>;

            Console.WriteLine($" ==================  Expect messages Start - befofe consuming from Offset = {offsetPosition.Offset}; Count = {kafkaMessages.Count()} ");


            //v1
            //var dict = ConsumeMessagesTaskRun(topic, options, seconds, offsetPosition);
            //var dict = ConsumeMessagesTaskRun(topic, options, 0, offsetPosition);
            var dict = ConsumeMessages(topic, options, offsetPosition, seconds);
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
            LogJobs();
            Assert.IsTrue(false);
        }

        internal static void LogJobs()
        {
            var topics = ScenarioContext.Current["kafkaTopics"] as List<string>;
            string jobs = "topics for testing \n\t"+ topics.Aggregate((x, y) => x + ", " + y) + "\n";          

            int k = 0;
            foreach (string key in topicJsons.Keys)
            {
                jobs += $"\n{k++}. {key}";
                foreach (string job in topicJsons[key])
                    jobs += $"\n\t {job}";
            }
            Console.WriteLine(jobs);
            string status = Path.Combine(ProjTmp, $"_job_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.txt");
            File.WriteAllText(status, jobs);
            topicJsons = new Dictionary<string, List<string>>();
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

        private static string _projTmp = null;
        private static string ProjTmp
        {
            get
            {
                if (string.IsNullOrEmpty(_projTmp))
                {
                    _projTmp = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
                    if (!Directory.Exists(_projTmp)) Directory.CreateDirectory(_projTmp);
                    Console.WriteLine("messages will be saved in directory " + _projTmp);
                }
                return _projTmp;
            }
        }

        internal static Dictionary<string, List<string>> topicJsons = new Dictionary<string, List<string>>();
     
        [Given("I have zookeepers for kafka")]
        public void Given_I_have_zookeepers_for_kafka(Table table)
        {
            var lst = new List<string>();
            foreach (var row in table.Rows)
                lst.Add(row[0]);
            ScenarioContext.Current["zookeepers"] = lst;
        }       

        /// <summary>
        /// returns dictionary with kvp: key - offset, value - json
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="options"></param>
        /// <param name="delay"></param>
        /// <param name="offsetPosition"></param>
        /// <returns></returns>
        public Dictionary<long, string> ConsumeMessages(string topic, KafkaOptions options, OffsetPosition offsetPosition, int delay = 0)
        {
            var list = new Dictionary<long, string>();
            int k = 0;
            var consumer = new Consumer(new ConsumerOptions(topic, new BrokerRouter(options))
            {
                Log = new DefaultTraceLog(),
                //TopicPartitionQueryTimeMs = 5000,
                //BackoffInterval=new TimeSpan (0,0,3),
                //MaxWaitTimeForMinimumBytes = new TimeSpan(0, 0, 3),
            }, offsetPosition);
          
            if (!topicJsons.ContainsKey(topic)) topicJsons[topic] = new List<string>();
            
            try
            {
                var to = GetProducerTopicOffset(options, topic);
                topicJsons[topic].Add($"PartitionId: {to.PartitionId}; FirstOffset: {to.FirstOffset}; Items: {to.Items} delay: {delay}\n");

                if (to.Items == 0)
                {
                    topicJsons[topic] = new List<string> {to.PartitionId == -1
                        ? "cannot connect to the Kafka server, exiting..."
                        : "topic does not have messages" };
                    return list;
                }
                
                var messages = (delay < 0) ? consumer.Consume() : consumer.Consume(new CancellationTokenSource(delay * 1000).Token);

                #region terminate long running thread
                int countTry = 0;
                int maxTry = 7;
                Thread workerThread = new Thread(() =>
                {
                    foreach (var data in messages)
                {
                    var message = data.Value.ToUtf8String();
                    var key = data.Key.ToUtf8String();
                    list.Add(data.Meta.Offset, message);

                    Console.WriteLine($"{k++} {DateTime.Now.ToString()} [{topic}] PartitionId {data.Meta.PartitionId}, Offset {data.Meta.Offset}, key: {key}, message {message}");
                    string jfile = $"{topic}-{data.Meta.PartitionId}-{data.Meta.Offset}-{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.json";
                    jfile = Path.Combine(ProjTmp, jfile);
                    File.WriteAllText(jfile, message);

                    topicJsons[topic].Add(Path.GetFileName(jfile));

                    if (data.Meta.Offset == to.FirstOffset + to.Items - 1)
                        break;
                }
                });

                workerThread.Start();
                while (workerThread.IsAlive)
                {
                    if (countTry++ > maxTry) workerThread.Abort();
                    Thread.Sleep(3000);
                }
                #endregion
            }
            catch (Exception ex)
            {
                topicJsons[topic].Add(ex.Message);
                Console.WriteLine($" -----------  topic:{topic}. {ex.Message}");
            }
            finally
            {
                consumer.Dispose();
            }
            return list;
        }


        [Given(@"I have brokers for kafka")]
        public void Given_I_have_kafka_brokers(Table table)
        {
            TableToContextList("kafkaBrokers", table);
        }

        [When(@"I call kafka server")]
        public void When_I_call_kafka_server()
        {
            // placeholder
        }

        [Then(@"data folder is created if missing")]
        public void Then_data_folder_is_created_if_missing()
        {
            Assert.IsNotNull(ProjTmp);
            Assert.IsTrue(Directory.Exists(ProjTmp));
            Console.WriteLine("file://" + ProjTmp.Replace("\\", "/"));
            //Console.WriteLine("ftp://" + ProjTmp.Replace("\\", "/"));
            //Console.WriteLine("folder://" + ProjTmp.Replace("\\", "/"));
            //Console.WriteLine("path://" + ProjTmp.Replace("\\", "/"));
            //Console.WriteLine("http://" + ProjTmp.Replace("\\", "/"));
            //Console.WriteLine("directory://" + ProjTmp.Replace("\\", "/"));
        }

        [Given(@"I have topics for kafka")]
        public void Given_I_have_kafka_topics(Table table)
        {
            TableToContextList("kafkaTopics", table);
        }

        [Given(@"I have topic list for kafka (.*)")]
        public void Given_I_have_topic_list_for_kafka_P0(string topics)
        {
            List<string> list = new List<string>();
            if (ScenarioContext.Current.Keys.Contains("kafkaTopics"))
                list = ScenarioContext.Current["kafkaTopics"] as List<string>;
            foreach (string topic in topics.Split(" ,;".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                if (!list.Contains(topic))
                    list.Add(topic);
            }
            ScenarioContext.Current["kafkaTopics"] = list;
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
                FirstOffset = offsets[0].Offsets.Last();
                Items = offsets[0].Offsets[0] - FirstOffset;
                PartitionId = offsets[0].PartitionId;
                Topic = offsets[0].Topic;
            }
        }

        internal static TopicOffset GetProducerTopicOffset(KafkaOptions options, string topic)
        {
            var to = new TopicOffset();
            using (var producer = new Producer(new BrokerRouter(options))
            {
                BatchSize = 100,
                BatchDelayTime = TimeSpan.FromMilliseconds(2000)
            })
            {
                List<OffsetResponse> offsets = null;

                #region terminate long running thread
                int countTry = 0;
                int maxTry = 3;
                bool invalidTopic = false;
                Thread workerThread = new Thread(() =>
                {
                    try
                    {
                        offsets = producer.GetTopicOffsetAsync(topic).Result;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        if (ex.InnerException != null)
                        {
                            if (ex.InnerException.Message.Contains("UnknownTopicOrPartition"))
                            {
                                countTry += maxTry;
                                invalidTopic = true;
                            }
                            Console.WriteLine(ex.InnerException.Message);
                        }
                    }
                });

                workerThread.Start();
                while (workerThread.IsAlive)
                {
                    if (countTry++ > maxTry) workerThread.Abort();
                    Thread.Sleep(3000);
                }
                #endregion

                // PartitionId -1: cannot connect to the Kafka server 
                // PartitionId  0: connected but threw UnknownTopicOrPartition exception
                to = (offsets == null)
                    ? new TopicOffset() { PartitionId = invalidTopic ? 0 : -1 }
                    : new TopicOffset(offsets);
            }
            return to;
        }

        internal long GetProducerOffset(Producer producer, string topic)
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

        private KafkaOptions GetKafkaOptions(List<string> brokers)
        {
            var options = new KafkaOptions();
            foreach (string broker in brokers)
                options.KafkaServerUri.Add(new Uri("http://" + broker));
            return options;
        }

        [Then(@"I should retrieve last (.*) messages in (.*) seconds")]
        public void Then_I_should_retrieve_it_in_P0_seconds(int p0, int seconds)
        {
            if (p0 < 0) p0 = 1;

            var brokers = ScenarioContext.Current["kafkaBrokers"] as List<string>;
            Assert.IsNotNull(brokers);

            List<string> topics = null;
            if (ScenarioContext.Current.ContainsKey("kafkaTopics"))
                topics = ScenarioContext.Current["kafkaTopics"] as List<string>;
            else if (ScenarioContext.Current.ContainsKey("kafkaTopic"))
                topics = new List<string> { ScenarioContext.Current["kafkaTopic"].ToString() };

            Assert.IsNotNull(topics);

            var options = GetKafkaOptions(brokers);

            int topicCount = 0;
            foreach (string topic in topics)
            {
                topicCount++;
                try
                {
                    TopicOffset to = GetProducerTopicOffset(options, topic);
                    if (to.Items == 0)
                    {
                        if (to.PartitionId == -1)
                        {
                            topicJsons[topic] = new List<string> { "cannot connect to the Kafka server, exiting..." };
                            break;
                        }

                        string tmsg = to.Topic == null
                         ? $"kafka topic '{topic}' is invalid - not present on server"
                         : $"kafka topic '{topic}' does not have messages... FirstOffset: {to.FirstOffset}";
                        topicJsons[topic] = new List<string> { tmsg };
                        Console.WriteLine(tmsg);
                        continue;
                    }

                    // get last 5 messages, otherwise it chokes..
                    long foffs = to.FirstOffset;
                    if (to.Items > p0) foffs += to.Items - p0;

                    OffsetPosition offsetPosition = new OffsetPosition() { Offset = foffs, PartitionId = to.PartitionId };

                    #region v1, v2
                    //v1
                    //Console.WriteLine($" ---- start 1 ------- {DateTime.Now.ToString()}  topic:{topic}. {topicCount} of {topics.Count}");
                    //ConsumeMessagesTaskRun(topic, options, offsetPosition, seconds);

                    //v2 no results:
                    //Console.WriteLine($" ---- start 2 ------- {DateTime.Now.ToString()}  topic:{topic}. {topicCount} of {topics.Count}");
                    //ConsumeMessagesTaskRun(topic, options, offsetPosition, 0);
                    #endregion

                    // v3 no results:
                    Console.WriteLine($" ---- start 3 ------- {DateTime.Now.ToString()}  topic:{topic}. {topicCount} of {topics.Count}");
                    ConsumeMessages(topic, options, offsetPosition, seconds);

                    #region v4, v5
                    //v1
                    //Console.WriteLine($" ---- start 4 ------- {DateTime.Now.ToString()}  topic:{topic}. {topicCount} of {topics.Count}");
                    //ConsumeMessages(topic, options, offsetPosition, 0);

                    //v5
                    //var cancellationToken = new CancellationTokenSource(seconds * 1000);
                    //Task.Factory.StartNew(() => ConsumeMessages(topic, options, 0, offsetPosition), cancellationToken.Token);
                    //Thread.Sleep(5000); //simulate some other work
                    //cancellationToken.Cancel(false); //this stops the Task??  false indicates that no exceptions will be thrown.
                    #endregion

                    Console.WriteLine($" ---- end ------- {DateTime.Now.ToString()}  topic:{topic}. {topicCount} of {topics.Count}");
                    continue;

                }
                catch (Exception ex) { Console.WriteLine($" -----------  topic:{topic}. {ex.Message}"); }
            }
            LogJobs();
            Console.WriteLine("file://" + ProjTmp.Replace("\\", "/"));
        }

        // bad
        //[Then("I should retrieve from another SimpleKafka nuget last (.*) messages in (.*) seconds")]
        //public void Then_I_should_retrieve_from_another_nuget_kafka_last_P0_messages_in_P1_seconds(int lastMessages, int seconds)
        //{
        //    var kbrokers = ScenarioContext.Current["kafkaBrokers"] as List<string>;
        //    Assert.IsNotNull(kbrokers);

        //    var topics = ScenarioContext.Current["kafkaTopics"] as List<string>;

        //    var brokers = new SimpleKafka.KafkaBrokers(kbrokers.Select(o => new Uri("http://" + o)).ToArray());

        //    using (var broker = brokers)
        //    {
        //        var consumer = SimpleKafka.KafkaConsumer.Create(
        //            topics[0], brokers, 
        //            new SimpleKafka.StringSerializer(),
        //            new SimpleKafka.TopicSelector { Topic = topics[0], Partition = 0, Offset = 0 });
        //        var result = consumer.ReceiveAsync(CancellationToken.None).Result;
        //        foreach (var message in result)
        //        {
        //            Console.WriteLine("Received {0}", message.Value);
        //        }
        //    }
        //}

        [Given(@"I have json file")]
        [Given(@"I have xml content")]
        public void Given_I_have_json_file()
        {
        }

        [When(@"I parse it")]
        [When(@"I load it")]
        public void When_I_parse_it()
        {
            string exmlContent = Path.GetTempFileName();

            // Create C# from xml - copy xml, in vs menu select Edit/Paste special/xml as classes
            // https://dennymichael.net/2014/05/30/convert-xml-to-csharp-classes/comment-page-1/
            string xml = "<?xml version='1.0'?><Base><Person>me</Person><Person>you</Person>" +
                $"<Time>{DateTime.Now.ToString()}</Time>" +
                "</Base>";
            File.Delete(exmlContent);
            exmlContent += ".xml";
            File.WriteAllText(exmlContent, xml);
            Console.WriteLine("expected:\n\t" + File.ReadAllText(exmlContent));

            ScenarioContext.Current["exmlContent"] = exmlContent;
            ScenarioContext.Current["xml"] = xml;
        }

        /// <summary>
        /// get stream - leave it open for reading
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8, 512, true))
            {
                writer.Write(s);
                writer.Flush();
                stream.Position = 0;
            }
            return stream;
        }

        public static XDocument LoadXDocument(Stream exmlContent)
        {
            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit }; // { XmlResolver = new CustomUrlResovler() };
            var reader = XmlReader.Create(exmlContent, settings);
            XDocument exmlXDocument = XDocument.Load(reader);

            if (exmlXDocument == null)
                //todo log error
                throw new NotSupportedException("Could not load eXML file content");

            exmlContent.Position = 0;
            return exmlXDocument;
        }

        [Then(@"It should be loaded as xmldoc.Load\(string\) with CustomUrlResovler")]
        public void Then_It_should_be_loaded_with_CustomUrlResovler()
        {
            string exmlContent = ScenarioContext.Current["exmlContent"].ToString(); // file path 
            string xml = ScenarioContext.Current["xml"].ToString(); // file content       

            var stream = GenerateStreamFromString(xml);
            string streamValue = null;
            using (var reader = new StreamReader(stream))
            {
                streamValue = reader.ReadToEnd();
            }

            Console.WriteLine("expected stream:\n\t" + streamValue);

            XmlDocument xmlDocumentClaims = new XmlDocument() { XmlResolver = new CustomUrlResovler() };
            try
            {
                xmlDocumentClaims.Load(exmlContent);
            }
            catch (XmlException xmlE)
            {
                Console.WriteLine("Load stream 1 into XML document failed with error: " + xmlE.Message);
                throw xmlE;
            }

            Console.WriteLine("loaded file with CustomUrlResovler:\n" + xmlDocumentClaims.OuterXml);

            try
            {
                MemoryStream mStrm = new MemoryStream(Encoding.UTF8.GetBytes(xml));
                var settings = new XmlReaderSettings { XmlResolver = new CustomUrlResovler() };
                var reader = XmlReader.Create(mStrm, settings);
                XDocument exmlXDocument = XDocument.Load(reader);
                Console.WriteLine("loaded stream:\n" + exmlXDocument.ToString());
                mStrm.Dispose();
                reader.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Load stream into XML document failed with error: " + ex.Message);
            }
        }

        [Then(@"It should be loaded as xmldoc.Load\(xmlreader\) loaded with DtdProcessing.Prohibit")]
        public void Then_It_should_be_loaded_with_DtdProcessing_Prohibit()
        {
            string exmlContent = ScenarioContext.Current["exmlContent"].ToString(); // file path 
            string xml = ScenarioContext.Current["xml"].ToString(); // file content       

            var stream = GenerateStreamFromString(xml);

            // do not read strem - it will be closed after ReadToEnd 
            //string streamValue = null;
            //using (var reader = new StreamReader(stream)) { streamValue = reader.ReadToEnd(); }
            //Console.WriteLine("expected stream:\n\t" + streamValue);

            var xmlDocumentClaims = new XmlDocument();
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings() { DtdProcessing = DtdProcessing.Prohibit };
                XmlReader reader = XmlReader.Create(stream, settings);
                xmlDocumentClaims.Load(reader);
            }
            catch (XmlException xmlE)
            {
                Console.WriteLine("Load stream 2 into XML document failed with error: " + xmlE.Message);
                throw xmlE;
            }

            Console.WriteLine("loaded file with DtdProcessing.Prohibit:\n" + xmlDocumentClaims.OuterXml);
            try
            {
                MemoryStream mStrm = new MemoryStream(Encoding.UTF8.GetBytes(xml));
                var settings = new XmlReaderSettings { XmlResolver = new CustomUrlResovler() };
                var reader = XmlReader.Create(mStrm, settings);
                XDocument exmlXDocument = XDocument.Load(reader);
                Console.WriteLine("loaded stream:\n" + exmlXDocument.ToString());
                mStrm.Dispose();
                reader.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Load stream into XML document failed with error: " + ex.Message);
            }
        }

        public static bool ValidateExml(XDocument exmlXDocument) { return true; }

        [Then(@"It should be deserialized as xmlSerializer.Deserialize\(xmlreader\) with DtdProcessing.Prohibit")]
        public void Then_It_should_be_deserialized_as_xmlSerializer_Deserialize_xmlreader_with_DtdProcessing_Prohibit()
        {
            string xml = ScenarioContext.Current["xml"].ToString(); // file content    
            var stream = GenerateStreamFromString(xml);
            XDocument exmlDocument = LoadXDocument(stream);

            Base claimCore = null;
            try
            {
                if (ValidateExml(exmlDocument))
                {
                    var xmlSerializer = new XmlSerializer(typeof(Base));

                    XmlReaderSettings settings = new XmlReaderSettings() { DtdProcessing = DtdProcessing.Prohibit };
                    // read from xml string: XmlReader reader = XmlReader.Create(new StringReader(claimExml), settings);
                    // read from file:       XmlReader reader = XmlReader.Create(claimExml, settings);

                    XmlReader reader = XmlReader.Create(stream, settings);
                    claimCore = (Base)xmlSerializer.Deserialize(reader);
                    Assert.IsNotNull(claimCore);
                    Assert.IsTrue(claimCore.Person.Contains("me"));
                    Assert.IsTrue(claimCore.Person.Contains("you"));
                }
            }
            catch (XmlSchemaException e)
            {
                //todo log error
                Console.WriteLine("Deserialziation failed with error: " + e.Message);
                throw;
            }
        }

        [Then(@"It should be loaded as xdocument.Load\(xmlreader\) with DtdProcessing.Prohibit")]
        public void Then_It_should_be_loaded_as_xdocument_Load_xmlreader_with_DtdProcessing_Prohibit()
        {
            string xml = ScenarioContext.Current["xml"].ToString(); // file content    
            var exmlContent = GenerateStreamFromString(xml);

            try
            {
                XmlReaderSettings settings = new XmlReaderSettings() { DtdProcessing = DtdProcessing.Prohibit };
                //XmlReader reader = XmlReader.Create(stream, settings); //does not prevent XXE attack

                string value = null;
                using (var strreader = new StreamReader(exmlContent, Encoding.UTF8, false, 512, true))
                {
                    value = strreader.ReadToEnd();
                }
                XmlReader reader = XmlReader.Create(new StringReader(value), settings);
                XDocument exmlXDocument = XDocument.Load(reader);
                // check - need to leave stream opened
                exmlContent.Position = 0;

                Assert.IsNotNull(exmlXDocument);
                Assert.IsTrue(exmlXDocument.ToString().Contains("you"));
                Assert.IsTrue(((XElement)exmlXDocument.FirstNode).Name.LocalName == "Base");
            }
            catch (XmlSchemaException e)
            {
                //todo log error
                Console.WriteLine("Deserialziation failed with error: " + e.Message);
                throw;
            }
        }


        [Then(@"xml file should be cleaned")]
        public void Then_xml_file_should_be_cleaned()
        {
            string exmlContent = ScenarioContext.Current["exmlContent"].ToString(); // file path 
            File.Delete(exmlContent);
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
                var val = obj.SelectToken(jpath);
                var found = (string)obj.SelectToken(jpath);
                if (jval.Equals("null", StringComparison.OrdinalIgnoreCase)) jval = null;
                Assert.AreEqual(found, jval);
            }
        }

        #region Setup/Teardown

        [BeforeScenario]
        public void SetupKafka()
        {
            // just place holder                     
        }

        #endregion

    }
}