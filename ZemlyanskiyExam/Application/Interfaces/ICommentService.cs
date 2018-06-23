using Application.BindingModels;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICommentService
    {
        List<CommentViewModel> GetList();

        CommentViewModel Get(int id);

        void AddElement(CommentBindingModel model);

        void UpdElement(CommentBindingModel model);

        void DelElement(int id);
    }
}
