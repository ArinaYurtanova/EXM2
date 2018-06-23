using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class BlogViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DateCreate { get; set; }

        public string AutorName { get; set; }

        public List<CommentViewModel> Comments { get; set; }
    }
}
