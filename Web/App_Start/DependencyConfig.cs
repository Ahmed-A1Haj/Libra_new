using Application.Extentions;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.SignalR;
using FluentValidation;
using Infrastructure;
using MediatR;
using Microsoft.Owin.Security;
using Persistence;
using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Web.App_Start
{
    public static class DependencyConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();
            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => componentContext.Resolve(t);
            });

            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            builder.Register(a => HttpContext.Current.GetOwinContext().Authentication).As<IAuthenticationManager>();

            builder.AddInfrastructure();
            PersistenceDependencyInjection.Register(builder);
            ApplicationDependencyInjection.RegisterApplication(builder);

            var container = builder.Build();

            DependencyResolver.SetResolver(new Autofac.Integration.Mvc.AutofacDependencyResolver(container));

            
        }
    }
}