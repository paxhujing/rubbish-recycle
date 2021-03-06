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
    }
}
