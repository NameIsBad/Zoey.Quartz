using Zoey.Quartz;
using Zoey.Quartz.Core.Dependency;
using Zoey.Quartz.Core.Quartz;
using Zoey.Quartz.Job;
using Microsoft.Extensions.Configuration;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ZoeyQuartzServiceCollectionExtensions
    {
        public static IServiceCollection AddQuartz(this IServiceCollection services, Action<ZoeyQuartzOptions> configureOptions)
        {
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }
            services.AddQuartzCore();
            return services;
        }

        public static IServiceCollection AddQuartz(this IServiceCollection services)
        {
            //IExample example = ObjectUtils.InstantiateType<IExample>(eType);
            IConfigurationRoot config = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("zoey.quartz.json", optional: true, reloadOnChange: true)
                         .Build();
            services.Configure<ZoeyQuartzOptions>(config);
            services.AddQuartzCore();
            return services;
        }

        private static void AddQuartzCore(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            //RegistByInterface(services);
            services.AddSingleton<ISchedulerFactory, ZoeyStdSchedulerFactory>();
            services.AddSingleton<IJobFactory, QuartzFactory>();
            services.AddSingleton<IJobManager, JobManager>();
            services.AddSingleton<ITriggerManager, TriggerManager>();

            #region 初始化Job的ConfigureServices
            Assembly asm = typeof(JobManager).GetTypeInfo().Assembly;
            IEnumerable<Type> types = asm.GetTypes().Where(t => typeof(ZoeyJob).IsAssignableFrom(t));
            if (types.Count() == 0)
            {
                return;
            }

            foreach (Type t in types)
            {
                var s =  (ZoeyJob)Activator.CreateInstance(t);
                s.ConfigureServices(services);
            }
            #endregion
        }

        private static void RegistByInterface(IServiceCollection services)
        {
            //获取所有需要依赖注入的程序集
            var types = new Assembly[]
             {
                Assembly.Load("Zoey.Quartz.Infrastructure"),
                Assembly.Load("Zoey.Quartz.Application"),
                Assembly.Load("Zoey.Quartz.Job")
             }.SelectMany(t => t.GetTypes());

            AutoDi(services, types, typeof(ITransientDependency), 0);
            AutoDi(services, types, typeof(ISingleDependency), 1);
            AutoDi(services, types, typeof(IScopeDependency), 2);
        }

        static void AutoDi(IServiceCollection services, IEnumerable<Type> allTypes, Type baseType, int type)
        {
            var scopeTypes = allTypes.Where(t => baseType.IsAssignableFrom(t));
            var implementScopeTypes = scopeTypes.Where(x => x.IsClass);
            var interfaceScopeTypes = scopeTypes.Where(x => x.IsInterface);
            foreach (Type implementType in implementScopeTypes)
            {
                Type interfaceType = interfaceScopeTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                if (interfaceType != null)
                {
                    switch (type)
                    {
                        case 0:
                            services.AddTransient(interfaceType, implementType);
                            break;
                        case 1:
                            services.AddSingleton(interfaceType, implementType);
                            break;
                        case 2:
                            services.AddScoped(interfaceType, implementType);
                            break;
                    }
                }
            }
        }
    }
}
