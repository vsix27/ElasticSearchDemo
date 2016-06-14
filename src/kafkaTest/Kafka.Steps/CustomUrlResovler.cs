using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kafka.Steps
{
    /// <summary> class to prevent XML External Entity Reference ('XXE') attack </summary>
    public class CustomUrlResovler : XmlUrlResolver
    {     
        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            if (baseUri == null && System.IO.File.Exists(relativeUri))
                baseUri = new Uri(relativeUri);

            Uri uri = new Uri(baseUri, relativeUri);
            if (IsUnsafeHost(uri.Host))
                return null;

            return base.ResolveUri(baseUri, relativeUri);
        }

        /// <summary> </summary>
        /// <param name="host">empty string means local computer</param>
        /// <returns></returns>
        private bool IsUnsafeHost(string host)
        {
            return false;
        }

        /// <summary>
        /// if document has <!DOCTYPE myxml SYSTEM "some.dtd">, then this GetEntity will be called with absoluteUri = some.dtd
        /// </summary>
        /// <param name="absoluteUri"></param>
        /// <param name="role"></param>
        /// <param name="ofObjectToReturn"></param>
        /// <returns></returns>
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            // reserved: exclude external entity [absoluteUri] which starts from: http://, file://, jdbc:, or ftp://
            if (absoluteUri != null && absoluteUri.AbsoluteUri.ToLower().StartsWith("httpZ://"))
                throw new Exception("external entity not allowded " + absoluteUri.AbsoluteUri);
            return base.GetEntity(absoluteUri, role, ofObjectToReturn);
        }
    }
}
