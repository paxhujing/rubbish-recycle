using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.PC.Contract
{
    public interface IAccountProxy
    {
        String RequestCommunication();

        Task<HttpResponseMessage> RequestCommunicationAsync();
    }
}
