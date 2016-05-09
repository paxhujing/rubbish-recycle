using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Config
{
    [ConfigurationCollection(typeof(CodeElement))]
    public class VerifyCodeElementCollection : ConfigurationElementCollection
    {
        #region Constructors

        public VerifyCodeElementCollection()
            :base(StringComparer.OrdinalIgnoreCase)
        {

        }

        #endregion

        #region Methods

        protected override ConfigurationElement CreateNewElement()
        {
            return new CodeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((CodeElement)element).SignName;
        }

        public CodeElement this[Int32 index]
        {
            get { return BaseGet(index) as CodeElement; }
        }

        public new CodeElement this[String signName]
        {
            get { return BaseGet(signName) as CodeElement; }
        }

        #endregion
    }
}
