using BlazorDeploy.Shared;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Globalization;

namespace BlazorDeploy.Models
{
    [Table("portf0l10-0003-_about")]
    public class AboutMatheusDelmondesModel : BaseModel
    {
        #region Columns
        [PrimaryKey("id", false)]
        public short Id { get; set; }
        [Column("about_matheus_delmondes_title")]
        public Dictionary<string, string> AboutMeTitle { get; set; }
        [Column("about_matheus_delmondes_content")]
        public Dictionary<string, string> AboutMeContent { get; set; }
        [Column("about_portfolio_title")]
        public Dictionary<string, string> AboutPortfolioTitle { get; set; }
        [Column("about_portfolio_content")]
        public Dictionary<string, string> AboutPortfolioContent { get; set; }
        [Column("developer_mindset_title")]
        public Dictionary<string, string> DeveloperMindsetTitle { get; set; }
        [Column("developer_mindset_content")]
        public Dictionary<string, string> DeveloperMindsetContent { get; set; }
        [Column("techlead_mindset_title")]
        public Dictionary<string, string> TechLeadMindsetTitle { get; set; }
        [Column("techlead_mindset_content")]
        public Dictionary<string, string> TechLeadMindsetContent { get; set; }
        [Column("dotnet_and_automation_title")]
        public Dictionary<string, string> DotnetAndAutomationTitle { get; set; }
        [Column("dotnet_and_automation_content")]
        public Dictionary<string, string> DotnetAndAutomationContent { get; set; }
        [Column("python_and_automation_title")]
        public Dictionary<string, string> PythonAndAutomationTitle { get; set; }
        [Column("python_and_automation_content")]
        public Dictionary<string, string> PythonAndAutomationContent { get; set; }
        [Column("angular_and_react_title")]
        public Dictionary<string, string> AngularAndReactTitle { get; set; }
        [Column("angular_and_react_content")]
        public Dictionary<string, string> AngularAndReactContent { get; set; }
        [Column("java_and_spring_title")]
        public Dictionary<string, string> JavaAndSpringTitle { get; set; }
        [Column("java_and_spring_content")]
        public Dictionary<string, string> JavaAndSpringContent { get; set; }
        [Column("invitation_title")]
        public Dictionary<string, string> InvitationTitle { get; set; }
        [Column("invitation_content")]
        public Dictionary<string, string> InvitationContent { get; set; }
        [Column("contact_title")]
        public Dictionary<string, string> ContactTitle { get; set; }
        [Column("contact_content")]
        public Dictionary<string, string> ContactContent { get; set; }
        [Column("mobile_and_multiplatform_title")]
        public Dictionary<string, string> MobileAndMultiplatformTitle { get; set; }
        [Column("mobile_and_multiplatform_content")]
        public Dictionary<string, string> MobileAndMultiplatformContent { get; set; }
        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }
        #endregion

        #region Display Properties
        public string DisplayAboutMeTitle => MDTranslator.GetTranslated(AboutMeTitle);
        public string DisplayAboutMeContent => MDTranslator.GetTranslated(AboutMeContent);
        public string DisplayAboutPortfolioTitle => MDTranslator.GetTranslated(AboutPortfolioTitle);
        public string DisplayAboutPortfolioContent => MDTranslator.GetTranslated(AboutPortfolioContent);
        public string DisplayDeveloperMindsetTitle => MDTranslator.GetTranslated(DeveloperMindsetTitle);
        public string DisplayDeveloperMindsetContent => MDTranslator.GetTranslated(DeveloperMindsetContent);
        public string DisplayTechLeadMindsetTitle => MDTranslator.GetTranslated(TechLeadMindsetTitle);
        public string DisplayTechLeadMindsetContent => MDTranslator.GetTranslated(TechLeadMindsetContent);
        public string DisplayDotnetAndAutomationTitle => MDTranslator.GetTranslated(DotnetAndAutomationTitle);
        public string DisplayDotnetAndAutomationContent => MDTranslator.GetTranslated(DotnetAndAutomationContent);
        public string DisplayPythonAndAutomationTitle => MDTranslator.GetTranslated(PythonAndAutomationTitle);
        public string DisplayPythonAndAutomationContent => MDTranslator.GetTranslated(PythonAndAutomationContent);
        public string DisplayAngularAndReactTitle => MDTranslator.GetTranslated(AngularAndReactTitle);
        public string DisplayAngularAndReactContent => MDTranslator.GetTranslated(AngularAndReactContent);
        public string DisplayJavaAndSpringTitle => MDTranslator.GetTranslated(JavaAndSpringTitle);
        public string DisplayJavaAndSpringContent => MDTranslator.GetTranslated(JavaAndSpringContent);
        public string DisplayInvitationTitle => MDTranslator.GetTranslated(InvitationTitle);
        public string DisplayInvitationContent => MDTranslator.GetTranslated(InvitationContent);
        public string DisplayContactTitle => MDTranslator.GetTranslated(ContactTitle);
        public string DisplayContactContent => MDTranslator.GetTranslated(ContactContent);
        public string DisplayMobileAndMultiplatformTitle => MDTranslator.GetTranslated(MobileAndMultiplatformTitle);
        public string DisplayMobileAndMultiplatformContent => MDTranslator.GetTranslated(MobileAndMultiplatformContent);
        #endregion
    }
}