using BlazorDeploy.Models;

namespace BlazorDeploy.Services
{
    public class ModalService : IModalService
    {
        public event Action<PortfolioProject>? OnShow;
        public event Action? OnClose;
        public void Close()
        {
            OnClose?.Invoke();
        }

        public void Show(PortfolioProject project)
        {
            OnShow?.Invoke(project);
        }
    }
}
