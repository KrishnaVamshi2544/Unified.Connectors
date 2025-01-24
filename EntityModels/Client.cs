using System.ComponentModel.DataAnnotations.Schema;

namespace Unified.Connectors.EntityModels
{
    [Table("Client", Schema = "unified")]
    public class Client : BaseModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }

    }
}
