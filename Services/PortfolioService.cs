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

        public async Task<List<ProjectCategoryModel>> GetCategoriesAsync()
        {
            try
            {
                var response = await _supabaseClient
                    .From<ProjectCategoryModel>()
                    .Order(x => x.Id, Supabase.Postgrest.Constants.Ordering.Ascending)
                    .Get();
                return response.Models;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to fetch categories: {ex.Message}");
                return new List<ProjectCategoryModel>();
            }
        }

        public async Task<List<PortfolioProjectModel>> GetProjectsAsync()
        {
            try
            {
                var response = await _supabaseClient
                .From<PortfolioProjectModel>()
                .Order(x => x.Id, Supabase.Postgrest.Constants.Ordering.Ascending)
                .Get();
                return response.Models;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to initialise project list: {ex.Message}");
                return new List<PortfolioProjectModel>();
            }
        }

        public async Task<List<AboutMatheusDelmondesModel>> GetAboutMatheusDelmondesAsync()
        {
            try
            {
                var response = await _supabaseClient
                    .From<AboutMatheusDelmondesModel>()
                    .Order(x => x.Id, Supabase.Postgrest.Constants.Ordering.Ascending)
                    .Get();
                return response.Models;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Failed to fetch about Matheus Delmondes: {ex.Message}");
                return new List<AboutMatheusDelmondesModel>();
            }
        }
    }
}