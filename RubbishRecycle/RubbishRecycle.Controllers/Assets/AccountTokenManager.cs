using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        private readonly Dictionary<String, AccountToken> _tokenMapAccountToken;

        private readonly Dictionary<String, String> _phoneMapToken;

        private readonly Timer _timer;

        private readonly Object _syncObj = new Object();

        #endregion

        #region Constructors

        private AccountTokenManager()
        {
            this._tokenMapAccountToken = new Dictionary<String, AccountToken>();
            this._phoneMapToken = new Dictionary<String, String>();

            this._timer = new Timer(TimeSpan.FromMinutes(3).TotalMilliseconds);
            this._timer.Elapsed += _timer_Elapsed;
        }

        #endregion

        #region Methods

        private static String GenerateToken()
        {
            return Guid.NewGuid().ToString().Replace("-", String.Empty);
        }

        public Boolean TryGetTokenByPhone(String phone,out String token)
        {
            if(this._phoneMapToken.ContainsKey(phone))
            {
                token = this._phoneMapToken[phone];
                this._tokenMapAccountToken[token].Timestamp = DateTime.Now;
                return true;
            }
            token = null;
            return false;
        }

        public AccountToken GetAccountTokenByToken(String token)
        {
            lock (this._syncObj)
            {
                if (this._tokenMapAccountToken.ContainsKey(token))
                {
                    AccountToken accountViewer = this._tokenMapAccountToken[token];
                    AppGlobal.Log.DebugFormat("Get account token by: {0}; interval: {1}", token, (DateTime.Now - accountViewer.Timestamp).Seconds);
                    if (IsValid(accountViewer))
                    {
                        accountViewer.Timestamp = DateTime.Now;
                        return accountViewer;
                    }
                }
                return null;
            }
        }

        public String Add(AccountToken accountToken)
        {
            lock(this._syncObj)
            {
                String token = AccountTokenManager.GenerateToken();
                accountToken.Timestamp = DateTime.Now;
                accountToken.Token = token;
                this._tokenMapAccountToken.Add(token, accountToken);
                this._phoneMapToken.Add(accountToken.Phone, token);
                if (!this._timer.Enabled)
                {
                    this._timer.Start();
                }
                AppGlobal.Log.InfoFormat("Account token count: {0}", this._tokenMapAccountToken.Count);
                return token;
            }
        }

        public void ChangeToken(String oldToken)
        {
            lock(this._syncObj)
            {
                if(this._tokenMapAccountToken.ContainsKey(oldToken))
                {
                    AccountToken accounToken = this._tokenMapAccountToken[oldToken];
                    accounToken.Timestamp = DateTime.Now;
                    String token = AccountTokenManager.GenerateToken();
                    this._tokenMapAccountToken.Remove(oldToken);
                    accounToken.Token = token;
                    this._tokenMapAccountToken.Add(token, accounToken);
                    AppGlobal.Log.InfoFormat("Token changed for '{0}' from '{1}' to '{2}'", accounToken.Phone, oldToken, token);
                }
            }
        }

        public void Remove(String token)
        {
            lock(this._syncObj)
            {
                if(this._tokenMapAccountToken.ContainsKey(token))
                {
                    AccountToken at = this._tokenMapAccountToken[token];
                    this._tokenMapAccountToken.Remove(token);
                    this._phoneMapToken.Remove(at.Phone);
                    if (this._tokenMapAccountToken.Count == 0)
                    {
                        this._timer.Stop();
                    }
                    AppGlobal.Log.InfoFormat("Account token count: {0}", this._tokenMapAccountToken.Count);
                }
            }
        }

        public void Clear()
        {
            lock(this._syncObj)
            {
                this._tokenMapAccountToken.Clear();
                this._phoneMapToken.Clear();
                this._timer.Stop();
            }
        }

        #region TC

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this._timer.Stop();
            lock (this._syncObj)
            {
                Rebuild();
                if (this._tokenMapAccountToken.Count != 0)
                {
                    this._timer.Start();
                }
            }
        }

        private void Rebuild()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Int32 count = this._tokenMapAccountToken.Count;
            AppGlobal.Log.InfoFormat("Token collection launch...total count: {0}", count);
            if (count == 0)
            {
                sw.Stop();
                AppGlobal.Log.InfoFormat("Token collection finish: {0}", sw.ElapsedMilliseconds);
                return;
            }
            AccountToken[] ats = this._tokenMapAccountToken.Values.ToArray();
            this._tokenMapAccountToken.Clear();
            this._phoneMapToken.Clear();
            Parallel.ForEach(ats, (at) =>
            {
                if (IsValid(at))
                {
                    this._tokenMapAccountToken.Add(at.Token, at);
                    this._phoneMapToken.Add(at.Phone, at.Token);
                }
            });
            AppGlobal.Log.InfoFormat("Token collection finish: {0}...New account token count: {1}", sw.ElapsedMilliseconds, this._tokenMapAccountToken.Count);
            sw.Stop();
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
