using System;
using System.Data.Common;
using System.Data.SQLite;

namespace Zoey.Quartz.Infrastructure
{
    public class DapperDbContexts
    {
        private static readonly string DbFile = $"{Environment.CurrentDirectory}\\ZoeyQuartz.sqlite";
        private static string Conn =>

                //1.基础连接，FailIfMissing 参数 true=没有数据文件将异常;false=没有数据库文件则创建一个  
                //Data Source=test.db;Pooling=true;FailIfMissing=false  
                //2。使用utf-8 格式  
                //Data Source={0};Version=3;UTF8Encoding=True;  
                //3.禁用日志  
                //Data Source={0};Version=3;UTF8Encoding=True;Journal Mode=Off;  
                //4.连接池  
                //Data Source=c:\mydb.db;Version=3;Pooling=True;Max Pool Size=100;  
                $@"Data Source={ DbFile };Pooling=true;FailIfMissing=false;Version=3;UTF8Encoding=True;Journal Mode=Off;";
        public DbConnection Connection
        {
            get
            {
                return new SQLiteConnection(Conn);
            }
        }
    }
}
