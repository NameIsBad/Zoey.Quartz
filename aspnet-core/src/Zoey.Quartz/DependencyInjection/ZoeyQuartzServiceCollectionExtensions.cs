using Zoey.Quartz;
using Zoey.Quartz.Job;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ZoeyQuartzServiceCollectionExtensions
    {
        public static IServiceCollection AddQuartz(this IServiceCollection services)
        {
            services.AddQuartzCore();
            return services;
        }

        private static void AddQuartzCore(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IScheduler>(t =>
            {
                System.Runtime.CompilerServices.ConfiguredTaskAwaitable<IScheduler> scheduler = ZoeySchedulerFactory.GetScheduler().ConfigureAwait(false);
                return scheduler.GetAwaiter().GetResult();
            });
            services.AddSingleton<IJobFactory, JobFactory>();

            #region 初始化Job的ConfigureServices
            List<Type> types = Assembly.Load("Zoey.Quartz.Job").GetTypes()
                .Where(t => !t.IsAbstract && HasImplementedRawGeneric(t, typeof(IZoeyJob<>))).ToList();
            if (types.Count == 0)
            {
                return;
            }

            foreach (Type t in types)
            {
                IZoeyJob<ZoeyJobDataMap> s = (IZoeyJob<ZoeyJobDataMap>)Activator.CreateInstance(t);
                s.ConfigureServices(services);
            }
            #endregion
        }

        /// <summary>
        /// 判断指定的类型 <paramref name="type"/> 是否是指定泛型类型的子类型，或实现了指定泛型接口。
        /// </summary>
        /// <param name="type">需要测试的类型。</param>
        /// <param name="generic">泛型接口类型，传入 typeof(IXxx&lt;&gt;)</param>
        /// <returns>如果是泛型接口的子类型，则返回 true，否则返回 false。</returns>
        public static bool HasImplementedRawGeneric(Type type, Type generic)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (generic == null)
            {
                throw new ArgumentNullException(nameof(generic));
            }

            // 测试接口。
            bool isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
            if (isTheRawGenericType)
            {
                return true;
            }

            // 测试类型。
            while (type != null && type != typeof(object))
            {
                isTheRawGenericType = IsTheRawGenericType(type);
                if (isTheRawGenericType)
                {
                    return true;
                }

                type = type.BaseType;
            }

            // 没有找到任何匹配的接口或类型。
            return false;

            // 测试某个类型是否是指定的原始接口。
            bool IsTheRawGenericType(Type test)
            {
                return generic == (test.IsGenericType ? test.GetGenericTypeDefinition() : test);
            }
        }
    }
}
