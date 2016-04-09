using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Toolkit
{
    /// <summary>
    /// AES 加密/解密器。
    /// </summary>
    public class AESCryptor
    {
        #region Constructors

        /// <summary>
        /// 初始化类型 RubbishRecycle.Controllers.Assets.AESCryptor 实例。
        /// </summary>
        /// <param name="encryptor">加密器。</param>
        /// <param name="decryptor">解密器。</param>
        public AESCryptor(ICryptoTransform encryptor, ICryptoTransform decryptor)
        {
            this._encryptor = encryptor;
            this._decryptor = decryptor;
        }

        #endregion

        #region Properties

        #region Encryptor

        private readonly ICryptoTransform _encryptor;
        /// <summary>
        /// 加密器。
        /// </summary>
        public ICryptoTransform Encryptor
        {
            get { return this._encryptor; }
        }

        #endregion

        #region Decryptor

        private readonly ICryptoTransform _decryptor;
        /// <summary>
        /// 解密器。
        /// </summary>
        public ICryptoTransform Decryptor
        {
            get { return this._decryptor; }
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// 加密。
        /// </summary>
        /// <param name="original">原始文本。</param>
        /// <returns>加密后的文本。</returns>
        public String Encrypt(String original)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, this.Encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(original);
                        sw.Flush();
                    }
                }
                Byte[] data = ms.ToArray();
                return Convert.ToBase64String(data);
            }
        }

        /// <summary>
        /// 解密。
        /// </summary>
        /// <param name="encryted">加密后的文本。</param>
        /// <returns>原始文本。</returns>
        public String Dencrypt(String encryted)
        {
            Byte[] data = Convert.FromBase64String(encryted);
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (CryptoStream cs = new CryptoStream(ms, this.Decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadLine();
                    }
                }
            }
        }

        #endregion
    }
}
