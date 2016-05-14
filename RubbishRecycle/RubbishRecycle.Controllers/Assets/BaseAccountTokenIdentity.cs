using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Controllers.Assets
{
    internal class BaseAccountTokenIdentity : IIdentity
    {
        #region Constructors

        public BaseAccountTokenIdentity(AccountToken at)
        {
            this._accountToken = at;
        }

        #endregion

        #region Properties

        #region AccountToken

        private readonly AccountToken _accountToken;
        public AccountToken AccountToken
        {
            get { return this._accountToken; }
        }

        #endregion

        public String AuthenticationType
        {
            get { return this._accountToken.Role; }
        }

        public Boolean IsAuthenticated
        {
            get;
            set;
        }

        public String Name
        {
            get { return this._accountToken.Phone; }
        }

        #endregion
    }
}
