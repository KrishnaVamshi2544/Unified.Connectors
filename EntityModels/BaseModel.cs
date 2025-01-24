namespace Unified.Connectors.EntityModels
{
    /// <summary>
    /// Base Model
    /// </summary>
    public class BaseModel
    {
        public virtual int CreatedById { get; set; }
        public virtual int ModifiedById { get; set; }
        public virtual DateTime CreationDate { get; set; } = DateTime.UtcNow;
        public virtual DateTime ModificationDate { get; set; } = DateTime.UtcNow;
        public virtual bool IsDeleted { get; set; } = false;

    }
}
