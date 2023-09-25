using MFAE.Jobs.Core.Dependency;
using MFAE.Jobs.Mobile.MAUI.Services.UI;

namespace MFAE.Jobs.Mobile.MAUI.Shared
{
    public abstract class ModalBase : JobsComponentBase
    {
        protected ModalManagerService ModalManager { get; set; }

        public abstract string ModalId { get; }

        public ModalBase()
        {
            ModalManager = DependencyResolver.Resolve<ModalManagerService>();
        }

        public virtual async Task Show()
        {
            await ModalManager.Show(JS, ModalId);
            StateHasChanged();
        }

        public virtual async Task Hide()
        {
            await ModalManager.Hide(JS, ModalId);
        }
    }
}
