namespace Unified.Connectors.Model
{
    /// <summary>
    /// Custodian response
    /// </summary>
    public class Custodian
    {
        public string odatacontext { get; set; }
        public string status { get; set; }
        public string holdStatus { get; set; }
        public DateTime? createdDateTime { get; set; }
        public DateTime? lastModifiedDateTime { get; set; }
        public object releasedDateTime { get; set; }
        public string Id { get; set; }
        public string displayName { get; set; }
        public string email { get; set; }
        public bool applyHoldToSources { get; set; }
        public DateTime? acknowledgedDateTime { get; set; }
    }
}
