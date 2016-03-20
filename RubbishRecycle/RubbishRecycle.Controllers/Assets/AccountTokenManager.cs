using RubbishRecycle.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Controllers.Assets
{
    internal class AccountTokenManager : Collection<AccountToken>
    {
        #region Fields

        public static readonly AccountTokenManager Manager = new AccountTokenManager();

        private readonly Dictionary<String, AccountToken> _mapTokens;

        #endregion

        #region Constructors

        private AccountTokenManager()
        {
            this._mapTokens = new Dictionary<String, AccountToken>();
        }

        #endregion

        #region Properties

        public AccountToken this[String token]
        {
            get
            {
                if (this._mapTokens.ContainsKey(token))
                {
                    return this._mapTokens[token];
                }
                return null;
            }
        }

        #endregion

        #region Methods

        protected override void InsertItem(Int32 index, AccountToken item)
        {
            base.InsertItem(index, item);
            this._mapTokens.Add(item.Token, item);
        }

        protected override void RemoveItem(Int32 index)
        {
            AccountToken accountToken = base.Items[index];
            this._mapTokens.Remove(accountToken.Token);
            base.RemoveItem(index);
        }

        protected override void SetItem(Int32 index, AccountToken item)
        {
            throw new NotSupportedException("Not support change existed token.");
        }

        protected override void ClearItems()
        {
            this._mapTokens.Clear();
            base.ClearItems();
        }

        #endregion
    }
}
