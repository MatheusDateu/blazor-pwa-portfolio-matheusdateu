using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BlazorDeploy.Models
{
    [Table("portf0l10-0008-_project_tech")]
    public class ProjectTechCapabilityModel : BaseModel
    {
        [PrimaryKey("project_id", false)]
        public long ProjectId { get; set; }

        [PrimaryKey("tech_capability_id", false)]
        public long TechCapabilityId { get; set; }

        /// <summary>
        /// Retrieves the associated technical capability details.
        /// </summary>
        [Reference(typeof(TechCapabilityModel))]
        public TechCapabilityModel? Capability { get; set; }
    }
}