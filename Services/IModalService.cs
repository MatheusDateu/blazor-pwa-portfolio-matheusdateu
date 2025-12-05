using BlazorDeploy.Models;

namespace BlazorDeploy.Services
{
    public interface IModalService
    {
        public void Show(PortfolioProject project);
        public void Close();
    }
}
