using System.Globalization;
using System.Text;

using BlazorDeploy.Models;

namespace BlazorDeploy.Pages.HomeComponents
{
    /// <summary>
    /// Code-behind component for the projects section, supporting category-first matching, alias expansion, and generic fallback search.
    /// </summary>
    public partial class Projects
    {
        #region Constants and Mappings

        private static readonly Dictionary<string, string[]> TagAliases = new(StringComparer.OrdinalIgnoreCase)
        {
            ["dotnet-desktop"] = [".net", ".net framework", ".net core", "dotnet", "c#", "csharp", "desktop", "wpf", "windows forms", "maui"],
            ["dotnet-backend"] = [".net", ".net framework", ".net core", "dotnet", "c#", "csharp", "backend", "asp.net", "asp.net core", "minimal apis", "ef core", "entity framework", "dapper", "docker"],
            ["dotnet-mobile"] = [".net", ".net core", "dotnet", "c#", "csharp", "maui", "blazor", "hybrid", "multiplataforma"],
            ["dotnet-frontend"] = [".net", ".net core", "dotnet", "c#", "csharp", "frontend", "blazor", "blazor wasm", "blazor server", "blazor app", "asp.net mvc", "mudblazor"],
            ["dotnet-rpa"] = [".net", ".net framework", ".net core", "dotnet", "c#", "csharp", "rpa", "automacao", "automacao de tarefas", "uipath", "selenium", "ef core", "dapper", "windows forms"],
            ["ts-frontend"] = ["typescript", "ts", "javascript", "frontend", "angular", "react", "nginx"],
            ["python-frontend"] = ["python", "frontend", "django", "flask"],
            ["python-backend-ai"] = ["python", "backend", "flask", "django", "ai", "ia", "inteligencia artificial", "rag", "llm", "chatbot", "chromadb", "pgvector", "mcp"],
            ["java-desktop"] = ["java", "desktop", "swing", "easyfood"],
            ["java-backend"] = ["java", "backend", "spring", "springboot", "hibernate"],
            ["java-android"] = ["java", "android"],
            ["kotlin-android"] = ["kotlin", "android", "supabase"],
            ["flutter"] = ["flutter", "dart"],
            ["db-sql"] = ["sql", "relacional", "banco de dados", "bancos de dados", "database", "postgresql", "postgres", "mysql", "sql server", "sqlite", "pgvector"],
            ["db-nosql"] = ["nosql", "cache", "banco de dados", "bancos de dados", "database", "redis", "chromadb", "firebase"]
        };

        #endregion Constants and Mappings

        #region Fields and Properties

        private HashSet<PortfolioProjectModel> Projetos { get; set; } = new();
        private HashSet<ProjectCategoryModel> Categories { get; set; } = new();
        private Dictionary<string, List<PortfolioProjectModel>> FilteredProjectsByCategory { get; set; } = new();

