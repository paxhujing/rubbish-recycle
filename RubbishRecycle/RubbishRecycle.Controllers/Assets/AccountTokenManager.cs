using RubbishRecycle.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Controllers.Assets
{
    public class AccountTokenManager : Collection<AccountToken>
    {
        #region Fields

        public static readonly AccountTokenManager Manager = new AccountTokenManager();

        #endregion

        #region Constructors

        private AccountTokenManager()
        {

        }

        #endregion

        #region Methods
        

        #endregion
    }
}
