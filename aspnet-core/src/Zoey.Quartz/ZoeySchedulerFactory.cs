using Quartz;
using System;
using System.Threading.Tasks;

namespace Zoey.Quartz
{
    public class ZoeySchedulerFactory
    {
        private static readonly string DbFile = $"{Environment.CurrentDirectory}\\quartznet.sqlite";
        public static async Task<IScheduler> GetScheduler()
        {
            SchedulerBuilder builder = SchedulerBuilder.Create()
                    //.UsePersistentStore(persistence =>
                    //    persistence
                    //        .UseSQLite(db =>
                    //            //1.基础连接，FailIfMissing 参数 true=没有数据文件将异常;false=没有数据库文件则创建一个  
                    //            //Data Source=test.db;Pooling=true;FailIfMissing=false  
                    //            //2。使用utf-8 格式  
                    //            //Data Source={0};Version=3;UTF8Encoding=True;  
                    //            //3.禁用日志  
                    //            //Data Source={0};Version=3;UTF8Encoding=True;Journal Mode=Off;  
                    //            //4.连接池  
                    //            //Data Source=c:\mydb.db;Version=3;Pooling=True;Max Pool Size=100; 
                    //            db.WithConnectionString($"Server=localhost;Database=quartznet;")
                    //        )
                    //)
                    ;
            return await builder.Build();
        }
    }
}
