using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using RubbishRecycle.Models;

namespace RubbishRecycle.Test
{
    class Program
    {
        //static readonly Uri BaseAddress = new Uri("http://localhost:49811");
        static readonly Uri BaseAddress = new Uri("http://192.168.1.103:8080");

        static readonly RijndaelManaged AESProvider = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };

        static readonly Byte[] SecretKey = AESProvider.Key;

        static String Token;

        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            String publicKey = RequestCommunication(client);

            Console.WriteLine("Public key: {0}", publicKey);
            String iv = null;
            //Token = Register(client, publicKey, out iv);
            Token = Login(client, publicKey, out iv);
            Console.WriteLine("token: {0};IV: {1}", Token, iv);
            String json = GetAccountInfo(client);
            Console.WriteLine("json: {0}", json);

            Console.ReadKey();
        }

        #region Order

        static void AddOrder()
        {
            Order order = new Order();
        }

        #endregion

        #region Init

        static String RequestCommunication(HttpClient client)
        {
            Uri uri = new Uri(Program.BaseAddress, "api/account/RequestCommunication");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            HttpResponseMessage response = client.SendAsync(request).Result;
            //String publicKey = response.Content.ReadAsStringAsync().Result;
            //return publicKey;
            String str = response.Content.ReadAsStringAsync().Result;
            XmlNode xn = JsonConvert.DeserializeXmlNode(str);
            return xn.InnerXml;
        }

        static String Register(HttpClient client, String publicKey, out String iv)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(publicKey);

            String account = "123456:123456";
            Byte[] data = Encoding.UTF8.GetBytes(account);
            data = provider.Encrypt(data, true);
            account = Convert.ToBase64String(data);

            data = provider.Encrypt(Program.SecretKey, RSAEncryptionPadding.OaepSHA1);
            String secretKey = Convert.ToBase64String(data);

            Uri uri = new Uri(Program.BaseAddress, "api/account/Register?accountType=0");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            request.Content = new StringContent(secretKey);
            request.Headers.Authorization = new AuthenticationHeaderValue("basic", account);
            HttpResponseMessage response = client.SendAsync(request).Result;
            iv = response.Headers.GetValues("IV").First();
            Byte[] encryptIV = Convert.FromBase64String(iv);
            Program.AESProvider.IV = encryptIV;
            return response.Content.ReadAsStringAsync().Result;
        }

        static String Login(HttpClient client, String publicKey, out String iv)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(publicKey);

            String account = "123456:123456";
            Byte[] data = Encoding.UTF8.GetBytes(account);
            data = provider.Encrypt(data, true);
            account = Convert.ToBase64String(data);

            data = provider.Encrypt(Program.SecretKey, true);
            String secretKey = Convert.ToBase64String(data);

            Uri uri = new Uri(Program.BaseAddress, "api/account/Login?accountType=0");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            request.Content = new StringContent(secretKey);
            request.Headers.Authorization = new AuthenticationHeaderValue("basic", account);
            HttpResponseMessage response = client.SendAsync(request).Result;
            iv = response.Headers.GetValues("IV").First();
            Byte[] encryptIV = Convert.FromBase64String(iv);
            Program.AESProvider.IV = encryptIV;
            return response.Content.ReadAsStringAsync().Result;
        }

        static String GetAccountInfo(HttpClient client)
        {
            Uri uri = new Uri(Program.BaseAddress, "api/account/GetAccountInfo?name=123456");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            request.Headers.Authorization = new AuthenticationHeaderValue("basic",Program.Token);
            HttpResponseMessage response = client.SendAsync(request).Result;
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(response.StatusCode.ToString());
                return null;
            }
            String encryptJson = response.Content.ReadAsStringAsync().Result;
            String json = DecryptContent(encryptJson);
            return json;
        }

        #endregion

        #region Crypt

        static String EncryptContent(String json)
        {
            ICryptoTransform encryptor = Program.AESProvider.CreateEncryptor();

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(json);
                        sw.Flush();
                    }
                }
                Byte[] data = ms.ToArray();
                return Convert.ToBase64String(data);
            }
        }

        static String DecryptContent(String encryptJson)
        {
            Byte[] data = Convert.FromBase64String(encryptJson);
            ICryptoTransform decryptor = Program.AESProvider.CreateDecryptor();
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        #endregion

    }
}
