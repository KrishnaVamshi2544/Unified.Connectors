using System.ComponentModel.DataAnnotations.Schema;

namespace Unified.Connectors.EntityModels
{
    [Table("CompanyConfiguration", Schema = "unified")]
    public class CompanyConfiguration : BaseModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int ClientId { get; set; }
        public int ConnectorId { get; set; }
        public string Path { get; set; }

    }
}
