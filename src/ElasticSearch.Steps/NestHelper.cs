using Nest;
using System;
using System.Linq;

namespace ElasticTest.Steps
{
    public class NestHelper
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

        private ElasticClient _elasticClient;
        public ElasticClient Client
        {
            get
            {
                if (_elasticClient == null)
                {
                    var node = new Uri(ElasticUri);
                    var settings = new ConnectionSettings(node);
                    _elasticClient = new ElasticClient(settings);
                }
                return _elasticClient;
            }
            set { _elasticClient = value; }
        }

        public static void DebugICatResponse(ICatResponse<CatHealthRecord> z, string info = null)
        {
            if (!string.IsNullOrEmpty(info)) Console.WriteLine(info);
            Console.WriteLine("ICatResponse " +
                $"\n\t ApiCall.Success =    {z.ApiCall.Success}" +
                $"\n\t ApiCall.Uri =        {z.ApiCall.Uri}" +
                $"\n\t ApiCall.HttpMethod = {z.ApiCall.HttpMethod}" +
                $"\n\t CallDetails =        {z.CallDetails}");

            DebugBytes($"\n\t ApiCall.RequestBodyInBytes=", z.ApiCall.RequestBodyInBytes);
            DebugBytes($"\n\t ApiCall.ResponseBodyInBytes=", z.ApiCall.ResponseBodyInBytes);

            if (z.ApiCall.AuditTrail != null && z.ApiCall.AuditTrail.Count > 0)
            {
                int n = 0;
                foreach (var r in z.ApiCall.AuditTrail)
                    Console.WriteLine($"\n\t ApiCall.AuditTrail {n++}:" +
                       $"\n\t\t Started - Ended: {r.Started} - {r.Ended}" +
                       $"\n\t\t Event:   {r.Event}" +
                       $"\n\t\t Exception:   {r.Exception}" +
                       r.Node == null ? "" : ($"\n\t\t Node Name: {r.Node.Name} id: {r.Node.Id}") +
                       $"\n\t\t Path:  {r.Path}"
                       );
            }

            if (z.Records != null && z.Records.Count() > 0)
            {
                int n = 0;
                foreach (var r in z.Records)
                    Console.WriteLine($"\n\t Record {n++}:" +
                        $"\n\t\t Cluster:    {r.Cluster}" +
                        $"\n\t\t Epoch:      {r.Epoch}" +
                        $"\n\t\t NodeData:   {r.NodeData}" +
                        $"\n\t\t NodeTotal:  {r.NodeTotal}" +
                        $"\n\t\t Primary:    {r.Primary}" +
                        $"\n\t\t Relocating: {r.Relocating}" +
                        $"\n\t\t Shards:     {r.Shards}" +
                        $"\n\t\t Status:     {r.Status}" +
                        $"\n\t\t Timestamp:  {r.Timestamp}" +
                        $"\n\t\t Unassigned: {r.Unassigned}"
                        );
            }
        }

        public static void DebugBytes(string sz, byte[] oz)
        {
            Action<string, byte[]> bytesToStr = (s, o) =>
            {
                string oo = (o == null) ? "null" : System.Text.Encoding.Default.GetString(o);
                Console.Write(s + oo);
            };
            bytesToStr(sz, oz);
        }

        public static void DebugIndexResponse(IIndexResponse z, string info = null)
        {

            if (!string.IsNullOrEmpty(info)) Console.WriteLine(info);

            Console.WriteLine("ICatResponse " +
                $"\n\t ApiCall.Success =    {z.ApiCall.Success}" +
                $"\n\t ApiCall.Uri =        {z.ApiCall.Uri}" +
                $"\n\t ApiCall.HttpMethod = {z.ApiCall.HttpMethod}" +
                $"\n\t Version = {z.Version}; Created = {z.Created}" +
                $"\n\t Index/Type/Id =      {z.Index}/{z.Type}/{z.Id}"
                );

            DebugBytes($"\n\t ApiCall.RequestBodyInBytes=", z.ApiCall.RequestBodyInBytes);
            DebugBytes($"\n\t ApiCall.ResponseBodyInBytes=", z.ApiCall.ResponseBodyInBytes);

            DebugBytes($"\n\t CallDetails.ResponseBodyInBytes=", z.CallDetails.ResponseBodyInBytes);
            DebugBytes($"\n\t CallDetails.ResponseBodyInBytes=", z.CallDetails.ResponseBodyInBytes);

            if (z.ApiCall.AuditTrail != null && z.ApiCall.AuditTrail.Count > 0)
            {
                int n = 0;
                foreach (var r in z.ApiCall.AuditTrail)
                    Console.WriteLine($"\n\t ApiCall.AuditTrail {n++}:" +
                       $"\n\t\t Started - Ended: {r.Started} - {r.Ended}" +
                       $"\n\t\t Event:   {r.Event}" +
                       $"\n\t\t Exception:   {r.Exception}" +
                       r.Node == null ? "" : ($"\n\t\t Node Name: {r.Node.Name} id: {r.Node.Id}") +
                       $"\n\t\t Path:  {r.Path}"
                       );
            }
        }

