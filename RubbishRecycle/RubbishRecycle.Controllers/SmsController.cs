using Newtonsoft.Json;
using RubbishRecycle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace RubbishRecycle.Controllers
{
    public class SmsController: ApiController
    {
        private static readonly Char[] Numbers;

        static SmsController()
        {
            SmsController.Numbers = new Char[]
            {
                '0', '1', '2', '3', '4',
                '5', '6', '7', '8', '9',
            };
        }

        [AllowAnonymous]
        public SmsResult GetRegisterVerifyCode(String bindingPhone, String roleId)
        {
            ITopClient client = new DefaultTopClient("http://gw.api.taobao.com/router/rest", "23340825", "253cefc41aed309b2d27c90102569a46", "json");
            AlibabaAliqinFcSmsNumSendRequest request = new AlibabaAliqinFcSmsNumSendRequest();
            request.Extend = roleId;
            request.SmsType = "normal";
            request.SmsFreeSignName = "注册验证";
            request.RecNum = bindingPhone;
            request.SmsTemplateCode = "SMS_7221679";

            String code = GenerateVerifyCode(6);
            var obj = new { code = code, product = "【收破烂】" };
            request.SmsParam = JsonConvert.SerializeObject(obj);
            //api/sms/GetRegisterVerifyCode?bindingPhone=18284559968&roleId=saler
            AlibabaAliqinFcSmsNumSendResponse response = client.Execute(request);
            SmsResult result = new SmsResult();
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
                chars[i] = SmsController.Numbers[index];
            }
            return new String(chars);
        }
    }
}
