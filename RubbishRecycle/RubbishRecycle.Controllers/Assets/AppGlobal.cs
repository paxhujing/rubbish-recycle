﻿using log4net;
using RubbishRecycle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Controllers.Assets
{
    public static class AppGlobal
    {
        public static readonly RubbishRecycleContext DbContext = new RubbishRecycleContext();

        public static readonly ILog Log = LogManager.GetLogger("RubbishRecycleLogger");

        public static OperationResult<T> GenerateResult<T>(T data, String errorMessage)
        {
            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                AppGlobal.Log.WarnFormat(errorMessage);
            }
            return new OperationResult<T>()
            {
                Data = data,
                ErrorMessage = errorMessage,
                IsSuccess = String.IsNullOrWhiteSpace(errorMessage)
            };
        }

        public static OperationResult<T> GenerateSuccessResult<T>(T data)
        {
            return new OperationResult<T>() { Data = data, IsSuccess = true };
        }

        public static OperationResult<T> GenerateErrorResult<T>(String errorMessage)
        {
            AppGlobal.Log.WarnFormat(errorMessage);
            return new OperationResult<T>() { ErrorMessage = errorMessage, IsSuccess = false };
        }
    }
}
