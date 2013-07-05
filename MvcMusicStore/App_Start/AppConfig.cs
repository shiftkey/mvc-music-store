using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using MvcMusicStore.Filters;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace MvcMusicStore
{
    public static class AppConfig
    {
        public static void Configure()
        {
            System.Data.Entity.Database.SetInitializer(new MvcMusicStore.Models.SampleData());

            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<AlbumRepository>().Named<IAlbumRepository>("inner");
            builder.RegisterType<InMemoryCacheService>().AsImplementedInterfaces();

            builder.RegisterDecorator<IAlbumRepository>(
                (c, inner) => new CacheableAlbumRepository(inner, c.Resolve<ICacheService>()), "inner");

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            CreateAdminUser();
        }

        private static void CreateAdminUser()
        {
            string _username = ConfigurationManager.AppSettings["DefaultAdminUsername"];
            string _password = ConfigurationManager.AppSettings["DefaultAdminPassword"];
            string _role = "Administrator";

            new InitializeSimpleMembershipAttribute().OnActionExecuting(null);

            if (!WebSecurity.UserExists(_username))
                WebSecurity.CreateUserAndAccount(_username, _password);

            if (!Roles.RoleExists(_role))
                Roles.CreateRole(_role);

            if (!Roles.IsUserInRole(_username, _role))
                Roles.AddUserToRole(_username, _role);
        }
    }
}