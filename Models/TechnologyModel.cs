using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BlazorDeploy.Models
{
    [Table("portf0l10-0005-_technology")]
    public class TechnologyModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("category_id")]
        public short CategoryId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("search_slug")]
        public string SearchSlug { get; set; }

        [Column("display_order")]
        public int DisplayOrder { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [Reference(typeof(TechCapabilityModel))]
        public List<TechCapabilityModel> Capabilities { get; set; } = new();
    }
}