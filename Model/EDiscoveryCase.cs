
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

    public class Lastmodifiedby
    {
        public User user { get; set; }
    }

    public class Closedby
    {
        public User user { get; set; }
    }
}
