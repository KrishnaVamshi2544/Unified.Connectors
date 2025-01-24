using System.ComponentModel.DataAnnotations.Schema;

namespace Unified.Connectors.EntityModels
{
    [Table("Company", Schema = "unified")]
    public class Company : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
    }
}
