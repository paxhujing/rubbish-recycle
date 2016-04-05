using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    /// <summary>
    /// 登陆信息。
    /// </summary>
    public class LoginInfo
    {
        /// <summary>
        /// 客户端提供的密钥。
        /// </summary>
        public Byte[] SecretKey { get; set; }

        /// <summary>
        /// 账户名称。
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 密码。
        /// </summary>
        public String Password { get; set; }

    }
}
