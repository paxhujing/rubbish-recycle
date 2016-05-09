using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Config
{
    public class CodeElement : ConfigurationElement
    {
        [ConfigurationProperty("smsType", IsRequired = false, DefaultValue = "normal")]
        public String SmsType
        {
            get { return (String)base["smsType"]; }
            set { base["smsType"] = value; }
        }

        [ConfigurationProperty("signName", IsRequired = true, IsKey = true)]
        public String SignName
        {
            get { return (String)base["signName"]; }
            set { base["signName"] = value; }
        }

        [ConfigurationProperty("templateCode", IsRequired = true)]
        public String TemplateCode
        {
            get { return (String)base["templateCode"]; }
            set { base["templateCode"] = value; }
        }

        [ConfigurationProperty("length", IsRequired = false, DefaultValue = "6")]
        public Int32 Length
        {
            get { return (Int32)base["length"]; }
            set { base["length"] = value; }
        }
    }
}
