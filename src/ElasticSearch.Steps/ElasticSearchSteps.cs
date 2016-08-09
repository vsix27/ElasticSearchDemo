using Elasticsearch.Net;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using TechTalk.SpecFlow;

namespace ElasticTest.Steps
{
    [Binding]
    public class ElasticSearchSteps
    {
        ElasticHelper eh = new ElasticHelper();
        NestHelper nh = new NestHelper();

        #region Setup/Teardown

        [BeforeScenario]
        public void Setup()
        {
            //ScenarioContext.Current["Files"] = new List<string>();
            //ScenarioContext.Current["Inputs"] = new List<Dictionary<string, string>>();
            //ScenarioContext.Current["Uri"] = ConfigurationManager.AppSettings["Uri"];
        }

        [AfterScenario]
        public void Teardown()
        {

        }

        #endregion

        [Given(@"Elastic is running")]
        public void GivenElasticIsRunning()
        {
            Assert.IsTrue(eh.Client != null);
            /*
                        var myJson = @"{ ""hello"" : ""world"" }";
            var oo = Client.Index<string>("library", "books", "1", myJson);
            txtOutput.Text = oo.Body;
            DebugElasticsearchResponsep(oo);

            //var configNs = new Nest.ConnectionSettings(node);
            //var clientNs = new Nest.ElasticClient(configNs);

            //clientEs.GetSource("myindex", "mytype", "1", qs => qs.Routing("routingvalue"));
            var z = Client.GetSource<string>("library", "books", "2");
            txtOutput.Text = z.Body;
            DebugElasticsearchResponsep(z);
            */
        }

        private string clear(string s)
        {
            return ("" + s).Replace("<", "").Replace(">", "");
        }

        private bool ScenarioNest
        {
            get { return ScenarioContext.Current.ScenarioInfo.Title.EndsWith(" NEST", StringComparison.OrdinalIgnoreCase); }
        }

        [When(@"I run (.*)")]
        public void WhenIRun(string p0)
        {
            p0 = clear(p0).Replace("/?", "?");

            //ScenarioContext.Current["WhenIRun"] = p0;
            if (p0.StartsWith("GET /_cluster/health", StringComparison.OrdinalIgnoreCase))
            {
                if (!ScenarioNest)
                {
                    // var z = eh.Client.CatHealthAsync<string>().Result;
                    var z = eh.Client.CatHealth<string>();
                    ScenarioContext.Current["Health"] = z.Success ? z.Body : null;
                    ElasticHelper.DebugElasticsearchResponse(z);
                }
                else
                {
                    var z = nh.Client.CatHealth();
                    ScenarioContext.Current["Health"] = z.ApiCall.Success ? z.ApiCall.Uri : null;
                    NestHelper.DebugICatResponse(z);
                }
            }
        }

        [Then(@"the result should not be empty")]
        public void ThenTheResultShouldNotBeEmpty()
        {
            string zBody = "" + ScenarioContext.Current["Health"];
            Assert.IsTrue(!string.IsNullOrWhiteSpace(zBody));
            Console.WriteLine(zBody);
        }

