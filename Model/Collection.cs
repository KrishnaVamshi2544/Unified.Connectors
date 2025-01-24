using Newtonsoft.Json;

namespace Unified.Connectors.Model
{
    /// <summary>
    /// Source collection response
    /// </summary>
    public class Collection
    {
        public string odatacontext { get; set; }
        public string displayName { get; set; }
        public string description { get; set; }
        public DateTime? lastModifiedDateTime { get; set; }
        public string status { get; set; }
        public object closedDateTime { get; set; }
        public string externalId { get; set; }
        public string Id { get; set; }
        public DateTime? createdDateTime { get; set; }
        public Lastmodifiedby lastModifiedBy { get; set; }
        public Closedby closedBy { get; set; }
    }


    /// <summary>
    /// Source collection request payload
    /// </summary>
    public class CollectionReq
    {
        public string displayName { get; set; }
        public string contentQuery { get; set; }
    }

    /// <summary>
    /// Source collection request payload for Custodial data source
    /// </summary>
    public class CollectionReqForCustodialDS : CollectionReq
    {
        [JsonProperty("custodianSources@odata.bind")]
        public string[] custodianSourcesodatabind { get; set; }

    }

    /// <summary>
    ///  Source collection request payload for Noncustodial data source
    /// </summary>
    public class CollectionReqForNonCustodialDS : CollectionReq
    {
        public string dataSourceScopes { get; set; }
    }
}
