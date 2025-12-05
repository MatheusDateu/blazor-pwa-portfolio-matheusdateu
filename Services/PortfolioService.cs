using BlazorDeploy.Models;

namespace BlazorDeploy.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly Supabase.Client _supabaseClient;

        /// <summary>
        /// Supabase Client Injection
        /// </summary>
        /// <param name="supabaseClient"></param>
        public PortfolioService(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<List<ProjectCategory>> GetCategoriesAsync()
        {
            try
            {
                var response = await _supabaseClient
                    .From<ProjectCategory>()
                    .Order(x => x.Id, Supabase.Postgrest.Constants.Ordering.Ascending)
                    .Get();
                return response.Models;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to fetch categories: {ex.Message}");
                return new List<ProjectCategory>();
            }
        }

        public async Task<List<PortfolioProject>> GetProjectsAsync()
        {
            try
            {
                var response = await _supabaseClient
                .From<PortfolioProject>()
                .Order(x => x.Id, Supabase.Postgrest.Constants.Ordering.Ascending)
                .Get();
                return response.Models;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to initialise project list: {ex.Message}");
                return new List<PortfolioProject>();
            }
        }
    }
}
