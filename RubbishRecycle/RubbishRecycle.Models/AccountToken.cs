using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    public class AccountToken
    {
        #region Constructors

        /// <summary>
        /// 初始化类型RubbishRecycle.ModelsAccountToken.实例。
        /// </summary>
        /// <param name="secretKey">客户端要求使用的密钥。</param>
        public AccountToken(String secretKey)
        {
            this._secretKey = secretKey;
            this._roles = new List<String>(1);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 客户端令牌。
        /// </summary>
        public String Token
        {
            get;
            set;
        }

        /// <summary>
        /// 客户端最近一次请求日期时间。
        /// 每次检查周期移除时间间隔超过30秒的客户端。
        /// </summary>
        public DateTime LastRequest
        {
            get;
            set;
        }

        #region PrivateKey

        private readonly String _secretKey;
        /// <summary>
        /// 客户端要求使用的密钥。
        /// </summary>
        public String SecretKey
        {
            get { return this._secretKey; }
        }

        #endregion

        #region Roles

        private IList<String> _roles;
        /// <summary>
        /// 角色列表。
        /// </summary>
        public IList<String> Roles
        {
            get { return this._roles; }
        }

        #endregion

        #endregion
    }
}
