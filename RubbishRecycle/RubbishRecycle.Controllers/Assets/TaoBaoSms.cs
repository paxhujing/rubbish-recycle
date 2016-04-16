using Newtonsoft.Json;
using RubbishRecycle.Config;
using RubbishRecycle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace RubbishRecycle.Controllers.Assets
{
    internal static class TaoBaoSms
    {
        #region Fields

        private static readonly RubbishRecycleConfigurationManager RCManager = RubbishRecycleConfigurationManager.GetInstance();

        private static readonly Char[] Numbers = new Char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        #endregion

        #region Methods

        public static VerifyCodeSmsResult SendVerifyCode(String bindingPhone, String roleId)
        {
            SmsConfigSection smsConfig = RCManager.SmsConfig;
            if (smsConfig == null) throw new MissingMemberException("miss section 'sms' in config file");
            VerifyCodeSmsElement verifyCodeConfig = smsConfig.VerifyCodeSms;
            if (verifyCodeConfig == null) throw new MissingMemberException("miss element 'verifyCodeSms' in config file");
            ITopClient client = new DefaultTopClient(smsConfig.ServerUrl, smsConfig.AppKey, smsConfig.AppSecretKey, smsConfig.Format);

            AlibabaAliqinFcSmsNumSendRequest request = new AlibabaAliqinFcSmsNumSendRequest();
            request.Extend = roleId;
            request.SmsType = verifyCodeConfig.SmsType;
            request.SmsFreeSignName = verifyCodeConfig.SignName;
            request.RecNum = bindingPhone;
            request.SmsTemplateCode = verifyCodeConfig.TemplateCode;
            String code = GenerateVerifyCode(verifyCodeConfig.Length);
            String roleName = null;
            switch (roleId)
            {
                case "saler":
                    roleName = "卖家方";
                    break;
                case "buyer":
                    roleName = "买家方";
                    break;
                default:
                    throw new InvalidOperationException();
            }
            request.SmsParam = String.Format("{{\"code\":\"{0}\",\"product\":\"【收破烂】{1}\"}}", code, roleName);
            //api/account/GetVerifyCode?bindingPhone=18284559968&roleId=saler
            AlibabaAliqinFcSmsNumSendResponse response = client.Execute(request);
            VerifyCodeSmsResult result = new VerifyCodeSmsResult();
            if (!response.IsError)
            {
                result.Code = code;
                result.Extend = roleId;
                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
                result.Error = response.ErrMsg;
            }
            return result;
        }

        private static String GenerateVerifyCode(Int32 count)
        {
            Random random = new Random((Int32)DateTime.Now.Ticks);
            Char[] chars = new Char[count];
            for (Int32 i = 0; i < count; i++)
            {
                Int32 index = random.Next(0, 9);
                chars[i] = TaoBaoSms.Numbers[index];
            }
            return new String(chars);
        }

        #endregion
    }
}
