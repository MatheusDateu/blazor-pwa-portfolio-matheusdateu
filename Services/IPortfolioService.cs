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
        Task<List<PortfolioProject>> GetProjectsAsync();

        /// <summary>
        /// Retrieves all available project categories.
        /// </summary>
        Task<List<ProjectCategory>> GetCategoriesAsync();
    }
}
