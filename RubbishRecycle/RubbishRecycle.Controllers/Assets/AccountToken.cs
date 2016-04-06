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
        /// <param name="context">安全上下文。</param>
        public AccountToken(String token, AccountSecurityContext context)
        {
            this._securityContext = context;
            this._token = token;
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

        #region Context

        private readonly AccountSecurityContext _securityContext;
        /// <summary>
        /// 安全上下文。
        /// </summary>
        public AccountSecurityContext SecurityContext
        {
            get { return this._securityContext; }
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
