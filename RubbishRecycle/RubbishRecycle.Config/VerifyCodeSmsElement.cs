using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Config
{
    public class VerifyCodeSmsElement: SmsElementBase
    {
        [ConfigurationProperty("length", IsRequired = false, DefaultValue = "6")]
        public Int32 Length
        {
            get { return (Int32)base["length"]; }
            set { base["length"] = value; }
        }
    }
}
