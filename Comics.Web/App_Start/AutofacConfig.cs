using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

using Autofac;
using Autofac.Integration.Mvc;

using Comics.Core.Downloaders;
using Comics.Core.Import;
using Comics.Core.Persistence;

namespace Comics.Web
{
    public static class AutofacConfig
    {
        public static void RegisterDependencyInjection()
        {
            var builder = new ContainerBuilder();

            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            //builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();

            RegisterTypes(builder);

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void RegisterTypes(ContainerBuilder builder)
        {
            builder.RegisterType<ComicsRepository>().As<IComicsRepository>();
            builder.RegisterType<ExplosmWebClient>().As<IExplosmWebClient>();
            builder.RegisterType<DilbertWebClient>().As<IDilbertWebClient>();
            builder.RegisterType<ComicsContext>().AsSelf();
            builder.RegisterType<ImportProcess>().As<IImportProcess>();
            builder.RegisterType<DilbertDownloader>().AsSelf();
            builder.RegisterType<ExplosmDownloader>().AsSelf();

            builder.RegisterType<PearlsWebClient>().As<IPearlsWebClient>();
            builder.RegisterType<PearlsDownloader>().AsSelf();

            builder.Register<ComicConfigRegistry>(ComicTypes.RegisterComics).AsSelf();
        }
    }
}