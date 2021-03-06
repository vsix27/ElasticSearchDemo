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
namespace ElasticTest.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("ElasticSearch using Elastic.Net package")]
    [NUnit.Framework.CategoryAttribute("Elastic_Search")]
    public partial class ElasticSearchUsingElastic_NetPackageFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ElasticSearch.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ElasticSearch using Elastic.Net package", "In order to proof concept \nI want to demonstrated CRUD operations", ProgrammingLanguage.CSharp, new string[] {
                        "Elastic_Search"});
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
        
        public virtual void FeatureBackground()
        {
#line 6
#line hidden
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("See Elastic - Health")]
        public virtual void SeeElastic_Health()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("See Elastic - Health", ((string[])(null)));
#line 17
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 18
 testRunner.Given("Elastic is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 19
 testRunner.When("I run <GET /_cluster/health>", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 20
 testRunner.Then("the result should not be empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve Item from Elastic Search - CRUD")]
        public virtual void RetrieveItemFromElasticSearch_CRUD()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve Item from Elastic Search - CRUD", ((string[])(null)));
#line 22
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 23
  testRunner.Given("Elastic is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 24
  testRunner.When("I get index <library> with type <books> with id <2>", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 25
  testRunner.Then("this item should be found", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve NON Existing Item from Elastic Search - CRUD")]
        public virtual void RetrieveNONExistingItemFromElasticSearch_CRUD()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve NON Existing Item from Elastic Search - CRUD", ((string[])(null)));
#line 27
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 28
  testRunner.Given("Elastic is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 29
  testRunner.When("I get index <library> with type <books> with id <GUID>", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 30
  testRunner.Then("this item should not be found", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add Item to Elastic Search - CRUD")]
        public virtual void AddItemToElasticSearch_CRUD()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add Item to Elastic Search - CRUD", ((string[])(null)));
#line 32
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 33
  testRunner.Given("Elastic is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "line"});
            table1.AddRow(new string[] {
                        "{"});
            table1.AddRow(new string[] {
                        "\"title\": \"YET MORE pads about it\","});
            table1.AddRow(new string[] {
                        "\"name\": {"});
            table1.AddRow(new string[] {
                        "\"first\": \"nino\","});
            table1.AddRow(new string[] {
                        "\"last\": \"rota\""});
            table1.AddRow(new string[] {
                        "},"});
            table1.AddRow(new string[] {
                        "\"publish_date\": \"2010-03-27T06:11:22-0400\","});
            table1.AddRow(new string[] {
                        "\"price\": 55.55,"});
            table1.AddRow(new string[] {
                        "\"itin\": \"<GUID>\""});
            table1.AddRow(new string[] {
                        "}"});
#line 34
  testRunner.When("I put index <library> with type <books> with id <51> with json", ((string)(null)), table1, "When ");
#line 46
  testRunner.Then("this item should be found", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Update Item in Elastic Search - CRUD")]
        public virtual void UpdateItemInElasticSearch_CRUD()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Update Item in Elastic Search - CRUD", ((string[])(null)));
#line 48
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 49
  testRunner.Given("Elastic is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "line"});
            table2.AddRow(new string[] {
                        "{"});
            table2.AddRow(new string[] {
                        "\"title\": \"yet another book about GUID\","});
            table2.AddRow(new string[] {
                        "\"itin\": \"GUID\""});
            table2.AddRow(new string[] {
                        "}"});
#line 50
  testRunner.When("I update index <library> with type <books> with id <2> with json", ((string)(null)), table2, "When ");
#line 56
  testRunner.Then("this item should be found with new version", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Delete Item from Elastic Search - CRUD")]
        public virtual void DeleteItemFromElasticSearch_CRUD()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete Item from Elastic Search - CRUD", ((string[])(null)));
#line 58
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 59
  testRunner.Given("Elastic is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "line"});
            table3.AddRow(new string[] {
                        "{"});
            table3.AddRow(new string[] {
                        "\"title\": \"real book about GUID\","});
            table3.AddRow(new string[] {
                        "\"itin\": \"<GUID>\""});
            table3.AddRow(new string[] {
                        "}"});
#line 60
  testRunner.When("I put index <library> with type <books> with id <GUID> with json", ((string)(null)), table3, "When ");
#line 66
 testRunner.And("delete this item", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 67
  testRunner.Then("this item should not be found", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add Item -location- to Elastic Search - CRUD")]
        public virtual void AddItem_Location_ToElasticSearch_CRUD()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add Item -location- to Elastic Search - CRUD", ((string[])(null)));
#line 70
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 71
  testRunner.Given("Elastic is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "line"});
            table4.AddRow(new string[] {
                        "{\"identifier\":[{\"system\":\"2.16.840.1.113883.4.6\",\"value\":\"5812345679\",\"use\":\"Offi" +
                            "cial\"},"});
            table4.AddRow(new string[] {
                        "{\"system\":\"2.16.840.1.113883.4.4\",\"value\":\"123456789\",\"use\":\"Secondary\"},"});
            table4.AddRow(new string[] {
                        "{\"system\":\"LU\",\"value\":\"123456789\",\"use\":\"Secondary\"}],\"name\":\"KILDARE ASSOCIATES" +
                            "\","});
            table4.AddRow(new string[] {
                        "\"address\":{\"use\":\"Work\",\"addressLine\":[\"2345 OCEAN BLVD\"],\"city\":\"MIAMI\",\"region\"" +
                            ":\"FL\",\"postalCode\":\"33111\"},"});
            table4.AddRow(new string[] {
                        "\"resourceId\":\"8ce8732c-df22-4bb5-af65-0ef4e50e1473\","});
            table4.AddRow(new string[] {
                        "\"extension\":[{\"url\":\"urn:resourceDocumentPath\",\"value\":{\"string\":\"Claim/1/Locatio" +
                            "n/1\"}}],"});
            table4.AddRow(new string[] {
                        "\"meta\":{\"sourceMessageType\":\"eXML\",\"sourceMessageDate\":{\"value\":63606018661667928" +
                            "6,\"text\":\"2016-08-05 18:31:01\"},"});
            table4.AddRow(new string[] {
                        "\"acquiredDate\":{\"value\":636060186642581062,\"text\":\"2016-08-05 18:31:04\"},\"sourceD" +
                            "ocumentId\":\"eXML_encounter_EB_test.xml\"},"});
            table4.AddRow(new string[] {
                        "\"ResourceDocumentPath\":\"QOLU3jlvHEmqG6Cs0cHBaqEl0Jo+vQtmfYg4N2TBgYpMr/bjRib1mN9U0" +
                            "8QjP835KUxDNY3yQQva/LOl4GHz2A==/Claim/1/Location/1\"}"});
#line 72
  testRunner.When("I put index <location> with type <location> with id <1> with json", ((string)(null)), table4, "When ");
#line 83
  testRunner.Then("this item should be found", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add Item -patient- to Elastic Search - CRUD")]
        public virtual void AddItem_Patient_ToElasticSearch_CRUD()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add Item -patient- to Elastic Search - CRUD", ((string[])(null)));
#line 86
this.ScenarioSetup(scenarioInfo);
#line 6
this.FeatureBackground();
#line 87
  testRunner.Given("Elastic is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "line"});
            table5.AddRow(new string[] {
                        "{"});
            table5.AddRow(new string[] {
                        "\"title\": \"resourceId: f7b91b47-3c94-4927-a6ce-0b89fbfb23bd-4\","});
            table5.AddRow(new string[] {
                        "\"name\": {"});
            table5.AddRow(new string[] {
                        "\"first\": \"reference: Organization/organizationid-1\","});
            table5.AddRow(new string[] {
                        "\"last\": \"ResourceDocumentPath: t674bcmehboqkrj3nb96teh34duqnyqbhy5nzfxwxpbcs3yago" +
                            "my/Patient/1\""});
            table5.AddRow(new string[] {
                        "},"});
            table5.AddRow(new string[] {
                        "\"publish_date\": \"2010-03-27T06:11:22-0400\","});
            table5.AddRow(new string[] {
                        "\"price\": 55.55,"});
            table5.AddRow(new string[] {
                        "\"itin\": \"code: OntologyPatient-74b012c1-05d7-4846-8ca9-94e8cfd9131e-2\""});
            table5.AddRow(new string[] {
                        "}"});
#line 88
  testRunner.When("I put index <library> with type <books> with id <49> with json", ((string)(null)), table5, "When ");
#line 100
  testRunner.Then("this item should be found", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
