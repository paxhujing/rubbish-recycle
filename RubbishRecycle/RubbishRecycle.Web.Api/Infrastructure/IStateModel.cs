using RubbishRecycle.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Web.Api.Infrastructure
{
    /// <summary>
    /// 表示此类型是用于描述数据模型状态。
    /// </summary>
    public interface IStateModel
    {
        ICollection<Link> Links { get; }
    }
}
