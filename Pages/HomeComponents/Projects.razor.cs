using BlazorDeploy.Models;

namespace BlazorDeploy.Pages.HomeComponents
{
    public partial class Projects
    {
        private HashSet<PortfolioProjectModel> Projetos = new();
        private HashSet<ProjectCategoryModel> Categories = new();
        private Dictionary<string, List<PortfolioProjectModel>> FilteredProjectsByCategory = new();

        private string _searchQuery = string.Empty;
        private string SearchQuery
        {
            get => _searchQuery;
            set { _searchQuery = value; FilterProjects(); }
        }

        private string _selectedCategoryFilter = string.Empty;
        private string SelectedCategoryFilter
        {
            get => _selectedCategoryFilter;
            set { _selectedCategoryFilter = value; FilterProjects(); }
        }

        private bool HasActiveFilters => !string.IsNullOrWhiteSpace(SearchQuery) || !string.IsNullOrWhiteSpace(SelectedCategoryFilter);

        protected override async Task OnInitializedAsync()
        {
            var categoriesTask = PortfolioService.GetCategoriesAsync();
            var projectsTask = PortfolioService.GetProjectsAsync();
            await Task.WhenAll(categoriesTask, projectsTask);

            Categories = await categoriesTask;
            Projetos = await projectsTask;

            await LoadFiltersFromStorage();

            var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
            var qs = ParseQueryString(uri.Query);
            if (qs.TryGetValue("tag", out var tagValues))
            {
                SearchQuery = tagValues.FirstOrDefault() ?? string.Empty;
            }

            FilterProjects();
        }

        private static IReadOnlyDictionary<string, string[]> ParseQueryString(string query)
        {
            if (string.IsNullOrEmpty(query)) return new Dictionary<string, string[]>();
            if (query.StartsWith("?")) query = query[1..];
            var dict = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            foreach (var part in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                var idx = part.IndexOf('=');
                var name = idx >= 0 ? Uri.UnescapeDataString(part.Substring(0, idx)) : Uri.UnescapeDataString(part);
                var value = idx >= 0 ? Uri.UnescapeDataString(part.Substring(idx + 1)) : string.Empty;
                if (!dict.TryGetValue(name, out var list)) { list = new List<string>(); dict[name] = list; }
                list.Add(value);
            }
            return dict.ToDictionary(kv => kv.Key, kv => kv.Value.ToArray());
        }

        private void FilterProjects()
        {
            FilteredProjectsByCategory = new Dictionary<string, List<PortfolioProjectModel>>();

            foreach (var cat in Categories)
            {
                if (!string.IsNullOrEmpty(SelectedCategoryFilter) && cat.Id.ToString() != SelectedCategoryFilter)
                    continue;

                var projectsInCat = Projetos.Where(p => 
                    p.ProjectCategories != null &&
                    p.ProjectCategories.Any(pc => pc.CategoryId == cat.Id)
                ).ToList();

                if (!string.IsNullOrWhiteSpace(SearchQuery))
                {
                    projectsInCat = projectsInCat.Where(p =>
                        p.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                        p.Synopsis.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                        (p.TechStacks != null && p.TechStacks.Any(ts => 
                            ts.Capability != null &&
                            ts.Capability.Description.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)))
                    ).ToList();
                }

                if (projectsInCat.Any())
                {
                    FilteredProjectsByCategory.Add(cat.Name, projectsInCat);
                }
            }
            StateHasChanged();
        }

        #region LocalStorage Management
        private async Task SaveFiltersToStorage()
        {
            await LocalStorage.SetItemAsync("project_filter_search", SearchQuery);
            await LocalStorage.SetItemAsync("project_filter_category", SelectedCategoryFilter);
        }

        private async Task LoadFiltersFromStorage()
        {
            if (await LocalStorage.ContainKeyAsync("project_filter_search"))
                _searchQuery = await LocalStorage.GetItemAsync<string>("project_filter_search") ?? string.Empty;

            if (await LocalStorage.ContainKeyAsync("project_filter_category"))
                _selectedCategoryFilter = await LocalStorage.GetItemAsync<string>("project_filter_category") ?? string.Empty;
        }

        private async Task ClearFilters()
        {
            _searchQuery = string.Empty;
            _selectedCategoryFilter = string.Empty;

            await LocalStorage.RemoveItemAsync("project_filter_search");
            await LocalStorage.RemoveItemAsync("project_filter_category");

            FilterProjects();
        }
        #endregion
    }
}
