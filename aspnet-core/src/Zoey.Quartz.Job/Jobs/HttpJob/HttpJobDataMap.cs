namespace Zoey.Quartz.Job
{
    public class HttpJobDataMap : ZoeyJobDataMap
    {
        /// <summary>
        /// url
        /// </summary>
        public string ApiUrl { get; set; }
        /// <summary>
        /// authkey
        /// </summary>
        public string AuthKey { get; set; }
        /// <summary>
        /// auth值
        /// </summary>
        public string AuthValue { get; set; }
        /// <summary>
        /// 请求类型 get post
        /// </summary>
        public string RequestType { get; set; }
    }
}
