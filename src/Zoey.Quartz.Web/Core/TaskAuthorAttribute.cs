using System;

namespace Zoey.Quartz.Web.Core
{
    public class TaskAuthorAttribute : Attribute
    {
        public string Name { get; set; }
        public string Role { get; set; }
    }
}
