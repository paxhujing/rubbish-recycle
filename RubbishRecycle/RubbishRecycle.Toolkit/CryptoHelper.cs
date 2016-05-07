using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Toolkit
{
    public static class CryptoHelper
    {
        private static readonly MD5CryptoServiceProvider MD5Provider = new MD5CryptoServiceProvider();

        /// <summary>
        /// 计算字符串的MD5值。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String MD5Compute(String str)
        {
            Byte[] data = Encoding.UTF8.GetBytes(str);
            data = CryptoHelper.MD5Provider.ComputeHash(data);
            return Convert.ToBase64String(data);
        }
    }
}
