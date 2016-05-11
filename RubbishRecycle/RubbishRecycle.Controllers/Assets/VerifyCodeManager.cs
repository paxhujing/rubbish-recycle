using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RubbishRecycle.Controllers.Assets
{
    internal class VerifyCodeManager
    {
        #region Fields

        public static readonly TimeSpan LifeTime = TimeSpan.FromMinutes(1);

        public static readonly VerifyCodeManager Manager = new VerifyCodeManager();

        private readonly Dictionary<String, PhoneVerifyCode> _mapVerifyCode;

        private readonly Timer _timer;

        private readonly Object _syncObj = new Object();

        #endregion

        #region Constructors

        private VerifyCodeManager()
        {
            this._mapVerifyCode = new Dictionary<String, PhoneVerifyCode>();

            this._timer = new Timer(TimeSpan.FromMinutes(1).TotalMilliseconds);
            this._timer.Elapsed += _timer_Elapsed;
        }

        #endregion

        #region Methods

        public String GetCodeByPhone(String phone)
        {
            if (String.IsNullOrEmpty(phone)) return null;
            lock (this._syncObj)
            {
                if (this._mapVerifyCode.ContainsKey(phone))
                {
                    PhoneVerifyCode pv = this._mapVerifyCode[phone];
                    AppGlobal.Log.DebugFormat("Get verify code: {0}; Timestamp: {1}", pv.Phone, pv.Timestamp);
                    this._mapVerifyCode.Remove(phone);
                    if (IsValid(pv))
                    {
                        return pv.Code;
                    }
                }
                return null;
            }
        }

        public void Add(String phone, String code, VerifyCodeType type)
        {
            if (String.IsNullOrEmpty(phone) || String.IsNullOrEmpty(code)) return;
            lock (this._syncObj)
            {
                PhoneVerifyCode pv = new PhoneVerifyCode(phone, code, type);
                if (this._mapVerifyCode.ContainsKey(phone))
                {
                    this._mapVerifyCode[phone] = pv;
                }
                else
                {
                    this._mapVerifyCode.Add(phone, pv);
                    AppGlobal.Log.InfoFormat("Add new verify code: {0};phone: {1};type: {2}", code, phone, type.ToString());
                    if (!this._timer.Enabled)
                    {
                        this._timer.Start();
                    }
                }
                AppGlobal.Log.InfoFormat("Verify code count: {0}", this._mapVerifyCode.Count);
            }
        }

        public void Remove(String phone)
        {
            if (String.IsNullOrEmpty(phone)) return;
            lock (this._syncObj)
            {
                if (this._mapVerifyCode.ContainsKey(phone))
                {
                    PhoneVerifyCode pv = this._mapVerifyCode[phone];
                    this._mapVerifyCode.Remove(phone);
                    if (this._mapVerifyCode.Count == 0)
                    {
                        this._timer.Stop();
                    }
                    AppGlobal.Log.InfoFormat("Verify code count: {0}", this._mapVerifyCode.Count);
                }
            }
        }

        #endregion

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (this._syncObj)
            {
                this._timer.Stop();
                Rebuild();
                if (this._mapVerifyCode.Count != 0)
                {
                    this._timer.Start();
                }
            }
        }

        private void Rebuild()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            PhoneVerifyCode[] pvs = this._mapVerifyCode.Values.ToArray();
            AppGlobal.Log.InfoFormat("Verify code collection launch...total count: {0}", pvs.Length);
            if (pvs.Length == 0)
            {
                sw.Stop();
                AppGlobal.Log.InfoFormat("Verify code collection finish: {0}", sw.ElapsedMilliseconds);
                return;
            }

            this._mapVerifyCode.Clear();
            Parallel.ForEach(pvs, (pv) =>
            {
                if (IsValid(pv))
                {
                    this._mapVerifyCode.Add(pv.Phone, pv);
                }
            });
            sw.Stop();
            AppGlobal.Log.InfoFormat("Verify code collection finish: {0}...New verify code count: {1}", sw.ElapsedMilliseconds, this._mapVerifyCode.Count);
        }

        private Boolean IsValid(PhoneVerifyCode pv)
        {
            TimeSpan interval = DateTime.Now - pv.Timestamp;
            return interval < VerifyCodeManager.LifeTime;
        }

    }
}
