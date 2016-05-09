using RubbishRecycle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Controllers.Assets
{
    internal static class OperationResultHelper
    {
        public static OperationResult GenerateResult(Object data, String errorMessage)
        {
            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                AppGlobal.Log.WarnFormat(errorMessage);
            }
            return new OperationResult
            {
                Data = data,
                ErrorMessage = errorMessage,
                IsSuccess = String.IsNullOrWhiteSpace(errorMessage)
            };
        }

        public static OperationResult GenerateSuccessResult(Object data = null)
        {
            return new OperationResult
            {
                IsSuccess = true,
                Data = data
            };
        }

        public static OperationResult GenerateErrorResult(String errorMessage)
        {
            AppGlobal.Log.WarnFormat(errorMessage);
            return new OperationResult
            {
                ErrorMessage = errorMessage,
                IsSuccess = false
            };
        }
    }
}