        [When(@"I put index (.*) with type (.*) with id (.*) with json")]
        public void When_I_put_index_P0_with_type_P1_with_id_P2_with_json(string p0, string p1, string p2, Table table)
        {
            p0 = clear(p0);
            p1 = clear(p1);
            p2 = clear(p2).Replace("GUID", Guid.NewGuid().ToString("N"));

            // make json
            string s = string.Empty;
            foreach (var row in table.Rows)
                s += row[0].Replace("<GUID>", Guid.NewGuid().ToString("N"));

            if (!ScenarioNest)
            {
                var found = eh.Client.Index<string>(p0, p1, p2, s);
                ElasticHelper.DebugElasticsearchResponse(found);
                ScenarioContext.Current["found"] = found.Body;
                Assert.IsTrue(found.Success);
            }
            else
            {
                var book = new Book
                {
                    title = "Gone to the lunch",
                    name = new name { first = "Jim", last = "Monroe" },
                    publish_date = DateTime.Parse("2010-01-27"),
                    price = 12.34m,
                    itin = "1324",
                };

                var found = nh.Client.Index(book, o => o.Index(p0).Type(p1).Id(p2).Refresh());

                var dpObj = new Nest.DocumentPath<object>(new Nest.Id(p2)).Index(p0).Type(p1);
                var foundO = nh.Client.Get(dpObj);
                NestHelper.DebugGetResponse(foundO, "object");

                var jfound = JObject.Parse(foundO.Source.ToString());
                jfound["_version"] = foundO.Version;
                jfound["_index"] = p0;
                jfound["_type"] = p1;
                jfound["_id"] = p2;

                ScenarioContext.Current["found"] = jfound;

                NestHelper.DebugIndexResponse(found);                 
            }
        }
        
        [Then(@"this item should be found")]
        public void ThenThisItemShouldBeFound()
        {
            Assert.IsTrue(ScenarioContext.Current["found"] != null);
        }

        [When(@"I get index (.*) with type (.*) with id (.*)")]
        public void When_I_get_index_P0_with_type_P1_with_id_P2(string p0, string p1, string p2)
        {
            p0 = clear(p0);
            p1 = clear(p1);
            p2 = clear(p2).Replace("GUID", Guid.NewGuid().ToString("N"));

            if (!ScenarioNest)
            {
                var found = eh.Client.Get<string>(p0, p1, p2);
                ElasticHelper.DebugElasticsearchResponse(found, "found");
                ScenarioContext.Current["found"] = found.Body;
            }
            else
            {
                var dpObj = new Nest.DocumentPath<object>(new Nest.Id(p2)).Index(p0).Type(p1);
                var foundO = nh.Client.Get(dpObj);
                NestHelper.DebugGetResponse(foundO, "object");

                Assert.IsNotNull(foundO);

                if (foundO.Source == null)
                {
                    ScenarioContext.Current["found"] = null;
                    return;
                }


                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(foundO.Source));
                var jfound = JObject.Parse(foundO.Source.ToString());
                jfound["_version"] = foundO.Version;
                jfound["_index"] = p0;
                jfound["_type"] = p1;
                jfound["_id"] = p2;

                ScenarioContext.Current["found"] = jfound;

                #region cast as a Book
                var dpBook = new Nest.DocumentPath<Book>(new Nest.Id(p2)).Index(p0).Type(p1);
                var found = nh.Client.Get(dpBook);
                NestHelper.DebugGetResponse(found, "Book");
                var foundBook = found.Source as Book;
                if (foundBook != null)
                    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(foundBook));
                else
                    Console.WriteLine("cannot cast found object to Book");
                #endregion
                