        public static void DebugGetResponse<T>(IGetResponse<T> z, string info = null) where T : class
        {
            if (!string.IsNullOrEmpty(info)) Console.WriteLine(info);

            Console.WriteLine("ICatResponse " +
                $"\n\t ApiCall.Success =    {z.ApiCall.Success}" +
                $"\n\t ApiCall.Uri =        {z.ApiCall.Uri}" +
                $"\n\t ApiCall.HttpMethod = {z.ApiCall.HttpMethod}" +
                $"\n\t Version = {z.Version};" +
                $"\n\t Index/Type/Id =      {z.Index}/{z.Type}/{z.Id}"
                );

            DebugBytes($"\n\t ApiCall.RequestBodyInBytes=", z.ApiCall.RequestBodyInBytes);
            DebugBytes($"\n\t ApiCall.ResponseBodyInBytes=", z.ApiCall.ResponseBodyInBytes);

            DebugBytes($"\n\t CallDetails.ResponseBodyInBytes=", z.CallDetails.ResponseBodyInBytes);
            DebugBytes($"\n\t CallDetails.ResponseBodyInBytes=", z.CallDetails.ResponseBodyInBytes);

            if (z.ApiCall.AuditTrail != null && z.ApiCall.AuditTrail.Count > 0)
            {
                int n = 0;
                foreach (var r in z.ApiCall.AuditTrail)
                    Console.WriteLine($"\n\t ApiCall.AuditTrail {n++}:" +
                       $"\n\t\t Started - Ended: {r.Started} - {r.Ended}" +
                       $"\n\t\t Event:   {r.Event}" +
                       $"\n\t\t Exception:   {r.Exception}" +
                       r.Node == null ? "" : ($"\n\t\t Node Name: {r.Node.Name} id: {r.Node.Id}") +
                       $"\n\t\t Path:  {r.Path}"
                       );
            }
        }

        public static void DebugDeleteResponse(IDeleteResponse z, string info = null) 
        {
            if (!string.IsNullOrEmpty(info)) Console.WriteLine(info);

            Console.WriteLine("ICatResponse " +
                $"\n\t ApiCall.Success =    {z.ApiCall.Success}" +
                $"\n\t ApiCall.Uri =        {z.ApiCall.Uri}" +
                $"\n\t ApiCall.HttpMethod = {z.ApiCall.HttpMethod}" +
                $"\n\t Version = {z.Version};" +
                $"\n\t Index/Type/Id =      {z.Index}/{z.Type}/{z.Id}"
                );

            DebugBytes($"\n\t ApiCall.RequestBodyInBytes=", z.ApiCall.RequestBodyInBytes);
            DebugBytes($"\n\t ApiCall.ResponseBodyInBytes=", z.ApiCall.ResponseBodyInBytes);

            DebugBytes($"\n\t CallDetails.ResponseBodyInBytes=", z.CallDetails.ResponseBodyInBytes);
            DebugBytes($"\n\t CallDetails.ResponseBodyInBytes=", z.CallDetails.ResponseBodyInBytes);

            if (z.ApiCall.AuditTrail != null && z.ApiCall.AuditTrail.Count > 0)
            {
                int n = 0;
                foreach (var r in z.ApiCall.AuditTrail)
                    Console.WriteLine($"\n\t ApiCall.AuditTrail {n++}:" +
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
