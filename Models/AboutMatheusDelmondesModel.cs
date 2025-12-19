using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BlazorDeploy.Models
{
    [Table("portf0l10-0003-_about")]
    public class AboutMatheusDelmondesModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public short Id { get; set; }
        [Column("about_matheus_delmondes_title")]
        public string AboutMeTitle { get; set; }
        [Column("about_matheus_delmondes_content")]
        public string AboutMeContent { get; set; }
        [Column("about_portfolio_title")]
        public string AboutPortfolioTitle { get; set; }
        [Column("about_portfolio_content")]
        public string AboutPortfolioContent { get; set; }
        [Column("developer_mindset_title")]
        public string DeveloperMindsetTitle { get; set; }
        [Column("developer_mindset_content")]
        public string DeveloperMindsetContent { get; set; }
        [Column("techlead_mindset_title")]
        public string TechLeadMindsetTitle { get; set; }
        [Column("techlead_mindset_content")]
        public string TechLeadMindsetContent { get; set; }
        [Column("dotnet_and_automation_title")]
        public string DotnetAndAutomationTitle { get; set; }
        [Column("dotnet_and_automation_content")]
        public string DotnetAndAutomationContent { get; set; }
        [Column("python_and_automation_title")]
        public string PythonAndAutomationTitle { get; set; }
        [Column("python_and_automation_content")]
        public string PythonAndAutomationContent { get; set; }
        [Column("angular_and_react_title")]
        public string AngularAndReactTitle { get; set; }
        [Column("angular_and_react_content")]
        public string AngularAndReactContent { get; set; }
        [Column("java_and_spring_title")]
        public string JavaAndSpringTitle { get; set; }
        [Column("java_and_spring_content")]
        public string JavaAndSpringContent { get; set; }
        [Column("invitation_title")]
        public string InvitationTitle { get; set; }
        [Column("invitation_content")]
        public string InvitationContent { get; set; }
        [Column("contact_title")]
        public string ContactTitle { get; set; }
        [Column("contact_content")]
        public string ContactContent { get; set; }
        [Column("mobile_and_multiplatform_title")]
        public string MobileAndMultiplatformTitle { get; set; }
        [Column("mobile_and_multiplatform_content")]
        public string MobileAndMultiplatformContent { get; set; }
        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}