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

        #endregion

        #region Constructors

        /// <summary>
        /// 初始化类型 RubbishRecycle.Controllers.Assets.AccountViewer 实例。
        /// </summary>
        /// <param name="phone">绑定的手机。</param>
        /// <param name="cryptor">账号的Id。</param>
        public AccountToken(String phone,String accountId)
        {
            this._phone = phone;
            this._accountId = accountId;
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

        #region AccountId

        private readonly String _accountId;
        /// <summary>
        /// 账号的的Id。
        /// </summary>
        public String AccountId
        {
            get { return this._accountId; }
        }

        #endregion

        #region Phone

        private readonly String _phone;
        /// <summary>
        /// 绑定的手机。
        /// </summary>
        public String Phone
        {
            get { return this._phone; }
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

        #region Token

        public String Token
        {
            get;
            set;
        }

        #endregion

        #endregion
    }
}
