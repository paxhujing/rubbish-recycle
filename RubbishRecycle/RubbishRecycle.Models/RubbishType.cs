using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models
{
    /// <summary>
    /// 废品类型
    /// </summary>
    public enum RubbishType : byte
    {
        /// <summary>
        /// 纸类。
        /// </summary>
        Paper,
        /// <summary>
        /// 塑料。
        /// </summary>
        Plastic,
        /// <summary>
        /// 玻璃。
        /// </summary>
        Glass,
        /// <summary>
        /// 金属。
        /// </summary>
        Metal,
        /// <summary>
        /// 布料。
        /// </summary>
        Cloth,
        /// <summary>
        /// 其它。
        /// </summary>
        Other = Byte.MaxValue
    }
}
