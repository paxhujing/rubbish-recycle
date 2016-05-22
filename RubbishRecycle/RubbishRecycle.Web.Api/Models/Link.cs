using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Web.Api.Models
{
    /// <summary>
    /// 表示超媒体中的链接关系，
    /// 并包含附加的元数据，描述与链接相关的可选操作。
    /// </summary>
    public class Link
    {
        /// <summary>
        /// 链接名称。
        /// </summary>
        public String Rel { get; set; }
        /// <summary>
        /// 超链接。
        /// </summary>
        public Uri Href { get; set; }
        /// <summary>
        /// 动作。
        /// </summary>
        public String Action { get; set; }
    }
}
