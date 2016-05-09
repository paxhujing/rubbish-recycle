using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Config
{
    public class SmsConfigSection : ConfigurationSection
    {
        #region Properties

        [ConfigurationProperty("serverUrl", IsRequired = true)]
        public String ServerUrl
        {
            get { return (String)base["serverUrl"]; }
            set { base["serverUrl"] = value; }
        }

        [ConfigurationProperty("appKey", IsRequired = true)]
        public String AppKey
        {
            get { return (String)base["appKey"]; }
            set { base["appKey"] = value; }
        }

        [ConfigurationProperty("appSecretKey", IsRequired = true)]
        public String AppSecretKey
        {
            get { return (String)base["appSecretKey"]; }
            set { base["appSecretKey"] = value; }
        }

        [ConfigurationProperty("appName", IsRequired = true)]
        public String AppName
        {
            get { return (String)base["appName"]; }
            set { base["appName"] = value; }
        }

        [ConfigurationProperty("format", IsRequired = false, DefaultValue = "json")]
        public String Format
        {
            get { return (String)base["format"]; }
            set { base["format"] = value; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public VerifyCodeElementCollection Codes
        {
            get { return (VerifyCodeElementCollection)base[""]; }
        }

        #endregion
    }
}
