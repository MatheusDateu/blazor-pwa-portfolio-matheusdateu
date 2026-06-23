using System.Globalization;

namespace BlazorDeploy.Shared
{
    internal static class MDTranslator
    {
        #region Methods
        public static string GetTranslated(Dictionary<string, string> field)
        {
            if (field == null) return string.Empty;

            var culture = CultureInfo.CurrentCulture.Name;

            var lang = culture.Split('-')[0];

            if (field.ContainsKey(culture)) return field[culture];
            if (field.ContainsKey(lang)) return field[lang];

            return field.Values.FirstOrDefault() ?? string.Empty;
        }
        #endregion
    }
}
