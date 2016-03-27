using Elasticsearch.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticTest.Steps
{
    public class ElasticHelper
    {
        private string _elasticUri;

        /// <summary> "http://localhost:9200" </summary>
        public string ElasticUri
        {
            get
            {
                if (string.IsNullOrEmpty(_elasticUri))
                    _elasticUri = "http://localhost:9200";
                return _elasticUri;
            }
            set { _elasticUri = value; }
        }

        private ElasticLowLevelClient _elasticClient;
        public ElasticLowLevelClient Client
        {
            get
            {
                if (_elasticClient == null)
                {
                    var node = new Uri(ElasticUri);
                    var config = new ConnectionConfiguration(node);
                    _elasticClient = new ElasticLowLevelClient(config);
                }
                return _elasticClient;
            }
            set { _elasticClient = value; }
        }

        public static void DebugElasticsearchResponse<T>(ElasticsearchResponse<T> z, string info = null)
        {
            if (!string.IsNullOrEmpty(info)) Console.WriteLine(info);
            Console.WriteLine("ElasticsearchResponse " +
                $"\n\t Success = {z.Success}" +
                $"\n\t HttpMethod = {z.HttpMethod}" +
                $"\n\t HttpStatusCode = {z.HttpStatusCode}" +
                $"\n\t Uri = {z.Uri}" +
                $"\n\t Body = {z.Body}"
                );
            if (z.AuditTrail != null && z.AuditTrail.Count > 0)
            {
                int n = 0;
                foreach (var r in z.AuditTrail)
                    Console.WriteLine($"\n\t AuditTrail {n++}:" +
                       $"\n\t\t Started - Ended: {r.Started} - {r.Ended}" +
                       $"\n\t\t Event:   {r.Event}" +
                       $"\n\t\t Exception:   {r.Exception}" +
                       r.Node == null ? "" : ($"\n\t\t Node Name: {r.Node.Name} id: {r.Node.Id}") +
                       $"\n\t\t Path:  {r.Path}"
                       );
            }
        }
    }
}
