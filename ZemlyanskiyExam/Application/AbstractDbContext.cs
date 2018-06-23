using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZemlyanskiyExam;

namespace Application
{
    public class AbstractDbContext:DbContext 
    {
        public AbstractDbContext():base("AbstractBlog")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
            Database.SetInitializer(new DropCreateDatabaseAlways<AbstractDbContext>());
        }

        public virtual DbSet<Blog> Blogs { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }
    }
}
