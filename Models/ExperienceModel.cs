using BlazorDeploy.Shared;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace BlazorDeploy.Models
{
    [Table("portf0l10-0007-_experience")]
    public class ExperienceModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }

        [Column("company_name")]
        public string CompanyName { get; set; } = string.Empty;

        [Column("period_start")]
        public DateTime PeriodStart { get; set; }

        [Column("period_end")]
        public DateTime? PeriodEnd { get; set; }

        [Column("is_current")]
        public bool IsCurrent { get; set; }

        [Column("display_order")]
        public int DisplayOrder { get; set; }

        [Column("role")]
        public Dictionary<string, string> RoleDict { get; set; } = new();

        [Column("contributions")]
        public Dictionary<string, List<string>> ContributionsDict { get; set; } = new();

        [Column("personal_notes")]
        public Dictionary<string, string> PersonalNotesDict { get; set; } = new();

        [Column("highlights")]
        public Dictionary<string, List<ExperienceHighlight>> HighlightsDict { get; set; } = new();

        #region Display Properties
        public string DisplayRole => MDTranslator.GetTranslated(RoleDict);
        public string DisplayNotes => MDTranslator.GetTranslated(PersonalNotesDict);

        public List<string> DisplayContributions => GetListTranslated(ContributionsDict);
        public List<ExperienceHighlight> DisplayHighlights => GetListTranslated(HighlightsDict);

        private List<T> GetListTranslated<T>(Dictionary<string, List<T>> dict)
        {
            var culture = System.Globalization.CultureInfo.CurrentUICulture.Name;
            var lang = culture.Split('-')[0];

            if (dict.ContainsKey(culture)) return dict[culture];
            if (dict.ContainsKey(lang)) return dict[lang];
            return dict.Values.FirstOrDefault() ?? new List<T>();
        }
        #endregion
    }

    public class ExperienceHighlight
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }
}