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
        /// <param name="tokenMapKey"></param>
        /// <param name="cryptor">AES 加密/解密器。</param>
        public AccountToken(Int32 accountId,Int32 tokenMapKey, AESCryptor cryptor)
        {
            this._token = Guid.NewGuid().ToString().Replace("-", String.Empty);
            this._accountId = accountId;
            this._cryptor = cryptor;
            this._tokenMapKey = tokenMapKey;
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

        #region TokenMapKey

        private readonly Int32 _tokenMapKey;
        /// <summary>
        /// 通过账户名和密码唯一标识该Token。
        /// </summary>
        public Int32 TokenMapKey
        {
            get { return this._tokenMapKey; }
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

        #region Methods

        /// <summary>
        /// 生成唯一标识Token的Key。
        /// </summary>
        /// <param name="name">账户名。</param>
        /// <param name="password">密码。</param>
        /// <returns>唯一标识Token的Key。</returns>
        public static Int32 GenerateTokenMapKey(String name,String password)
        {
            return name.GetHashCode() ^ password.GetHashCode();
        }

        #endregion
    }
}
