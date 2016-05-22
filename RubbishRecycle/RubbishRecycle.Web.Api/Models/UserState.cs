using RubbishRecycle.Web.Api.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Web.Api.Models
{
    /// <summary>
    /// 用户状态模型。
    /// </summary>
    public class UserState : User, IStateModel
    {
        public UserState()
        {
            _links = new Collection<Link>();
        }

        #region Properties

        #region Links

        private readonly ICollection<Link> _links;
        /// <summary>
        /// 与该数据模型相关操作的链接。
        /// </summary>
        public ICollection<Link> Links
        {
            get { return _links; }
        }

        #endregion

        #endregion
    }
}
