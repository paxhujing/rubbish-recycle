using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace RubbishRecycle.Controllers.Assets
{
    public class UnhandleExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Exception ex = actionExecutedContext.Exception;
            AppGlobal.Log.ErrorFormat("Exception: {0}\r\n{1}", ex.Message, ex.StackTrace);
            base.OnException(actionExecutedContext);
        }
    }
}
