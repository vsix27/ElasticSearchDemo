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
    [NUnit.Framework.DescriptionAttribute("ElasticSearch using NEST package")]
    [NUnit.Framework.CategoryAttribute("Elastic_NEST")]
    public partial class ElasticSearchUsingNESTPackageFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ElasticNest.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "ElasticSearch using NEST package", "In order to proof concept \nI want to demonstrated CRUD operations", ProgrammingLanguage.CSharp, new string[] {
                        "Elastic_NEST"});
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
        [NUnit.Framework.DescriptionAttribute("See Elastic Health with NEST")]
        [NUnit.Framework.CategoryAttribute("Health")]
        public virtual void SeeElasticHealthWithNEST()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("See Elastic Health with NEST", new string[] {
                        "Health"});
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("Elastic is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 9
 testRunner.When("I run <GET /_cluster/health>", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 10
 testRunner.Then("the result should not be empty", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve Item from Elastic Search with NEST")]
        [NUnit.Framework.CategoryAttribute("CRUD_NEST")]
        public virtual void RetrieveItemFromElasticSearchWithNEST()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve Item from Elastic Search with NEST", new string[] {
                        "CRUD_NEST"});
#line 13
this.ScenarioSetup(scenarioInfo);
#line 14
  testRunner.Given("Elastic is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 15
  testRunner.When("I get index <nestlibrary> with type <calendar> with id <2>", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 16
  testRunner.Then("this item should be found", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Retrieve NON Existing Item from Elastic Search with NEST")]
        [NUnit.Framework.CategoryAttribute("CRUD_NEST")]
        public virtual void RetrieveNONExistingItemFromElasticSearchWithNEST()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Retrieve NON Existing Item from Elastic Search with NEST", new string[] {
                        "CRUD_NEST"});
#line 19
this.ScenarioSetup(scenarioInfo);
#line 20
  testRunner.Given("Elastic is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 21
  testRunner.When("I get index <nestlibrary> with type <calendar> with id <GUID>", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 22
  testRunner.Then("this item should not be found", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Add Item to Elastic Search with NEST")]
        [NUnit.Framework.CategoryAttribute("CRUD_NEST")]
        public virtual void AddItemToElasticSearchWithNEST()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Add Item to Elastic Search with NEST", new string[] {
                        "CRUD_NEST"});
#line 25
this.ScenarioSetup(scenarioInfo);
#line 26
  testRunner.Given("Elastic is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "line"});
            table1.AddRow(new string[] {
                        "{  }"});
#line 27
  testRunner.When("I put index <nestlibrary> with type <calendar> with id <2> with json", ((string)(null)), table1, "When ");
#line 30
  testRunner.Then("this item should be found", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Update Item in Elastic Search with NEST")]
        [NUnit.Framework.CategoryAttribute("CRUD_NEST")]
        public virtual void UpdateItemInElasticSearchWithNEST()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Update Item in Elastic Search with NEST", new string[] {
                        "CRUD_NEST"});
#line 33
this.ScenarioSetup(scenarioInfo);
#line 34
  testRunner.Given("Elastic is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "line"});
            table2.AddRow(new string[] {
                        "{ }"});
#line 35
  testRunner.When("I update index <nestlibrary> with type <calendar> with id <2> with json", ((string)(null)), table2, "When ");
#line 38
  testRunner.Then("this item should be found with new version", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Delete Item from Elastic Search with NEST")]
        [NUnit.Framework.CategoryAttribute("CRUD_NEST")]
        public virtual void DeleteItemFromElasticSearchWithNEST()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Delete Item from Elastic Search with NEST", new string[] {
                        "CRUD_NEST"});
#line 41
this.ScenarioSetup(scenarioInfo);
#line 42
  testRunner.Given("Elastic is running", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "line"});
            table3.AddRow(new string[] {
                        "{ }"});
#line 43
  testRunner.When("I put index <nestlibrary> with type <calendar> with id <GUID> with json", ((string)(null)), table3, "When ");
#line 46
 testRunner.And("delete this item", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 47
  testRunner.Then("this item should not be found", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
