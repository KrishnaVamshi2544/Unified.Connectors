
using Newtonsoft.Json;

namespace Unified.Connectors.Model
{
    public class EDiscoveryCase
    {
        public string Odatacontext { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public string Status { get; set; }
        public object ClosedDateTime { get; set; }
        public string ExternalId { get; set; }
        public string Id { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public Lastmodifiedby LastModifiedBy { get; set; }
        public Closedby ClosedBy { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Lastmodifiedby
    {
        public User user { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Closedby
    {
        public User user { get; set; }
    }

    /// <summary>
    /// Case creation response
    /// </summary>
    public class CaseRoot
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        [JsonProperty("@odata.count")]
        public int OdataCount { get; set; }

        [JsonProperty("@odata.nextLink")]
        public string OdataNextLink { get; set; }
        [JsonProperty("value")]
        public List<EDiscoveryCase> Case { get; set; }
    }
}
