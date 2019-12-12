using Autofac;
using System;
using System.Linq;
using System.Reflection;
using Zoey.Quartz.Core.Dependency;
using Zoey.Quartz.Core.Quartz;

namespace ZoeyQuartz.Quartz.Core
{
    public class ZoeyQuartzQuartzModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //获取所有需要依赖注入的程序集
            Assembly[] assemblies = new Assembly[]
            {
                Assembly.Load("ZoeyQuartz.Quartz.Infrastructure"),
                Assembly.Load("ZoeyQuartz.Quartz.Application"),
                Assembly.Load("ZoeyQuartz.Quartz.Job")
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

            Type jobTypes = typeof(ZoeyQuartzJob);
            builder.RegisterAssemblyTypes(assemblies)
               .Where(type => jobTypes.IsAssignableFrom(type) && !type.IsAbstract)
               .AsSelf()
               .SingleInstance();
        }
    }
}