                #region cast as a Pad - only matching properties present
                var dpPad = new Nest.DocumentPath<Pad>(new Nest.Id(p2)).Index(p0).Type(p1);
                var found2 = nh.Client.Get(dpPad);
                NestHelper.DebugGetResponse(found2, "Pad");
                var foundPad = found2.Source as Pad;
                if (foundPad != null)
                    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(foundPad));
                else
                    Console.WriteLine("cannot cast found2 object to Pad");
                #endregion
            }
        }

        [When]
        public void When_delete_this_item()
        {
            Assert.IsNotNull(ScenarioContext.Current["found"]);
            JObject jfound = JObject.Parse("" + ScenarioContext.Current["found"]);

            string versionFound = jfound["_version"].ToString();

            string p0 = jfound["_index"].ToString();
            string p1 = jfound["_type"].ToString();
            string p2 = jfound["_id"].ToString();

            if (!ScenarioNest)
            {
                var deleted = eh.Client.Delete<string>(p0, p1, p2);
                ElasticHelper.DebugElasticsearchResponse(deleted, "deleted");

                var found = eh.Client.Get<string>(p0, p1, p2);
                ScenarioContext.Current["found"] = found.Body;
            }
            else
            {
                var dpObj = new Nest.DocumentPath<object>(new Nest.Id(p2)).Index(p0).Type(p1);
                var foundO = nh.Client.Delete(dpObj);
                NestHelper.DebugDeleteResponse(foundO, "object");

                var found1 = nh.Client.Get(dpObj);
                Assert.IsFalse(found1.IsValid);
                ScenarioContext.Current["found"] = found1.Source;
            }
        }

        [Then]
        public void Then_this_item_should_not_be_found()
        {
            Assert.IsNull(ScenarioContext.Current["found"]);           
        }

        [When(@"I update index (.*) with type (.*) with id (.*) with json")]
        public void When_I_update_index_P0_with_type_P1_with_id_P2_with_json(string p0, string p1, string p2, Table table)
        {
            p0 = clear(p0);
            p1 = clear(p1);
            p2 = clear(p2);

            if (!ScenarioNest)
            {
                var found = eh.Client.Get<string>(p0, p1, p2);
                ElasticHelper.DebugElasticsearchResponse(found, "found");
                ScenarioContext.Current["found"] = found.Body;

                // make json
                string s = string.Empty;
                foreach (var row in table.Rows) s += row[0].Replace("GUID", Guid.NewGuid().ToString("N"));
                var updated = eh.Client.Index<string>(p0, p1, p2, s);
                ElasticHelper.DebugElasticsearchResponse(updated, "updated");
                ScenarioContext.Current["updated"] = updated.Body;
            }
            else
            {
                var book = new Book
                {
                    title = "Gone to the lunch",
                    name = new name { first = "Jim", last = "Monroe" },
                    publish_date = DateTime.Parse("2010-01-27"),
                    price = 12.34m,
                    itin = "1324",
                };

                var found = nh.Client.Index(book, o => o.Index(p0).Type(p1).Id(p2).Refresh());
                NestHelper.DebugIndexResponse(found);
                var dpObj = new Nest.DocumentPath<object>(new Nest.Id(p2)).Index(p0).Type(p1);
                var foundO = nh.Client.Get(dpObj);
                NestHelper.DebugGetResponse(foundO, "object found");

                var jfound = JObject.Parse(foundO.Source.ToString());
                jfound["_version"] = found.Version;
                jfound["_index"] = p0;
                jfound["_type"] = p1;
                jfound["_id"] = p2;
                ScenarioContext.Current["found"] = jfound;

                book.title += " again";

                var updated = nh.Client.Index(book, o => o.Index(p0).Type(p1).Id(p2).Refresh());
                NestHelper.DebugIndexResponse(updated);
                var dpObjUpd = new Nest.DocumentPath<object>(new Nest.Id(p2)).Index(p0).Type(p1);
                var foundOUpd = nh.Client.Get(dpObjUpd);
                NestHelper.DebugGetResponse(foundOUpd, "object updated");

                var jfoundUpd = JObject.Parse(foundO.Source.ToString());
                jfoundUpd["_version"] = foundOUpd.Version;
                jfoundUpd["_index"] = p0;
                jfoundUpd["_type"] = p1;
                jfoundUpd["_id"] = p2;

                ScenarioContext.Current["updated"] = jfoundUpd;
            }
        }

        [Then]
        public void Then_this_item_should_be_found_with_new_version()
        {
            Assert.IsNotNull(ScenarioContext.Current["found"]);
            Assert.IsNotNull(ScenarioContext.Current["updated"]);
            JObject jfound = JObject.Parse("" + ScenarioContext.Current["found"]);
            JObject jupdated = JObject.Parse("" + ScenarioContext.Current["updated"]);
            string versionFound = jfound["_version"].ToString();
            string versionUpdated = jupdated["_version"].ToString();
            Assert.AreNotEqual(versionFound, versionUpdated);
            Console.WriteLine($"versionUpdated: {versionUpdated} differs from versionFound: {versionFound}");
        }
    }
}
