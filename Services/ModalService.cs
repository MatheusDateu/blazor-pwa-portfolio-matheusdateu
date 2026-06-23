using BlazorDeploy.Models;

namespace BlazorDeploy.Services
{
    public class ModalService : IModalService
    {
        public event Action<PortfolioProjectModel>? OnShow;
        public event Action? OnClose;
        public void Close()
        {
            OnClose?.Invoke();
        }

        public void Show(PortfolioProjectModel project)
        {
            OnShow?.Invoke(project);
        }
    }
}
