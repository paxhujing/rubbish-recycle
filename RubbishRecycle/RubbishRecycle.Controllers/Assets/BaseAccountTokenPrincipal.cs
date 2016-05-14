using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Controllers.Assets
{
    internal class BaseAccountTokenPrincipal : IPrincipal
    {
        #region Constructors

        public BaseAccountTokenPrincipal(AccountToken at)
        {
            BaseAccountTokenIdentity identity = new BaseAccountTokenIdentity(at);
            this.Identity = identity;
        }

        #endregion

        public IIdentity Identity
        {
            get;
            private set;
        }

        public Boolean IsInRole(String role)
        {
            if (String.IsNullOrEmpty(role)) return true;
            return role.Split(';').Contains(this.Identity.AuthenticationType);
        }
    }
}
