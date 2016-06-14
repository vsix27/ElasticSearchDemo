﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.42000
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Kafka.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("KafkaBrokers")]
    public partial class KafkaBrokersFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "KafkaBrokers.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "KafkaBrokers", "In order to avoid silly mistakes\r\nAs an Kafka amateur\r\nI want to consume Kafka me" +
                    "tadata", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Able to produce and consume kafka - 172.26.11.135 - messages")]
        [NUnit.Framework.CategoryAttribute("FileProc_Kafka_produce")]
        public virtual void AbleToProduceAndConsumeKafka_172_26_11_135_Messages()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Able to produce and consume kafka - 172.26.11.135 - messages", new string[] {
                        "FileProc_Kafka_produce"});
#line 11
this.ScenarioSetup(scenarioInfo);
#line 12
  testRunner.Given("I have Random expressions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 13
  testRunner.When("I send it to kafka 172.26.11.135:9092 server to TestMessage topic", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 14
  testRunner.Then("I should consume it in 0 seconds", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Able to produce and consume kafka - shortfusedev-dn9.westus.cloudapp.azure.com:90" +
            "92 - messages to Claim topic")]
        [NUnit.Framework.CategoryAttribute("FileProc_Kafka_produce")]
        public virtual void AbleToProduceAndConsumeKafka_Shortfusedev_Dn9_Westus_Cloudapp_Azure_Com9092_MessagesToClaimTopic()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Able to produce and consume kafka - shortfusedev-dn9.westus.cloudapp.azure.com:90" +
                    "92 - messages to Claim topic", new string[] {
                        "FileProc_Kafka_produce"});
#line 17
this.ScenarioSetup(scenarioInfo);
#line 18
  testRunner.Given("I have Random expressions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 19
  testRunner.When("I send it to kafka shortfusedev-dn9.westus.cloudapp.azure.com:9092 server to fuse" +
                    "test topic", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 20
  testRunner.Then("I should consume it in 0 seconds", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Able to produce/consume kafka")]
        [NUnit.Framework.CategoryAttribute("FileProc_Kafka_produce")]
        [NUnit.Framework.TestCaseAttribute("fusetest", null)]
        [NUnit.Framework.TestCaseAttribute("fusetest2", null)]
        [NUnit.Framework.TestCaseAttribute("TestMessage", null)]
        [NUnit.Framework.TestCaseAttribute("Practitioner", null)]
        [NUnit.Framework.TestCaseAttribute("Claim", null)]
        [NUnit.Framework.TestCaseAttribute("Coverage", null)]
        [NUnit.Framework.TestCaseAttribute("Location", null)]
        public virtual void AbleToProduceConsumeKafka(string topic, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "FileProc_Kafka_produce"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Able to produce/consume kafka", @__tags);
#line 23
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "kafka broker"});
            table1.AddRow(new string[] {
                        "shortfusedev-dn9.westus.cloudapp.azure.com:9092"});
            table1.AddRow(new string[] {
                        "shortfusedev-dn8.westus.cloudapp.azure.com:9092"});
            table1.AddRow(new string[] {
                        "shortfusedev-dn10.westus.cloudapp.azure.com:9092"});
            table1.AddRow(new string[] {
                        "shortfusedev-dn11.westus.cloudapp.azure.com:9092"});
#line 24
  testRunner.Given("I have brokers for kafka", ((string)(null)), table1, "Given ");
#line 30
  testRunner.And("I have Random expressions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 31
  testRunner.When(string.Format("I send it to kafka 172.26.8.26:9092 server to {0} topic", topic), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 32
  testRunner.Then("I should consume it in 0 seconds", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Able to produce and consume kafka - shortfusedev - messages to topic")]
        [NUnit.Framework.CategoryAttribute("FileProc_Kafka_produce")]
        public virtual void AbleToProduceAndConsumeKafka_Shortfusedev_MessagesToTopic()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Able to produce and consume kafka - shortfusedev - messages to topic", new string[] {
                        "FileProc_Kafka_produce"});
#line 46
this.ScenarioSetup(scenarioInfo);
#line 47
  testRunner.Given("I have Random expressions", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 48
  testRunner.When("I send it to kafka shortfusedev-dn9.westus.cloudapp.azure.com:9092 server to fuse" +
                    "test topic", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 49
  testRunner.Then("I should consume it in 0 seconds", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Able to consume kafka - shortfusedev")]
        [NUnit.Framework.CategoryAttribute("FileProc_Kafka_consume")]
        public virtual void AbleToConsumeKafka_Shortfusedev()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Able to consume kafka - shortfusedev", new string[] {
                        "FileProc_Kafka_consume"});
#line 54
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "kafka broker"});
            table2.AddRow(new string[] {
                        "shortfusedev-dn10.westus.cloudapp.azure.com:9092"});
            table2.AddRow(new string[] {
                        "shortfusedev-dn9.westus.cloudapp.azure.com:9092"});
            table2.AddRow(new string[] {
                        "shortfusedev-dn8.westus.cloudapp.azure.com:9092"});
            table2.AddRow(new string[] {
                        "shortfusedev-dn11.westus.cloudapp.azure.com:9092"});
#line 55
  testRunner.Given("I have brokers for kafka", ((string)(null)), table2, "Given ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "topic",
                        "info"});
            table3.AddRow(new string[] {
                        "fusetest",
                        "4 messages"});
            table3.AddRow(new string[] {
                        "fusetest2",
                        "4 messages"});
            table3.AddRow(new string[] {
                        "Practitioner",
                        "10 messages"});
            table3.AddRow(new string[] {
                        "Patient",
                        "4 messages"});
            table3.AddRow(new string[] {
                        "Procedure",
                        "4 messages"});
            table3.AddRow(new string[] {
                        "Organization",
                        "4 messages  v1"});
            table3.AddRow(new string[] {
                        "Claim",
                        "4 messages  v1"});
            table3.AddRow(new string[] {
                        "Coverage",
                        "2 messages  v1"});
            table3.AddRow(new string[] {
                        "Location",
                        "2 messages  v1"});
            table3.AddRow(new string[] {
                        "Condition",
                        "FirstOffset: 258; Items: 7"});
            table3.AddRow(new string[] {
                        "DiagnosticReport",
                        "FirstOffset: 61; Items: 2 (53, 10)"});
            table3.AddRow(new string[] {
                        "Encounter",
                        "2 messages  v1"});
            table3.AddRow(new string[] {
                        "FamilyMemberHistory",
                        "FirstOffset: 16; Items: 2"});
            table3.AddRow(new string[] {
                        "Immunization",
                        "FirstOffset: 129; Items: 3"});
            table3.AddRow(new string[] {
                        "Medication",
                        "FirstOffset: 396; Items: 4"});
            table3.AddRow(new string[] {
                        "AllergyIntolerance",
                        "FirstOffset: 51; Items: 8"});
#line 61
  testRunner.And("I have topics for kafka", ((string)(null)), table3, "And ");
#line 79
  testRunner.When("I call kafka server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 80
  testRunner.Then("I should retrieve last 2 messages in -1 seconds", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Able to get data folder")]
        [NUnit.Framework.CategoryAttribute("FileProc_Kafka_datafolder")]
        public virtual void AbleToGetDataFolder()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Able to get data folder", new string[] {
                        "FileProc_Kafka_datafolder"});
#line 88
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "kafka broker"});
            table4.AddRow(new string[] {
                        "172.26.8.26:9092"});
