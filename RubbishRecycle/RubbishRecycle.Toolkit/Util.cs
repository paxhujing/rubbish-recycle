using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Toolkit
{
    public static class Util
    {
        public static void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidationCallback;
        }

        private static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
            //if (sslPolicyErrors == SslPolicyErrors.None)
            //{
            //    return true;
            //}
            //else
            //{
            //    if ((SslPolicyErrors.RemoteCertificateNameMismatch & sslPolicyErrors) == SslPolicyErrors.RemoteCertificateNameMismatch)
            //    {
            //        Debug.WriteLine("证书名称不匹配：{0}", sslPolicyErrors);
            //    }
            //    if ((SslPolicyErrors.RemoteCertificateChainErrors & sslPolicyErrors) == SslPolicyErrors.RemoteCertificateChainErrors)
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        foreach (X509ChainStatus status in chain.ChainStatus)
            //        {
            //            sb.AppendFormat("status code: {0};info: {1}\n", status.Status, status.StatusInformation);
            //        }
            //        Debug.WriteLine("证书链错误：{0}", sb.ToString());
            //    }
            //    return false;
            //}
        }
    }
}
