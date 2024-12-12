using System;

namespace Book.Store.Api.Server
{
    public class BaseDataModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateModified { get; set; }
    }
}