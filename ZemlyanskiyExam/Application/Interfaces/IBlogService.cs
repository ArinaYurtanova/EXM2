using Application.BindingModels;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBlogService
    {
        List<BlogViewModel> GetList();

        BlogViewModel Get(int id);

        void AddElement(BlogBindingModel model);

        void UpdElement(BlogBindingModel model);

        void DelElement(int id);
    }
}
