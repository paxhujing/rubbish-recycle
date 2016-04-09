using RubbishRecycle.Toolkit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Controllers.Assets
{
    internal class AccountToken
    {
        #region Constructors

        /// <summary>
        /// 初始化类型 RubbishRecycle.Controllers.Assets.AccountToken 实例。
        /// </summary>
        /// <param name="token">令牌。</param>
        /// <param name="accountId">账号Id。</param>
        /// <param name="cryptor">AES 加密/解密器。</param>
        public AccountToken(String token,Int32 accountId, AESCryptor cryptor)
        {
            this._token = token;
            this._accountId = accountId;
            this._cryptor = cryptor;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 客户端最近一次请求日期时间。
        /// 每次检查周期移除时间间隔超过30秒的客户端。
        /// </summary>
        public DateTime LastRequest
        {
            get;
            set;
        }

        #region Token

        private readonly String _token;
        /// <summary>
        /// 客户端令牌。
        /// </summary>
        public String Token
        {
            get { return this._token; }
        }

        #endregion

        #region AccountId

        private readonly Int32 _accountId;
        /// <summary>
        /// 账号Id。
        /// </summary>
        public Int32 AccountId
        {
            get { return this._accountId; }
        }

        #endregion

        #region Context

        private readonly AESCryptor _cryptor;
        /// <summary>
        /// AES 加密/解密器。
        /// </summary>
        public AESCryptor Cryptor
        {
            get { return this._cryptor; }
        }

        #endregion

        #region Roles
        /// <summary>
        /// 角色列表。
        /// </summary>
        public String Role
        {
            get;
            set;
        }

        #endregion

        #endregion
    }
}
