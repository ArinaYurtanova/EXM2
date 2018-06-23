using Application.Implementations;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Attributes;
using Unity.Lifetime;

namespace Application
{
    class Program
    {

        static void Main(string[] args)
        {
            var container = BuildUnityContainer();
            Execute exe = new Execute(container.Resolve<IBlogService>(), container.Resolve<ICommentService>(), container.Resolve<IReportService>());
            exe.Run();
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<AbstractDbContext, AbstractDbContext>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IBlogService, BlogService>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ICommentService, CommentService>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IReportService, ReportService>(new HierarchicalLifetimeManager());

            return currentContainer;
        }
    }
}
