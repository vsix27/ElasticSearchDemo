using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafka.Steps
{
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Base
    {

        private string[] personField;

        private string timeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Person")]
        public string[] Person
        {
            get
            {
                return this.personField;
            }
            set
            {
                this.personField = value;
            }
        }

        /// <remarks/>
        public string Time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }
    }


}
