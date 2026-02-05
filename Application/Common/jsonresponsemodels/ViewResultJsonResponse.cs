namespace Application.Common.JsonResponseModels
{
    /// <summary>
    /// The class representing base JSON response.
    /// </summary>
    public class ViewResultJsonResponse
    {
        /// <summary>
        /// Gets or sets the execution result code.
        /// </summary>
        public ExecutionResult Result { get; set; }

        /// <summary>
        /// Gets or sets the view converted to string.
        /// </summary>
        public string ViewHtml { get; set; }
    }
}
