namespace Unified.Connectors.Model
{
    /// <summary>
    /// User source
    /// </summary>
    public class UserSource
    {
        public string odatacontext { get; set; }
        public string displayName { get; set; }
        public DateTime? createdDateTime { get; set; }
        public string id { get; set; }
        public string email { get; set; }
        public string includedSources { get; set; }
        public Createdby createdBy { get; set; }
    }
}
