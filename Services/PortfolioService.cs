using BlazorDeploy.Models;
using Blazored.LocalStorage;
using Newtonsoft.Json;

namespace BlazorDeploy.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly Supabase.Client _supabaseClient;
        public readonly ILocalStorageService _localStorage;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

        /// <summary>
        /// Injections
        /// </summary>
        /// <param name="supabaseClient">Database service</param>
        /// <param name="localStorageService">Cache service</param>
        public PortfolioService(Supabase.Client supabaseClient, ILocalStorageService localStorageService)
        {
            _supabaseClient = supabaseClient;
            _localStorage = localStorageService;
        }

        /// <summary>
        /// Wrapper to hold cached data and its expiration date
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public record CachedRecord<T>
        {
            public T Data { get; set; } = default!;
            public DateTime Expiration { get; set; }
        }

        /// <summary>
        /// Generic helper method to manage caching logic for any model type.
        /// </summary>
        private async Task<HashSet<T>> GetFromCacheOrFetchAsync<T>(string cacheKey, Func<Task<HashSet<T>>> fetchAction)
        {
            try
            {
                var jsonStr = await _localStorage.GetItemAsStringAsync(cacheKey);

                if (!string.IsNullOrWhiteSpace(jsonStr))
                {
                    var cachedItem = JsonConvert.DeserializeObject<CachedRecord<HashSet<T>>>(jsonStr);
                    if (cachedItem is not null && cachedItem.Expiration > DateTime.UtcNow)
                    {
                        return cachedItem.Data;
                    }
                }
            } catch(Exception ex)
            {
                Console.WriteLine($"[Cache Read Error] {ex.Message}");
            }

            var data = await fetchAction();

            if (data is not null && data.Any())
            {
                try
                {
                    var newCache = new CachedRecord<HashSet<T>>
                    {
                        Data = data,
                        Expiration = DateTime.UtcNow.Add(_cacheDuration)
                    };

                    var jsonToSave = JsonConvert.SerializeObject(newCache);
                    await _localStorage.SetItemAsStringAsync(cacheKey, jsonToSave);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Cache Write Error] {ex.Message}");
                }
            }

            return data ?? new HashSet<T>();
        }

        public Task<HashSet<ProjectCategoryModel>> GetCategoriesAsync()
        {
            return GetFromCacheOrFetchAsync("categories_cache", async () =>
            {
                try
                {
                    var response = await _supabaseClient
                        .From<ProjectCategoryModel>()
                        .Order(x => x.Id, Supabase.Postgrest.Constants.Ordering.Ascending)
                        .Get();
                    return response.Models.ToHashSet();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] Failed to fetch categories: {ex.Message}");
                    return new HashSet<ProjectCategoryModel>();
                }
            });
        }

        public Task<HashSet<PortfolioProjectModel>> GetProjectsAsync()
        {
            return GetFromCacheOrFetchAsync("projects_cache", async () =>
            {
                try
                {
                    var response = await _supabaseClient
                    .From<PortfolioProjectModel>()
                    .Order(x => x.Id, Supabase.Postgrest.Constants.Ordering.Ascending)
                    .Get();
                    return response.Models.ToHashSet();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] Failed to fetch project list: {ex.Message}");
                    return new HashSet<PortfolioProjectModel>();
                }
            });
        }

        public Task<HashSet<AboutMatheusDelmondesModel>> GetAboutMatheusDelmondesAsync()
        {
            return GetFromCacheOrFetchAsync("about_cache", async () =>
            {
                try
                {
                    var response = await _supabaseClient
                        .From<AboutMatheusDelmondesModel>()
                        .Get();
                    return response.Models.ToHashSet();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] Failed to fetch about Matheus Delmondes: {ex.Message}");
                    return new HashSet<AboutMatheusDelmondesModel>();
                }
            });
        }

        public Task<HashSet<TechCategoryModel>> GetTechStackAsync()
        {
            return GetFromCacheOrFetchAsync("tech_stack_cache", async () =>
            {
                try
                {
                    var response = await _supabaseClient
                        .From<TechCategoryModel>()
                        .Order("display_order", Supabase.Postgrest.Constants.Ordering.Ascending)
                        .Get();
                    return response.Models.Where(x => x.IsActive).ToHashSet();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] Failed to fetch Tech Stack: {ex.Message}");
                    return new HashSet<TechCategoryModel>();
                }
            });
        }

        public Task<HashSet<ExperienceModel>> GetExperiencesAsync()
        {
            return GetFromCacheOrFetchAsync("experiences_cache", async () =>
            {
                try
                {
                    var response = await _supabaseClient
                        .From<ExperienceModel>()
                        .Order("display_order", Supabase.Postgrest.Constants.Ordering.Ascending)
                        .Get();

                    return response.Models.ToHashSet();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] Failed to fetch experiences: {ex.Message}");
                    return new HashSet<ExperienceModel>();
                }
            });
        }
    }
}