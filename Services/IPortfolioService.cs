using BlazorDeploy.Models;

namespace BlazorDeploy.Services
{
    /// <summary>
    /// Defines the behaviour for managing portfolio data retrieval.
    /// </summary>
    public interface IPortfolioService
    {
        /// <summary>
        /// Retrieves all projects ordered by creation date (newest first).
        /// </summary>
        Task<List<PortfolioProjectModel>> GetProjectsAsync();

        /// <summary>
        /// Retrieves all available project categories.
        /// </summary>
        Task<List<ProjectCategoryModel>> GetCategoriesAsync();
        
        /// <summary>
        /// Retrieves information about Matheus Delmondes.
        /// </summary>
        /// <returns></returns>
        Task<List<AboutMatheusDelmondesModel>> GetAboutMatheusDelmondesAsync();

        /// <summary>
        /// Retrieves the technology stack categories and their associated technologies.
        /// </summary>
        /// <returns></returns>
        Task<List<TechCategoryModel>> GetTechStackAsync();
    }
}
