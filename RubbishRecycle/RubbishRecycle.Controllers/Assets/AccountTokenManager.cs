using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;

namespace RubbishRecycle.Controllers.Assets
{
    internal class AccountTokenManager : Collection<AccountToken>
    {
        #region Fields

        private static readonly Int32 LifeTime = 3;

        public static readonly AccountTokenManager Manager = new AccountTokenManager();

        private Dictionary<String, AccountToken> _mapAccountToken;

        private Dictionary<Int32, String> _mapToken;

        private readonly Timer _timer;

        #endregion

        #region Constructors

        private AccountTokenManager()
        {
            this._mapAccountToken = new Dictionary<String, AccountToken>();
            this._mapToken = new Dictionary<Int32, String>();
            this._timer = new Timer(RebuildAccountTokens, null, TimeSpan.FromMinutes(4), TimeSpan.FromMinutes(4));
        }

        #endregion

        #region Properties

        public AccountToken this[String token]
        {
            get
            {
                if (this._mapAccountToken.ContainsKey(token))
                {
                    return GetAndUpdateAccountTokenLife(token);
                }
                return null;
            }
        }

        #endregion

        #region Methods

        public AccountToken GetTokenByAccountId(Int32 accountId)
        {
            if (this._mapToken.ContainsKey(accountId))
            {
                String token = this._mapToken[accountId];
                return GetAndUpdateAccountTokenLife(token);
            }
            return null;
        }

        protected override void InsertItem(Int32 index, AccountToken item)
        {
            base.InsertItem(index, item);
            item.life = AccountTokenManager.LifeTime;
            this._mapAccountToken.Add(item.Token, item);
            this._mapToken.Add(item.AccountId, item.Token);
        }

        protected override void RemoveItem(Int32 index)
        {
            AccountToken accountToken = base.Items[index];
            this._mapAccountToken.Remove(accountToken.Token);
            this._mapToken.Remove(accountToken.AccountId);
            //如果accountToken本就是最后一项，就直接移除
            if (base.Count == (index + 1))
            {
                base.RemoveItem(index);
            }//否则用最后一项替换删除项
            else
            {
                Int32 lastIndex = base.Count - 1;
                AccountToken last = base.Items[lastIndex];
                base.SetItem(index, last);
                base.RemoveItem(lastIndex);
            }
        }

        protected override void ClearItems()
        {
            this._mapAccountToken.Clear();
            this._mapToken.Clear();
            base.ClearItems();
        }

        #region Misc

        private AccountToken GetAndUpdateAccountTokenLife(String token)
        {
            AccountToken accountToken = this._mapAccountToken[token];
            lock(accountToken.SyncRoot)
            {
                if (accountToken.IsInvalide) return null;
                accountToken.life = AccountTokenManager.LifeTime;
                return accountToken;
            }
        }

        private void RebuildAccountTokens(Object state)
        {
            AccountToken[] accountTokens = this.ToArray();
            IList<AccountToken> lifeAccountTokens = new List<AccountToken>(accountTokens.Length);
            Parallel.ForEach(accountTokens, (accountToken) =>
             {
                 lock(accountToken.SyncRoot)
                 {
                     accountToken.life--;
                     if (!accountToken.IsInvalide)
                     {
                         lifeAccountTokens.Add(accountToken);
                     }
                 }
             });
            Dictionary<String, AccountToken> newMapAccountToken = new Dictionary<String, AccountToken>();
            Dictionary<Int32, String> newMapToken = new Dictionary<Int32, String>();
            foreach (AccountToken accountToken in lifeAccountTokens)
            {
                newMapAccountToken.Add(accountToken.Token, accountToken);
                newMapToken.Add(accountToken.AccountId, accountToken.Token);
            }
            lock(this)
            {
                this._mapAccountToken = newMapAccountToken;
                this._mapToken = newMapToken;
            }
        }

        #endregion

        #endregion
    }
}
