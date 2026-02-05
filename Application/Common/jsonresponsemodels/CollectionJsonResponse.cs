using System.Collections.Generic;

namespace Application.Common.JsonResponseModels
{
    public class CollectionJsonResponse<TRecord> : BaseJsonResponse where TRecord : class
    {
        public IEnumerable<TRecord> Records { get; set; }
    }
}
