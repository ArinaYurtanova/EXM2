using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ZemlyanskiyExam
{
    [DataContract]
    public class Blog
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string AutorName { get; set; }

        [DataMember]
        public DateTime DateCreate { get; set; }

        [ForeignKey("BlogId")]
        public virtual List<Comment> Comments { get; set; }
    }
}
