namespace svc_global_cache.Utilities
{
	public class CommonErrorMap
    {
        private string _status;
        public string status
        {
            get { return _status ?? "ERROR"; }
            set { _status = value; }
        }
        public string reason { get; set; }
    }

    public class CommonSuccessMap
    {

        private string _status;
        public string status
        {
            get { return _status ?? "OK"; }
            set { _status = value; }
        }
        public object detail { get; set; }
    }

    public class CommonResult
    {
        public static CommonSuccessMap GetSucessResult(object result)
        {
            var data = new CommonSuccessMap()
            {
                detail = result
            };
            return data;
        }

        public static CommonErrorMap GetErrorResult(string result)
        {
            var data = new CommonErrorMap()
            {
                reason = result
            };
            return data;
        }
    }
}
