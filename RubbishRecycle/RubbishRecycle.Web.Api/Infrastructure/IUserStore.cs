using RubbishRecycle.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Web.Api.Infrastructure
{
    public interface IUserStore
    {
        Task CreateAsync(User user);
        Task DeleteAsync(String userId);
        Task UpdateAsync(User user);
    }
}
