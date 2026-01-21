using BlazorDeploy.Models;

namespace BlazorDeploy.Services
{
    public interface IModalService
    {
        public void Show(PortfolioProjectModel project);
        public void Close();
    }
}
