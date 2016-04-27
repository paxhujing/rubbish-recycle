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
        public static OperationResult<T> GenerateResult<T>(T data, String errorMessage)
        {
            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                AppGlobal.Log.WarnFormat(errorMessage);
            }
            return new OperationResult<T>
            {
                Data = data,
                ErrorMessage = errorMessage,
                IsSuccess = String.IsNullOrWhiteSpace(errorMessage)
            };
        }

        public static OperationResult<T> GenerateSuccessResult<T>(T data)
        {
            return new OperationResult<T> { Data = data, IsSuccess = true };
        }

        public static OperationResult<T> GenerateErrorResult<T>(String errorMessage)
        {
            AppGlobal.Log.WarnFormat(errorMessage);
            return new OperationResult<T> { ErrorMessage = errorMessage, IsSuccess = false };
        }

        public static OperationResult GenerateResult(String errorMessage)
        {
            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                AppGlobal.Log.WarnFormat(errorMessage);
            }
            return new OperationResult
            {
                ErrorMessage = errorMessage,
                IsSuccess = String.IsNullOrWhiteSpace(errorMessage)
            };
        }

        public static OperationResult GenerateSuccessResult()
        {
            return new OperationResult { IsSuccess = true };
        }

        public static OperationResult GenerateErrorResult(String errorMessage)
        {
            AppGlobal.Log.WarnFormat(errorMessage);
            return new OperationResult { ErrorMessage = errorMessage, IsSuccess = false };
        }
    }
}
