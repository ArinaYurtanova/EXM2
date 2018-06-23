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
    public class BlogService : IBlogService
    {
        private readonly AbstractDbContext context;

        public BlogService(AbstractDbContext context)
        {
            this.context = context;
        }

        public void AddElement(BlogBindingModel model)
        {
            Blog element = context.Blogs.FirstOrDefault(rec => rec.Name == model.Name);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            context.Blogs.Add(new Blog
            {
                AutorName = model.AutorName,
                Name = model.Name,
                DateCreate = DateTime.Now
            });
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            var element = context.Blogs.FirstOrDefault(rec => rec.Id == id);
            if (element == null)
            {
                throw new Exception("element not found");
            }

            context.Blogs.Remove(element);
            context.SaveChanges();

        }

        public BlogViewModel Get(int id)
        {
            Blog element = context.Blogs.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new BlogViewModel
                {
                    Id = element.Id,
                    AutorName = element.AutorName,
                    Name = element.Name,
                    Comments = context.Comments
                            .Where(recM => recM.BlogId == element.Id)
                            .Select(recM => new CommentViewModel
                            {
                                AutorName = recM.AutorName,
                                BlogId = recM.BlogId,
                                Id = recM.Id,
                                Text = recM.Text,
                                Title = recM.Title,
                                DateCreate = SqlFunctions.DateName("dd", recM.DateCreate) + " " +
                                SqlFunctions.DateName("mm", recM.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", recM.DateCreate)

                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public List<BlogViewModel> GetList()
        {
            List<BlogViewModel> result = context.Blogs
                .Select(rec => new BlogViewModel
                {
                    Id = rec.Id,
                    Name = rec.Name,
                    AutorName = rec.AutorName,
                    DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateCreate),
                })
                .ToList();
            return result;
        }

        public void UpdElement(BlogBindingModel model)
        {
            Blog element = context.Blogs.FirstOrDefault(rec =>
                                    rec.Name == model.Name && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть блог с таким именем");
            }
            element = context.Blogs.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.Name = model.Name;
            element.AutorName = model.AutorName;
            context.SaveChanges();
        }
    }
}
