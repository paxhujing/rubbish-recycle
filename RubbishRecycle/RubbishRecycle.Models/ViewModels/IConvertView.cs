using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubbishRecycle.Models.ViewModels
{
    public interface IConvertView<T>
    {
        T ToView();
    }
}
