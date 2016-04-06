using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    /// <summary>
    /// 登陆结果。
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// 对称加密向量。
        /// </summary>
        public Byte[] IV { get; set; }

        /// <summary>
        /// 会话Token。
        /// </summary>
        public String Token { get; set; }
    }
}
