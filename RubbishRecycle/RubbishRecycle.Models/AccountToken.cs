using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    public class AccountToken
    {
        public String Token
        {
            get;
            set;
        }

        public String PrivateKey
        {
            get;
            set;
        }

        public Int32 Expire
        {
            get;
            set;
        }
    }
}
