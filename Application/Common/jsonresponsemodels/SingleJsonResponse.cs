namespace Application.Common.JsonResponseModels
{
    public class SingleJsonResponse<TRecord> : BaseJsonResponse where TRecord : class
    {
        public TRecord Record { get; set; }
    }
}
