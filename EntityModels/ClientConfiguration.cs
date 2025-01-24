using System.ComponentModel.DataAnnotations.Schema;

namespace Unified.Connectors.EntityModels
{
    [Table("ClientConfiguration", Schema = "unified")]
    public class ClientConfiguration : BaseModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int ClientId { get; set; }
        public string DataSource { get; set; }
        public string Path { get; set; }

    }
}
