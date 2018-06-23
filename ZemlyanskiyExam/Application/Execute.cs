using Application.BindingModels;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Attributes;

namespace Application
{
    public class Execute
    {

        private readonly IBlogService blogService;

        private readonly ICommentService commentService;

        private readonly IReportService reportService;

        public Execute(IBlogService blogService, ICommentService commentService, IReportService reportService)
        {
            this.blogService = blogService;

            this.commentService = commentService;

            this.reportService = reportService;
        }

        public void Run()
        {
            Console.Write("Creating context...");
            Init();
            Console.WriteLine("complited");

            SaveToPdf(new ReportBindingModel
            {
                Path = "report.pdf",
                DateFrom= DateTime.MinValue,
                DateTo = DateTime.Now
            });

            SavaToJson(new ReportBindingModel
            {
                Path = "bachup.json"
            });

            Console.ReadLine();
        }

        private void Init()
        {
            for (int i = 0; i < 4; i++)
            {
                blogService.AddElement(new BindingModels.BlogBindingModel
                {
                    Name = string.Format("Blog {0}", i),
                    AutorName = string.Format("Autor {0}", i)
                });
                for (int j = 0; j < 2; j++)
                {
                    commentService.AddElement(new BindingModels.CommentBindingModel
                    {
                        BlogId = (i + 1),
                        AutorName = string.Format("Autor {0}", j),
                        Title = string.Format("Title {0}", j),
                        Text = string.Format("Text {0}", j),
                    });
                }
            }
        }

        private void SaveToPdf(ReportBindingModel model)
        {
            reportService.CreateReport(model);
            Console.WriteLine("saved to pdf");
        }

        private void SavaToJson(ReportBindingModel model)
        {
            reportService.SaveToJson(model);
            Console.WriteLine("saved to json");
        }

    }
}
