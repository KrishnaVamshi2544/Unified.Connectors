namespace Unified.Connectors.Model
{
    /// <summary>
    /// Estimate statistics response
    /// </summary>
    public class EstimateStatisticsOperation
    {
        public string odatacontext { get; set; }
        public string odatatype { get; set; }
        public DateTime? createdDateTime { get; set; }
        public DateTime? completedDateTime { get; set; }
        public int? percentProgress { get; set; }
        public string status { get; set; }
        public string action { get; set; }
        public string id { get; set; }
        public long? indexedItemCount { get; set; }
        public long? indexedItemsSize { get; set; }
        public long? unindexedItemCount { get; set; }
        public long? unindexedItemsSize { get; set; }
        public long? mailboxCount { get; set; }
        public long? siteCount { get; set; }
        public Createdby createdBy { get; set; }
    }

    public class HoldStats
    {
        public int? indexedItemCount { get; set; }
        public decimal? indexedItemsSize { get; set; }
    }
}
