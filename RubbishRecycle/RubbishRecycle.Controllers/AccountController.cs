using RubbishRecycle.Controllers.Assets;
using RubbishRecycle.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Web.Http;
using System.Linq;
using System.Text;
using System.Web.Http.Controllers;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;
using RubbishRecycle.Controllers.Assets.DB;
using System.Xml;

namespace RubbishRecycle.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        #region Fields

        #region 非对称加密

        private static readonly RSACryptoServiceProvider RSAProvider;

        private static readonly String GlobalPrivateKey;

        private static readonly String GlobalPublicKey;

        #endregion

        #region 对称加密

        private static readonly RijndaelManaged AESProvider;

        private static readonly Byte[] GlobalIV;

        private static readonly String GlobalIVBase64String;

        #endregion

        private static readonly MD5CryptoServiceProvider MD5Provider;

        #endregion

        #region Constructors

        static AccountController()
        {
            RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
            AccountController.RSAProvider = rsaProvider;
            AccountController.GlobalPrivateKey = rsaProvider.ToXmlString(true);
            AccountController.RSAProvider.FromXmlString(AccountController.GlobalPrivateKey);
            AccountController.GlobalPublicKey = rsaProvider.ToXmlString(false);

            RijndaelManaged aesProvider = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            AccountController.AESProvider = aesProvider;
            AccountController.GlobalIV = aesProvider.IV;
            AccountController.GlobalIVBase64String = Convert.ToBase64String(AccountController.GlobalIV);

            AccountController.MD5Provider = new MD5CryptoServiceProvider();
        }

        public AccountController()
        {

        }

        #endregion

        #region Methods

        [AllowAnonymous]
        [HttpGet]
        [Route("RequestCommunication")]
        public String RequestCommunication(Boolean isHexEncode = true)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(AccountController.GlobalPublicKey);
            String temp = JsonConvert.SerializeXmlNode(doc);

            if (isHexEncode)
            {
                dynamic obj = JsonConvert.DeserializeObject(temp);

                String exponentStr = obj.RSAKeyValue.Exponent;
                exponentStr = ToHexEncode(Convert.FromBase64String(exponentStr));

                String modulusStr = obj.RSAKeyValue.Modulus;
                modulusStr = ToHexEncode(Convert.FromBase64String(modulusStr));

                var rsakeyvalue =
                new
                {
                    RSAKeyValue =
                    new
                    {
                        Exponent = exponentStr,
                        Modulus = modulusStr
                    }
                };
                return JsonConvert.SerializeObject(rsakeyvalue);
            }
            else
            {
                return temp;
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public HttpResponseMessage Login([FromBody]String encryptedSecretKey,[FromUri]AccountType accountType)
        {
            KeyValuePair<String, String> account;
            if (TryGetAccountAndPassword(base.ActionContext, out account))
            {
                //验证用户
                if (VerifyAccount(account.Key, account.Value))
                {
                    Byte[] secretKey = GetClientSecretKey(encryptedSecretKey);
                    if (secretKey != null && secretKey.Length != 0)
                    {
                        return InitAccountToken(secretKey,account,accountType);
                    }
                }
            }
            throw new HttpResponseException(HttpStatusCode.NotAcceptable);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public HttpResponseMessage Register([FromBody]String encryptedSecretKey, [FromUri]AccountType accountType)
        {
            KeyValuePair<String, String> account;
            if (TryGetAccountAndPassword(base.ActionContext, out account))
            {
                //验证用户
                if (RegisterAccount(account.Key, account.Value))
                {
                    Byte[] secretKey = GetClientSecretKey(encryptedSecretKey);
                    if (secretKey != null && secretKey.Length != 0)
                    {
                        return InitAccountToken(secretKey, account, accountType);
                    }
                }
            }
            throw new HttpResponseException(HttpStatusCode.NotAcceptable);
        }

        [RubbishRecycleAuthorize(Roles ="saler")]
        [HttpGet]
        [Route("GetAccountInfo")]
        public String GetAccountInfo([FromUri]String name)
        {
            using (RubbishRecycleContext context = new RubbishRecycleContext())
            {
                Account account = context.Accounts.FirstOrDefault(x=>x.Name == name);
                if (account == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                AccountToken accountToken = (AccountToken)base.ActionContext.Request.Properties["AccountToken"];
                String json = JsonConvert.SerializeObject(account);
                return accountToken.SecurityContext.Encrypt(json);
            }
        }

        #region Private

        /// <summary>
        /// 注册用户信息。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private Boolean RegisterAccount(String name, String password)
        {
            using (RubbishRecycleContext context = new RubbishRecycleContext())
            {
                Account account = new Account();
                account.Name = name;
                account.Password = password;
                account.LastLogin = DateTime.Now.Date;
                context.Accounts.Add(account);
                Int32 result = context.SaveChanges();
                return result != 0;
            }
        }

        /// <summary>
        /// 验证用户信息。
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private Boolean VerifyAccount(String name, String password)
        {
            using (RubbishRecycleContext context = new RubbishRecycleContext())
            {
                Account account = context.Accounts.FirstOrDefault(x => x.Name == name && x.Password == password);
                return account != null;
            }
        }

        /// <summary>
        /// 初始化账户令牌。
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="account"></param>
        /// <param name="accountType"></param>
        /// <returns></returns>
        private HttpResponseMessage InitAccountToken(Byte[] secretKey, KeyValuePair<String, String> account, AccountType accountType)
        {
            AccountToken accountToken = CreateAccountToken(secretKey, account.Key, account.Value);
            accountToken.Roles.Add(accountType == AccountType.Saler ? "saler" : "buyer");
            AccountTokenManager.Manager.Add(accountToken);
            HttpResponseMessage response = base.ActionContext.Request.CreateResponse(accountToken.Token);
            response.Headers.Add("IV", AccountController.GlobalIVBase64String);
            return response;
        }

        /// <summary>
        /// 获取认证信息。
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="account"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private Boolean TryGetAccountAndPassword(HttpActionContext actionContext, out KeyValuePair<String, String> account)
        {
            AuthenticationHeaderValue authenticationHeader = actionContext.Request.Headers.Authorization;
            if (authenticationHeader != null)
            {
                String arg = authenticationHeader.Parameter;
                if (!String.IsNullOrEmpty(arg))
                {
                    Byte[] data = Convert.FromBase64String(arg);
                    data = AccountController.RSAProvider.Decrypt(data, RSAEncryptionPadding.OaepSHA1);
                    String temp = Encoding.UTF8.GetString(data);
                    String[] accountAndPassword = temp.Split(':');
                    if (accountAndPassword.Length == 2)
                    {
                        account = new KeyValuePair<String, String>(accountAndPassword[0], accountAndPassword[1]);
                        return true;
                    }
                }
            }
            account = new KeyValuePair<String, String>();
            return false;
        }

        /// <summary>
        /// 获取客户端设置密钥。
        /// </summary>
        /// <param name="encryptedSecretKey">加密后的客户端密钥。</param>
        /// <returns>解密后的客户端密钥。</returns>
        private Byte[] GetClientSecretKey(String encryptedSecretKey)
        {
            Byte[] data = Convert.FromBase64String(encryptedSecretKey);
            data = AccountController.RSAProvider.Decrypt(data, RSAEncryptionPadding.OaepSHA1);
            return data;
        }

        /// <summary>
        /// 创建通信的安全上下文。
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        private AccountToken CreateAccountToken(Byte[] secretKey, String account, String password)
        {
            try
            {
                //创建安全上下文
                ICryptoTransform decryptor = AccountController.AESProvider.CreateDecryptor(secretKey, AccountController.GlobalIV);
                ICryptoTransform encryptor = AccountController.AESProvider.CreateEncryptor(secretKey, AccountController.GlobalIV);
                AccountSecurityContext context = new AccountSecurityContext(encryptor, decryptor);
                //生成令牌
                String temp = String.Format("{0}-{1}-{2}:{3}", secretKey.GetHashCode(), DateTime.Now.Ticks, account, password);
                Byte[] data = Encoding.UTF8.GetBytes(temp);
                data = AccountController.MD5Provider.ComputeHash(data);
                String token = Convert.ToBase64String(data);
                AccountToken accountToken = new AccountToken(token, context);
                return accountToken;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
        }

        private static String ToHexEncode(Byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", String.Empty);
        }

        #endregion

        #endregion
    }
}
