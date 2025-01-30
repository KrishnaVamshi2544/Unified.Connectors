using System.ComponentModel.DataAnnotations.Schema;

namespace Unified.Connectors.EntityModels
{
    [Table("Connectors", Schema = "unified")]
    public class Connector : BaseModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
