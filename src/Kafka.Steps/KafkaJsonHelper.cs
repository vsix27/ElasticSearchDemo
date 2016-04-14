using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.ProtocolBuffers;
using Hl7.Fhir.ModelFake;

namespace Kafka.Steps
{
    public class KafkaJsonHelper
    {
        public static void WriteOntologyToJson(string testFile, IMessageLite ontologyObject = null, string testComponentName = null)
        {

            if (!String.IsNullOrEmpty(testComponentName))
                testComponentName = "_" + testComponentName;
            else if (ontologyObject != null)
            {
                testComponentName = "_" + ontologyObject.GetType().Name;
            }

            string tmp = Path.GetTempPath();
            File.Delete(tmp);
            string outputFilePath = tmp + ".json";

            //serialize and write
            IMessageLite ontologyObjectAsMessageLite = ontologyObject;
            string text = ontologyObjectAsMessageLite.ToJson();
            File.WriteAllText(outputFilePath, text);
        }

        internal static string WriteOntologiesToJson(string testFile, List<Tuple<ResourceType, IMessageLite>> generatedOntologyObjects)
        {
            string tmp = Path.GetTempPath();
            File.Delete(tmp);

            Func<Tuple<ResourceType, IMessageLite>, string> all = (x) => $"{x.Item1}: \n{x.Item2.ToJson()}\n";
            string text =  generatedOntologyObjects.Select(x => all(x)).Aggregate((x, y) => x + y);           

            string outputFilePath = tmp + "_allOntologies.json";
            File.WriteAllText(outputFilePath, text);
            return outputFilePath;
        }
    }
}