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
        Task<HashSet<PortfolioProjectModel>> GetProjectsAsync();

        /// <summary>
        /// Retrieves all available project categories.
        /// </summary>
        Task<HashSet<ProjectCategoryModel>> GetCategoriesAsync();
        
        /// <summary>
        /// Retrieves information about Matheus Delmondes.
        /// </summary>
        /// <returns></returns>
        Task<HashSet<AboutMatheusDelmondesModel>> GetAboutMatheusDelmondesAsync();

        /// <summary>
        /// Retrieves the technology stack categories and their associated technologies.
        /// </summary>
        /// <returns></returns>
        Task<HashSet<TechCategoryModel>> GetTechStackAsync();

        /// <summary>
        /// Retrives the experiences of Matheus Delmondes, ordered by period start date (newest first).
        /// </summary>
        /// <returns></returns>
        Task<HashSet<ExperienceModel>> GetExperiencesAsync();
    }
}
