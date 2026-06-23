using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BlazorDeploy.Models
{
    [Table("portf0l10-0009-_project_category")]
    public class ProjectCategoryRelationModel : BaseModel
    {
        [PrimaryKey("project_id", false)]
        public long ProjectId { get; set; }

        [PrimaryKey("category_id", false)]
        public int CategoryId { get; set; }

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Retrieves the associated project category details.
        /// </summary>
        [Reference(typeof(ProjectCategoryModel))]
        public ProjectCategoryModel? Category { get; set; }
    }
}