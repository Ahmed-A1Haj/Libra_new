using System.Collections.Generic;

namespace Application.Common.JsonResponseModels
{
    /// <summary>
    /// The class representing base JSON response.
    /// </summary>
    public class ExceptionJsonResponse : BaseJsonResponse
    {
        /// <summary>
        /// Gets the execution result code.
        /// </summary>
        public override ExecutionResult Result => ExecutionResult.EXCEPTION;

        /// <summary>
        /// Gets or sets the stack trace in case of the 
        /// <see cref="ExecutionResult.EXCEPTION"/> execution result.
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the validation failures.
        /// </summary>
        public IDictionary<string, string[]> Failures { get; set; }
    }
}
