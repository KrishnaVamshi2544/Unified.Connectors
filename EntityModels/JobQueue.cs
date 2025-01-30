using System.ComponentModel.DataAnnotations.Schema;

namespace Unified.Connectors.EntityModels
{
    [Table("JobQueue", Schema = "unified")]
    public class JobQueue : BaseModel
    {
        public int Id { get; set; }
        public Guid JobId { get; set; }
        public string? Error { get; set; }
        public string? Status { get; set; }
        public int? Count { get; set; }
        public decimal? Size { get; set; }
    }
}
