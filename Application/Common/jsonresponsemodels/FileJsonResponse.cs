namespace Application.Common.JsonResponseModels
{
    public class FileJsonResponse : BaseJsonResponse
    {
        public override ExecutionResult Result => ExecutionResult.OK;
        public byte[] Bytes { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}