        private string _searchQuery = string.Empty;
        private string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                FilterProjects();
            }
        }

        private string _selectedCategoryFilter = string.Empty;
        private string SelectedCategoryFilter
        {
            get => _selectedCategoryFilter;
            set
            {
                _selectedCategoryFilter = value;
                FilterProjects();
            }
        }

        private bool HasActiveFilters =>
            !string.IsNullOrWhiteSpace(SearchQuery) || !string.IsNullOrWhiteSpace(SelectedCategoryFilter);

        #endregion Fields and Properties

        #region Lifecycle Methods

        /// <summary>
        /// Initialises the component, loads categories and projects, restores saved filters, and applies any query string tag.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            Task<HashSet<ProjectCategoryModel>> categoriesTask = PortfolioService.GetCategoriesAsync();
            Task<HashSet<PortfolioProjectModel>> projectsTask = PortfolioService.GetProjectsAsync();
            await Task.WhenAll(categoriesTask, projectsTask);

            Categories = categoriesTask.Result ?? new HashSet<ProjectCategoryModel>();
            Projetos = projectsTask.Result ?? new HashSet<PortfolioProjectModel>();

            await LoadFiltersFromStorage();

            Uri uri = NavManager.ToAbsoluteUri(NavManager.Uri);
            IReadOnlyDictionary<string, string[]> queryString = ParseQueryString(uri.Query);
            if (queryString.TryGetValue("tag", out string[]? tagValues))
            {
                string tagValue = tagValues.FirstOrDefault() ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(tagValue))
                {
                    ApplyTagFilter(tagValue.Trim());
                    await SaveFiltersToStorage();
                }
            }

            FilterProjects();
        }

        #endregion Lifecycle Methods

        #region Filtering and Matching

        /// <summary>
        /// Applies a tag filter by attempting a category match first and falling back to generic search with expanded terms.
        /// </summary>
        /// <param name="tag">The raw tag value or search slug.</param>
        private void ApplyTagFilter(string tag)
        {
            HashSet<string> candidateTerms = ExpandSearchTerms(tag);
            ProjectCategoryModel? matchedCategory = FindMatchingCategory(candidateTerms, Categories);

            if (matchedCategory != null)
            {
                _selectedCategoryFilter = matchedCategory.Id.ToString(CultureInfo.InvariantCulture);
                _searchQuery = string.Empty;
                return;
            }

            _selectedCategoryFilter = string.Empty;
            _searchQuery = tag;
        }

        /// <summary>
        /// Filters projects by selected category and, when a search query is present, across project fields, categories, and technology stack.
        /// </summary>
        private void FilterProjects()
        {
            FilteredProjectsByCategory = new Dictionary<string, List<PortfolioProjectModel>>();
            HashSet<string> searchTerms = !string.IsNullOrWhiteSpace(SearchQuery)
                ? ExpandSearchTerms(SearchQuery)
                : new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (ProjectCategoryModel category in Categories)
            {
                if (!string.IsNullOrEmpty(SelectedCategoryFilter) &&
                    category.Id.ToString(CultureInfo.InvariantCulture) != SelectedCategoryFilter)
                {
                    continue;
                }

                List<PortfolioProjectModel> projectsInCategory = Projetos
                    .Where(project =>
                        project.ProjectCategories != null &&
                        project.ProjectCategories.Any(pc => pc.CategoryId == category.Id))
                    .ToList();

                if (searchTerms.Count > 0)
                {
                    projectsInCategory = projectsInCategory
                        .Where(project => MatchesGenericSearch(project, searchTerms))
                        .ToList();
                }

                if (projectsInCategory.Count > 0)
                {
                    FilteredProjectsByCategory.Add(category.Name, projectsInCategory);
                }
            }

            StateHasChanged();
        }

        /// <summary>
        /// Expands a tag or query into normalised search terms including configured aliases and separator variants.
        /// </summary>
        /// <param name="input">The raw search query or slug.</param>
        /// <returns>A set of accent-stripped search terms.</returns>
        private static HashSet<string> ExpandSearchTerms(string input)
        {
            HashSet<string> terms = new(StringComparer.OrdinalIgnoreCase);
            string trimmed = input.Trim();

            if (string.IsNullOrWhiteSpace(trimmed))
            {
                return terms;
            }

            AddNormalisedVariant(terms, trimmed);

            if (TagAliases.TryGetValue(trimmed, out string[]? aliases))
            {
                foreach (string alias in aliases)
                {
                    AddNormalisedVariant(terms, alias);
                }
            }

            return terms;
        }

        /// <summary>
        /// Adds both the diacritic-free term and its space-separated variant to the term set.
        /// </summary>
        /// <param name="terms">The target collection of search terms.</param>
        /// <param name="rawTerm">The term to normalise and add.</param>
        private static void AddNormalisedVariant(HashSet<string> terms, string rawTerm)
        {
            string normalised = RemoveDiacritics(rawTerm.Trim());
            if (!string.IsNullOrWhiteSpace(normalised))
            {
                terms.Add(normalised);
                string spaced = normalised.Replace('-', ' ').Replace('_', ' ').Trim();
                if (!string.IsNullOrWhiteSpace(spaced))
                {
                    terms.Add(spaced);
                }
            }
        }

        /// <summary>
        /// Finds the first category matching any of the expanded search terms.
        /// </summary>
        /// <param name="terms">The normalised search terms.</param>
        /// <param name="categories">Available project categories.</param>
        /// <returns>The matching <see cref="ProjectCategoryModel"/> if found; otherwise, <c>null</c>.</returns>
        private static ProjectCategoryModel? FindMatchingCategory(
            HashSet<string> terms,
            IEnumerable<ProjectCategoryModel>? categories)
        {
            if (terms.Count == 0 || categories == null)
            {
                return null;
            }

            return categories.FirstOrDefault(category =>
                !string.IsNullOrWhiteSpace(category.Name) &&
                ContainsAnyTerm(category.Name, terms));
        }

        /// <summary>
        /// Evaluates whether a project matches any expanded search term across its name, synopsis, description, tech stack, and categories.
        /// </summary>
        /// <param name="project">The project model to check.</param>
        /// <param name="terms">The set of normalised search terms.</param>
        /// <returns><c>true</c> if any project field matches; otherwise, <c>false</c>.</returns>
        private static bool MatchesGenericSearch(PortfolioProjectModel project, HashSet<string> terms)
        {
            bool matchesBasicFields =
                ContainsAnyTerm(project.Name, terms) ||
                ContainsAnyTerm(project.Synopsis, terms) ||
                ContainsAnyTerm(project.Description, terms);

            if (matchesBasicFields)
            {
                return true;
            }

            bool matchesTechStack = project.TechStacks != null &&
                project.TechStacks.Any(ts =>
                    ts.Capability != null &&
                    ContainsAnyTerm(ts.Capability.Description, terms));

            if (matchesTechStack)
            {
                return true;
            }

            bool matchesCategories = project.ProjectCategories != null &&
                project.ProjectCategories.Any(pc =>
                    pc.Category != null &&
                    ContainsAnyTerm(pc.Category.Name, terms));

            return matchesCategories;
        }

        /// <summary>
        /// Checks whether a candidate string contains at least one normalised search term using case-insensitive comparison.
        /// </summary>
        /// <param name="source">The source text to inspect.</param>
        /// <param name="terms">The set of normalised search terms.</param>
        /// <returns><c>true</c> if any term is contained within the normalised source; otherwise, <c>false</c>.</returns>
        private static bool ContainsAnyTerm(string? source, HashSet<string> terms)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return false;
            }

            string normalisedSource = RemoveDiacritics(source);

            foreach (string term in terms)
            {
                if (normalisedSource.Contains(term, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Strips diacritical marks from a string for accent-insensitive comparisons.
        /// </summary>
        /// <param name="text">The input string.</param>
        /// <returns>The text without accent marks.</returns>
        private static string RemoveDiacritics(string text)
        {
            string decomposed = text.Normalize(NormalizationForm.FormD);
            StringBuilder builder = new(decomposed.Length);

            foreach (char character in decomposed)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(character);
                if (category != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(character);
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Parses a query string into a case-insensitive dictionary of parameter keys and decoded values.
        /// </summary>
        /// <param name="query">The raw query string starting with or without '?'.</param>
        /// <returns>A read-only dictionary of parsed query parameters.</returns>
        private static IReadOnlyDictionary<string, string[]> ParseQueryString(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return new Dictionary<string, string[]>();
            }

            if (query.StartsWith("?", StringComparison.Ordinal))
            {
                query = query[1..];
            }

            Dictionary<string, List<string>> dictionary = new(StringComparer.OrdinalIgnoreCase);

            foreach (string part in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                int separatorIndex = part.IndexOf('=');
                string name = separatorIndex >= 0
                    ? Uri.UnescapeDataString(part[..separatorIndex])
                    : Uri.UnescapeDataString(part);
                string value = separatorIndex >= 0
                    ? Uri.UnescapeDataString(part[(separatorIndex + 1)..])
                    : string.Empty;

                if (!dictionary.TryGetValue(name, out List<string>? values))
                {
                    values = new List<string>();
                    dictionary[name] = values;
                }

                values.Add(value);
            }

            return dictionary.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToArray());
        }

        #endregion Filtering and Matching

        #region LocalStorage Management

        /// <summary>
        /// Persists the active search query and category filter to local storage.
        /// </summary>
        private async Task SaveFiltersToStorage()
        {
            await LocalStorage.SetItemAsync("project_filter_search", SearchQuery);
            await LocalStorage.SetItemAsync("project_filter_category", SelectedCategoryFilter);
        }

        /// <summary>
        /// Loads previously saved search and category filters from local storage.
        /// </summary>
        private async Task LoadFiltersFromStorage()
        {
            if (await LocalStorage.ContainKeyAsync("project_filter_search"))
            {
                _searchQuery = await LocalStorage.GetItemAsync<string>("project_filter_search") ?? string.Empty;
            }

            if (await LocalStorage.ContainKeyAsync("project_filter_category"))
            {
                _selectedCategoryFilter = await LocalStorage.GetItemAsync<string>("project_filter_category") ?? string.Empty;
            }
        }

        /// <summary>
        /// Clears all active filters, removes them from local storage, and refreshes the displayed projects.
        /// </summary>
        private async Task ClearFilters()
        {
            _searchQuery = string.Empty;
            _selectedCategoryFilter = string.Empty;

            await LocalStorage.RemoveItemAsync("project_filter_search");
            await LocalStorage.RemoveItemAsync("project_filter_category");

            FilterProjects();
        }

        #endregion LocalStorage Management
    }
}