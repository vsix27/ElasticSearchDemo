using System;
using System.Collections.Generic;
using System.IO;
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

        internal static void WriteOntologiesToJson(string testFile, List<Tuple<ResourceType, IMessageLite>> generatedOntologyObjects)
        {
            string tmp = Path.GetTempPath();
            File.Delete(tmp);

            string outputFilePath = tmp + "_allOntologies.json";
               
            //serialize and write
            string text = string.Empty;
            foreach (Tuple<ResourceType, IMessageLite> generatedOntologyObject in generatedOntologyObjects)
            {
                text += String.Format("{0}: {1}{2}{1}",
                    generatedOntologyObject.Item1, Environment.NewLine, generatedOntologyObject.Item2.ToJson());
            }

            File.WriteAllText(outputFilePath, text);
        }
    }
}