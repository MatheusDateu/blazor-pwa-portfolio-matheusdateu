using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BlazorDeploy.Models
{
    [Table("portf0l10-0006-_tech_capability")]
    public class TechCapabilityModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("technology_id")]
        public long TechnologyId { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}