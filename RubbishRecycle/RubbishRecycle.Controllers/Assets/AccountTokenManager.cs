using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace RubbishRecycle.Controllers.Assets
{
    internal class AccountTokenManager
    {
        #region Fields

        private static readonly TimeSpan LifeTime = TimeSpan.FromMinutes(10);

        public static readonly AccountTokenManager Manager = new AccountTokenManager();

        private readonly Dictionary<String, AccountToken> _idMapAccountToken;

        private readonly Dictionary<String, AccountToken> _tokenMapAccountToken;

        private readonly Timer _timer;

        private readonly Object _syncObj = new Object();

        #endregion

        #region Constructors

        private AccountTokenManager()
        {
            this._idMapAccountToken = new Dictionary<String, AccountToken>();
            this._tokenMapAccountToken = new Dictionary<String, AccountToken>();

            this._timer = new Timer(TimeSpan.FromMinutes(3).TotalMilliseconds);
            this._timer.Elapsed += _timer_Elapsed;
        }

        #endregion

        #region Methods

        public AccountToken GetAccountTokenById(String accountId)
        {
            lock(this._syncObj)
            {
                if (this._idMapAccountToken.ContainsKey(accountId))
                {
                    AccountToken accountToken = this._idMapAccountToken[accountId];
                    AppGlobal.Log.DebugFormat("Get account token by id: {0}; interval: {1}", accountId, DateTime.Now - accountToken.Timestamp);
                    if (IsValid(accountToken))
                    {
                        accountToken.Timestamp = DateTime.Now;
                        return accountToken;
                    }
                    Remove(accountToken);
                }
                return null;
            }
        }

        public AccountToken GetAccountTokenByToken(String token)
        {
            lock (this._syncObj)
            {
                if (this._tokenMapAccountToken.ContainsKey(token))
                {
                    AccountToken accountToken = this._tokenMapAccountToken[token];
                    AppGlobal.Log.DebugFormat("Get account token by token: {0}; interval: {1}", token, DateTime.Now - accountToken.Timestamp);
                    if (IsValid(accountToken))
                    {
                        accountToken.Timestamp = DateTime.Now;
                        return accountToken;
                    }
                }
                return null;
            }
        }

        public void Add(AccountToken accountToken)
        {
            lock(this._syncObj)
            {
                accountToken.Timestamp = DateTime.Now;
                this._idMapAccountToken.Add(accountToken.AccountId, accountToken);
                this._tokenMapAccountToken.Add(accountToken.Token, accountToken);
                if (!this._timer.Enabled)
                {
                    this._timer.Start();
                }
                AppGlobal.Log.InfoFormat("Account token count: {0}", this._tokenMapAccountToken.Count);
            }
        }

        public void Remove(AccountToken accountToken)
        {
            lock(this._syncObj)
            {
                this._idMapAccountToken.Remove(accountToken.AccountId);
                this._tokenMapAccountToken.Remove(accountToken.Token);
                if (this._idMapAccountToken.Count == 0)
                {
                    this._timer.Stop();
                }
                AppGlobal.Log.InfoFormat("Account token count: {0}", this._tokenMapAccountToken.Count);
            }
        }

        public void Remove(String token)
        {
            lock(this._syncObj)
            {
                AccountToken at = GetAccountTokenByToken(token);
                if (at == null) return;
                Remove(at);
            }
        }

        public void Clear()
        {
            lock(this._syncObj)
            {
                this._idMapAccountToken.Clear();
                this._tokenMapAccountToken.Clear();
                this._timer.Stop();
            }
        }

        #region TC

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (this._syncObj)
            {
                this._timer.Stop();
                Rebuild();
                if (this._idMapAccountToken.Count != 0)
                {
                    this._timer.Start();
                }
            }
        }

        private void Rebuild()
        {
            AccountToken[] accountTokens = this._idMapAccountToken.Values.ToArray();
            AppGlobal.Log.InfoFormat("Token collection launch...total count: {0}", accountTokens.Length);
            if (accountTokens.Length == 0) return;

            this._idMapAccountToken.Clear();
            this._tokenMapAccountToken.Clear();
            Parallel.ForEach(accountTokens, (accountToken) =>
            {
                if (IsValid(accountToken))
                {
                    this._idMapAccountToken.Add(accountToken.AccountId, accountToken);
                    this._tokenMapAccountToken.Add(accountToken.Token, accountToken);
                }
            });
            AppGlobal.Log.InfoFormat("New account token count: {0}", this._idMapAccountToken.Count);
        }

        private Boolean IsValid(AccountToken token)
        {
            TimeSpan interval = DateTime.Now - token.Timestamp;
            return interval < AccountTokenManager.LifeTime;
        }

        #endregion


        #endregion
    }
}
