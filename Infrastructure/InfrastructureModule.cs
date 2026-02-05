using Application.Interfaces;
using Autofac;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => 
                new CurrentUserService(new HttpContextWrapper(HttpContext.Current)))
                .As<ICurrentUserService>().InstancePerRequest();
            builder.RegisterType<MachineDateTime>().As<IDateTime>().InstancePerRequest();
        }
    }
}
