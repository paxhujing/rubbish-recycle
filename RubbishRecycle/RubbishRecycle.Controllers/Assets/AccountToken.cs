using RubbishRecycle.Models;
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
        #region Fields

        /// <summary>
        /// 生命值。当生命值小于或等于0时，Token失效。
        /// </summary>
        internal Int32 life;

        #endregion

        #region Constructors

        /// <summary>
        /// 初始化类型 RubbishRecycle.Controllers.Assets.AccountToken 实例。
        /// </summary>
        /// <param name="accountId">账号Id。</param>
        /// <param name="cryptor">AES 加密/解密器。</param>
        public AccountToken(Int32 accountId, AESCryptor cryptor)
        {
            this._token = Guid.NewGuid().ToString().Replace("-", String.Empty);
            this._accountId = accountId;
            this._cryptor = cryptor;
            this._syncRoot = new Object();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Token是否失效。
        /// </summary>
        public Boolean IsInvalide
        {
            get { return this.life <= 0; }
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

        #region IsFreeze

        /// <summary>
        /// 账户是否被冻结。
        /// </summary>
        public Boolean IsFreeze
        {
            get;
            set;
        }

        #endregion

        #region SyncRoot

        private readonly Object _syncRoot;
        /// <summary>
        /// 用于线程同步。
        /// </summary>
        public Object SyncRoot
        {
            get{ return this._syncRoot; }
        }

        #endregion

        #endregion
    }
}
