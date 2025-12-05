using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BlazorDeploy.Models
{
    [Table("p0rtf0l10-0001-_project")]
    public class PortfolioProject : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("synopsis")]
        public string Synopsis { get; set; }
        [Column("category_id")]
        public int CategoryId { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("thumbnail_url")]
        public string ThumbnailUrl { get; set; }
        [Column("image_url")]
        public string ImageUrl { get; set; }
        [Column("images_url")]
        public IList<string> ImagesUrl { get; set; } = new List<string>();
        [Column("video_url")]
        public string? VideoUrl { get; set; }
        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
