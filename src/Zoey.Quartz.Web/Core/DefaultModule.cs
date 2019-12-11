using Autofac;
using Zoey.Quartz.Core.Dependency;
using Zoey.Quartz.Core.Quartz;
using Zoey.Quartz.Job;
using System;
using System.Linq;
using System.Reflection;

namespace Zoey.Quartz.Web.Core
{
    public class DefaultModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //获取所有需要依赖注入的程序集
            Assembly[] assemblies = new Assembly[]
            {
                Assembly.Load("Zoey.Quartz.Infrastructure"),
                Assembly.Load("Zoey.Quartz.Application"),
                Assembly.Load("Zoey.Quartz.Job")
            };

            Type scopTypes = typeof(IScopeDependency);
            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => scopTypes.IsAssignableFrom(type) && !type.IsAbstract)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            Type singleTypes = typeof(ISingleDependency);
            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => singleTypes.IsAssignableFrom(type) && !type.IsAbstract)
                .AsImplementedInterfaces()
                .SingleInstance();

            Type transientTypes = typeof(ITransientDependency);
            builder.RegisterAssemblyTypes(assemblies)
                .Where(type => transientTypes.IsAssignableFrom(type) && !type.IsAbstract)
                .AsImplementedInterfaces()
                .InstancePerDependency();

            Type jobTypes = typeof(ZoeyJob);
            builder.RegisterAssemblyTypes(assemblies)
               .Where(type => jobTypes.IsAssignableFrom(type) && !type.IsAbstract)
               .AsSelf()
               .SingleInstance();
        }
    }
}
