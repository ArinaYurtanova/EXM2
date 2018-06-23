using Application.BindingModels;
using Application.Interfaces;
using Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZemlyanskiyExam;

namespace Application.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly AbstractDbContext context;

        public CommentService(AbstractDbContext context)
        {
            this.context = context;
        }

        public void AddElement(CommentBindingModel model)
        {
            context.Comments.Add(new Comment
            {
                AutorName = model.AutorName,
                Title = model.Title,
                BlogId = model.BlogId,
                Text = model.Text,
                DateCreate = DateTime.Now
            });
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            var element = context.Comments.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("element not found");
            }

            context.Comments.Remove(element);
            context.SaveChanges();

        }

        public CommentViewModel Get(int id)
        {
            Comment element = context.Comments.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new CommentViewModel
                {
                    Id = element.Id,
                    AutorName = element.AutorName,
                    Title = element.Title,
                    Text = element.Text,
                    BlogId = element.BlogId,
                    DateCreate = element.DateCreate.ToLongDateString()
                   
                };
            }
            throw new Exception("Элемент не найден");
        }

        public List<CommentViewModel> GetList()
        {
            List<CommentViewModel> result = context.Comments
                .Select(rec => new CommentViewModel
                {
                    Id = rec.Id,
                    Text = rec.Text,
                    BlogId=rec.BlogId,
                    Title=rec.Title,
                    AutorName = rec.AutorName,
                    DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateCreate),
                })
                .ToList();
            return result;
        }

        public void UpdElement(CommentBindingModel model)
        {
            Comment element = context.Comments.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Title = model.Title;
            element.Text = model.Text;
            element.AutorName = model.AutorName;
            context.SaveChanges();
        }
    }
}
