using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BlazorDeploy.Models
{
    [Table("p0rt1f0l10-0002-_category")]
    public class ProjectCategory : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
