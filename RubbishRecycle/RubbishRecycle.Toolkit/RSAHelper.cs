using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Toolkit
{
    public static class RSAHelper
    {
        public static String Encrypt(this RSACryptoServiceProvider provider,String original)
        {
            Byte[] data = Encoding.UTF8.GetBytes(original);
            Int32 maxLength = provider.KeySize / 8 - 11;
            if (data.Length <= maxLength)
            {
                Byte[] result = provider.Encrypt(data, false);
                return Convert.ToBase64String(result);
            }
            else
            {
                Byte[] buffer = new Byte[maxLength];
                using (MemoryStream input = new MemoryStream(data))
                {
                    using (MemoryStream output = new MemoryStream())
                    {
                        Int32 readCount = input.Read(buffer, 0, maxLength);
                        while (readCount > 0)
                        {
                            Byte[] temp = new Byte[readCount];
                            Array.Copy(buffer, 0, temp, 0, readCount);

                            Byte[] block = provider.Encrypt(temp, false);
                            output.Write(block, 0, block.Length);
                            readCount = input.Read(buffer, 0, maxLength);
                        }
                        return Convert.ToBase64String(output.ToArray());
                    }
                }
            }
        }

        public static String Decrypt(this RSACryptoServiceProvider provider, String encrypted)
        {
            Int32 maxLength = provider.KeySize / 8;
            Byte[] data = Convert.FromBase64String(encrypted);
            if (data.Length <= maxLength)
            {
                data = provider.Decrypt(data, false);
            }
            else
            {
                Byte[] buffer = new Byte[maxLength];
                using (MemoryStream input = new MemoryStream(data))
                {
                    using (MemoryStream output = new MemoryStream())
                    {
                        Int32 readCount = input.Read(buffer, 0, maxLength);
                        while (readCount > 0)
                        {
                            Byte[] block = provider.Decrypt(buffer, false);
                            output.Write(block, 0, block.Length);
                            readCount = input.Read(buffer, 0, maxLength);
                        }
                        data = output.ToArray();
                    }
                }
            }
            return Encoding.UTF8.GetString(data);
        }
    }
}
