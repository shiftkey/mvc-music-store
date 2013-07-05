using Autofac;
using Autofac.Integration.Mvc;
using MvcMusicStore.Filters;
using MvcMusicStore.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using MvcMusicStore.Models.DataAccess;
using WebMatrix.WebData;

namespace MvcMusicStore
{
    public static class AppConfig
    {
        public static void Configure()
        {
            System.Data.Entity.Database.SetInitializer(new SampleData());

            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterType<AlbumRepository>().AsImplementedInterfaces();
            builder.RegisterType<InMemoryCacheService>().AsImplementedInterfaces();

            builder.Register<Func<int, List<Album>>>(a =>
            {
                var cacheService = a.Resolve<ICacheService>();
                var repository = a.Resolve<IAlbumRepository>();
                return count => cacheService.Get("top-selling-albums", () => repository.GetTopSellingAlbums(count));
            });

            //builder.RegisterDecorator<IAlbumRepository>(
            //    (c, inner) => new CacheableAlbumRepository(inner, c.Resolve<ICacheService>()), "repository");

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