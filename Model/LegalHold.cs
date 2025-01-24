using Microsoft.AspNetCore.Http.HttpResults;

namespace Unified.Connectors.Model
{
    /// <summary>
    /// Legal hold response
    /// </summary>
    public class LegalHold
    {
        public string odatacontext { get; set; }
        public string description { get; set; }
        public DateTime? lastModifiedDateTime { get; set; }
        public bool isEnabled { get; set; }
        public string status { get; set; }
        public string contentQuery { get; set; }
        public object[] errors { get; set; }
        public string Id { get; set; }
        public string displayName { get; set; }
        public DateTime? createdDateTime { get; set; }
        public Createdby createdBy { get; set; }
        public Lastmodifiedby lastModifiedBy { get; set; }
    }

    public class Createdby
    {
        public object application { get; set; }
        public User user { get; set; }
    }
}