#line 89
  testRunner.Given("I have brokers for kafka", ((string)(null)), table4, "Given ");
#line 92
  testRunner.When("I call kafka server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 93
  testRunner.Then("data folder is created if missing", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Able to consume kafka all topics")]
        [NUnit.Framework.CategoryAttribute("FileProc_Kafka_consume")]
        public virtual void AbleToConsumeKafkaAllTopics()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Able to consume kafka all topics", new string[] {
                        "FileProc_Kafka_consume"});
#line 101
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "kafka broker"});
            table5.AddRow(new string[] {
                        "172.26.8.26:9092"});
            table5.AddRow(new string[] {
                        "172.26.8.27:9092"});
            table5.AddRow(new string[] {
                        "172.26.8.28:9092"});
            table5.AddRow(new string[] {
                        "172.26.8.29:9092"});
#line 102
  testRunner.Given("I have brokers for kafka", ((string)(null)), table5, "Given ");
#line 108
  testRunner.And("I have topic list for kafka dummy, jnmtopic, JnmTopic, fusetest, Claim,", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 109
  testRunner.And("I have topic list for kafka Coverage, Immunization, Location,fusetest1, fusetest2" +
                    ", fusetest3", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 110
  testRunner.And("I have topic list for kafka Medication, Organization, Patient, Procedure, Practit" +
                    "ioner", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 111
  testRunner.When("I call kafka server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 112
  testRunner.Then("I should retrieve last 2 messages in -1 seconds", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Able to consume kafka topic")]
        [NUnit.Framework.CategoryAttribute("FileProc_Kafka_consume")]
        [NUnit.Framework.TestCaseAttribute("fusetest", "A messages", null)]
        [NUnit.Framework.TestCaseAttribute("fusetest2", "B messages", null)]
        [NUnit.Framework.TestCaseAttribute("Practitioner", "10 messages", null)]
        [NUnit.Framework.TestCaseAttribute("Location", "2 messages  v1", null)]
        [NUnit.Framework.TestCaseAttribute("Patient", "4 messages", null)]
        [NUnit.Framework.TestCaseAttribute("Procedure", "4 messages", null)]
        [NUnit.Framework.TestCaseAttribute("Organization", "4 messages  v1", null)]
        [NUnit.Framework.TestCaseAttribute("Claim", "4 messages  v1", null)]
        [NUnit.Framework.TestCaseAttribute("Coverage", "2 messages  v1", null)]
        public virtual void AbleToConsumeKafkaTopic(string topic, string info, string[] exampleTags)
        {
            string[] @__tags = new string[] {
                    "FileProc_Kafka_consume"};
            if ((exampleTags != null))
            {
                @__tags = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Concat(@__tags, exampleTags));
            }
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Able to consume kafka topic", @__tags);
#line 115
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table6 = new TechTalk.SpecFlow.Table(new string[] {
                        "kafka broker"});
            table6.AddRow(new string[] {
                        "172.26.8.26:9092"});
            table6.AddRow(new string[] {
                        "172.26.8.27:9092"});
            table6.AddRow(new string[] {
                        "172.26.8.28:9092"});
            table6.AddRow(new string[] {
                        "172.26.8.29:9092"});
#line 116
  testRunner.Given("I have brokers for kafka", ((string)(null)), table6, "Given ");
#line 122
  testRunner.And(string.Format("I have kafka {0}", topic), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 123
  testRunner.When("I call kafka server", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 124
  testRunner.Then("I should retrieve last 2 messages in -1 seconds", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Able to parse json message Patient")]
        [NUnit.Framework.CategoryAttribute("FileProc_Json")]
        public virtual void AbleToParseJsonMessagePatient()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Able to parse json message Patient", new string[] {
                        "FileProc_Json"});
#line 157
  this.ScenarioSetup(scenarioInfo);
#line 158
  testRunner.Given("I have json file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 159
  testRunner.When("I parse it", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table7 = new TechTalk.SpecFlow.Table(new string[] {
                        "jsonpath",
                        "value"});
            table7.AddRow(new string[] {
                        "name[0].family[0]",
                        "LAMBERT"});
            table7.AddRow(new string[] {
                        "address[0].addressLine[0]",
                        "7881 Metus Street"});
            table7.AddRow(new string[] {
                        "address[0].addressLine[1]",
                        "none"});
            table7.AddRow(new string[] {
                        "address[0].addressLine[2]",
                        "null"});
#line 160
  testRunner.Then("I should be find in file <messages\\Patient\\item_215.json> matching json values", ((string)(null)), table7, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Able to parse json message Condition")]
        [NUnit.Framework.CategoryAttribute("FileProc_Json")]
        public virtual void AbleToParseJsonMessageCondition()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Able to parse json message Condition", new string[] {
                        "FileProc_Json"});
#line 168
  this.ScenarioSetup(scenarioInfo);
#line 169
  testRunner.Given("I have json file", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 170
  testRunner.When("I parse it", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
            TechTalk.SpecFlow.Table table8 = new TechTalk.SpecFlow.Table(new string[] {
                        "jsonpath",
                        "value"});
            table8.AddRow(new string[] {
                        "category.coding[?(@.code==\'ccc\')].system",
                        "xxx"});
            table8.AddRow(new string[] {
                        "code.coding[0].code",
                        "V04.81"});
            table8.AddRow(new string[] {
                        "category.coding[0].code",
                        "55607006"});
            table8.AddRow(new string[] {
                        "category.coding[0].primary",
                        "True"});
            table8.AddRow(new string[] {
                        "onset.onsetDateTime.offset",
                        "0"});
#line 171
  testRunner.Then("I should be find in file <messages\\Condition\\item_257.json> matching json values", ((string)(null)), table8, "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Able to parse xml message with XmlResolver")]
        [NUnit.Framework.CategoryAttribute("Xml_validation_XXE")]
        public virtual void AbleToParseXmlMessageWithXmlResolver()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Able to parse xml message with XmlResolver", new string[] {
                        "Xml_validation_XXE"});
#line 181
this.ScenarioSetup(scenarioInfo);
#line 182
testRunner.Given("I have xml content", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 183
testRunner.When("I load it", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 184
testRunner.Then("It should be loaded as xmldoc.Load(xmlreader) loaded with DtdProcessing.Prohibit", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 185
 testRunner.And("It should be loaded as xmldoc.Load(string) with CustomUrlResovler", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 186
 testRunner.And("It should be loaded as xdocument.Load(xmlreader) with DtdProcessing.Prohibit", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 187
 testRunner.And("It should be deserialized as xmlSerializer.Deserialize(xmlreader) with DtdProcess" +
                    "ing.Prohibit", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 188
 testRunner.And("xml file should be cleaned", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
