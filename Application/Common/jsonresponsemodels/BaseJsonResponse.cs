namespace Application.Common.JsonResponseModels
{
    /// <summary>
    /// The class representing base JSON response.
    /// </summary>
    public class BaseJsonResponse
    {
        /// <summary>
        /// Gets or sets the execution result code.
        /// </summary>
        public virtual ExecutionResult Result { get; set; }

        /// <summary>
        /// Gets or sets the execution message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether should be show the 
        /// toast message in the frontend.
        /// </summary>
        public bool ShowToast { get; set; }
    }
}
