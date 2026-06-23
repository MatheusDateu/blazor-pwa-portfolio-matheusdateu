using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BlazorDeploy.Models
{
    [Table("portf0l10-0004-_tech_category")]
    public class TechCategoryModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public short Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("icon_class")]
        public string IconClass { get; set; }

        [Column("display_order")]
        public int DisplayOrder { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [Reference(typeof(TechnologyModel))]
        public List<TechnologyModel> Technologies { get; set; } = new();
    }
}