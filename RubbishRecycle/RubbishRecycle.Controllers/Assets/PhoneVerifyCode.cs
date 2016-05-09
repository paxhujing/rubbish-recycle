using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Controllers.Assets
{
    internal struct PhoneVerifyCode
    {
        public PhoneVerifyCode(String phone, String code, VerifyCodeType type)
        {
            this._timestamp = DateTime.Now;
            this._phone = phone;
            this._code = code;
            this._type = type;
        }
        #region Properties

        #region Timestamp

        private readonly DateTime _timestamp;
        public DateTime Timestamp
        {
            get { return this._timestamp; }
        }

        #endregion

        #region Code

        private readonly String _code;

        public String Code
        {
            get
            { return this._code; }
        }

        #endregion

        #region Phone

        private readonly String _phone;

        public String Phone
        {
            get
            { return this._phone; }
        }

        #endregion

        #region Type

        private readonly VerifyCodeType _type;
        public VerifyCodeType Type
        {
            get { return this._type; }
        } 

        #endregion

        #endregion
    }
}
