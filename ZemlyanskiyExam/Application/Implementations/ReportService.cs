using Application.BindingModels;
using Application.Interfaces;
using Application.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using ZemlyanskiyExam;

namespace Application.Implementations
{
    public class ReportService : IReportService
    {
        private readonly AbstractDbContext context;

        public ReportService(AbstractDbContext context)
        {
            this.context = context;
        }



        private async Task<List<ReportViewModel>> GetComments(ReportBindingModel model)
        {
            return await context.Comments.Where(rec => rec.DateCreate >= model.DateFrom && rec.DateCreate <= model.DateTo).Include(rec => rec.Blog)
                .Select(rec => new ReportViewModel
                {
                    AutorName=rec.AutorName,
                    BlogDateCreate= SqlFunctions.DateName("dd", rec.Blog.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.Blog.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.Blog.DateCreate),
                    BlogName = rec.Blog.Name,
                    Title = rec.Title,
                    CommentDateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
                                SqlFunctions.DateName("mm", rec.DateCreate) + " " +
                                SqlFunctions.DateName("yyyy", rec.DateCreate)

                }).ToListAsync();
        }

        public async Task CreateReport(ReportBindingModel model)
        {
            FileStream fs = new FileStream(model.Path, FileMode.OpenOrCreate, FileAccess.Write);
            //создаем документ, задаем границы, связываем документ и поток
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            doc.SetMargins(0.5f, 0.5f, 0.5f, 0.5f);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            doc.Open();
            BaseFont baseFont = BaseFont.CreateFont();

            iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(new Phrase("Comments",
                new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD)))
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 12
            };

            var list = await GetComments(model);

            PdfPTable table = new PdfPTable(5);

            var fontForCellBold = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD);

            table.AddCell(new PdfPCell(new Phrase("BlogName", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            table.AddCell(new PdfPCell(new Phrase("DateCreateBlog", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            table.AddCell(new PdfPCell(new Phrase("Title", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            table.AddCell(new PdfPCell(new Phrase("Autor", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            table.AddCell(new PdfPCell(new Phrase("CommentDateCreate", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });

            var fontForCells = new iTextSharp.text.Font(baseFont, 10);

            foreach (var rec in list)
            {
                table.AddCell(new PdfPCell(new Phrase(rec.BlogName, fontForCells)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase(rec.BlogDateCreate, fontForCells)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase(rec.Title, fontForCells)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase(rec.AutorName, fontForCells)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase(rec.CommentDateCreate, fontForCells)) { HorizontalAlignment = Element.ALIGN_CENTER });
            }

            doc.Add(table);

            doc.Close();


        }

        public void SaveToJson(ReportBindingModel model)
        {
            var listBlogs = context.Blogs.ToList();

            var listComments = context.Comments.ToList();

            DataContractJsonSerializer formatterBlog = new DataContractJsonSerializer(typeof(List<Blog>));
            MemoryStream msBlog = new MemoryStream();
            formatterBlog.WriteObject(msBlog, listBlogs);
            msBlog.Position = 0;
            StreamReader srBlog = new StreamReader(msBlog);
            string clientsJSON = srBlog.ReadToEnd();
            srBlog.Close();
            msBlog.Close();

            DataContractJsonSerializer formatterComment = new DataContractJsonSerializer(typeof(List<Comment>));
            MemoryStream msComment = new MemoryStream();
            formatterComment.WriteObject(msComment, listComments);
            msComment.Position = 0;
            StreamReader srComment = new StreamReader(msComment);
            string commentsJSON = srComment.ReadToEnd();
            srComment.Close();
            msComment.Close();

            File.WriteAllText(model.Path, "{\n" +
                "    \"Blogs\": " + clientsJSON + ",\n" +
                "    \"Comments\": " + commentsJSON + ",\n" +
                
                "}");
        }
    }
}
