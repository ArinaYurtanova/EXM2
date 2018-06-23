using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ZemlyanskiyExam
{
    [DataContract]
    public class Comment
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public DateTime DateCreate { get; set; }

        [DataMember]
        public string AutorName { get; set; }

        [DataMember]
        public int BlogId { get; set; }

        public virtual Blog Blog { get; set; }
    }
}
