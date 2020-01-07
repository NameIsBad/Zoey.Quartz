using Quartz;
using Quartz.Impl;
using Quartz.Impl.Calendar;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using Quartz.Logging;
using Quartz.Simpl;
using Quartz.Spi;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zoey.Quartz.Core.Quartz
{
    public class ZoeyStdSchedulerFactory : StdSchedulerFactory
    {
        public ZoeyStdSchedulerFactory()
        {
            var builder = SchedulerBuilder.Create()
                .UsePersistentStore(persistence =>
                    persistence
                        .UseSQLite(db =>
                            db.WithConnectionString("Server=localhost;Database=quartznet;")
                        )
                );
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
