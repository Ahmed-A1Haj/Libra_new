using Application.Interfaces;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Persistence
{
    public static class PersistenceDependencyInjection
    {
        public static void Register(ContainerBuilder builder)
        {
            builder.RegisterType<AppDbContext>().As<IAppDbContext>();
        }
    }
}
