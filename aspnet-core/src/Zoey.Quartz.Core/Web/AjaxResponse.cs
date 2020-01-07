namespace Zoey.Quartz.Core.Web
{
    public class AjaxResponse
    {
        public AjaxResponse()
            : this(true, string.Empty)
        {

        }

        public AjaxResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; protected set; }

        public string Message { get; set; }

    }

    public class AjaxResponse<T> : AjaxResponse where T : class
    {
        public AjaxResponse(T data)
            : this(true, data)
        {
        }

        public AjaxResponse(bool success, T data)
        {
            base.Success = success;
            Data = data;
        }
        public T Data { get; set; }
    }
}
