﻿using RubbishRecycle.Models;
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

        #endregion

        #region Constructors

        /// <summary>
        /// 初始化类型 RubbishRecycle.Controllers.Assets.AccountToken 实例。
        /// </summary>
        /// <param name="accountId">账号Id。</param>
        /// <param name="cryptor">AES 加密/解密器。</param>
        public AccountToken(String accountId)
        {
            this._accountId = accountId;
            this.Timestamp = DateTime.Now;
            GenerateToken();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 时间戳。
        /// </summary>
        public DateTime Timestamp
        {
            get;
            set;
        }

        #region Token

        private String _token;
        /// <summary>
        /// 客户端令牌。
        /// </summary>
        public String Token
        {
            get { return this._token; }
        }

        #endregion

        #region AccountId

        private readonly String _accountId;
        /// <summary>
        /// 账号Id。
        /// </summary>
        public String AccountId
        {
            get { return this._accountId; }
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

        #endregion

        #region Methods

        public String GenerateToken()
        {
            String token = Guid.NewGuid().ToString().Replace("-", String.Empty);
            this._token = token;
            return token;
        }

        #endregion
    }
}
