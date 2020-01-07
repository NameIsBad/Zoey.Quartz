using System;

namespace Zoey.Quartz.Job
{
    /// <summary>
    /// 作业属性
    /// </summary>
    public class ZoeyJobAttribute : Attribute
    {
        public ZoeyJobAttribute()
        {
        }
        public ZoeyJobAttribute(string name,string description = "")
        {
            this.Name = name;
            this.Description = description;
        }
        /// <summary>
        /// 作业名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 作业描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// DataMap类型
        /// </summary>
        public Type DataMapType { get; set; }
    }
}
