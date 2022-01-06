using Caliburn.Micro;
using System.Windows;

namespace DictionaryRenderGraphicOverlay.ViewModels
{
    public class ShellViewModel : PropertyChangedBase
    {
        private GraphicOverlayMapViewModel _mapViewModel = new();

        public ShellViewModel()
        {
        }

        public GraphicOverlayMapViewModel MapViewModel
        {
            get => _mapViewModel;
            set => Set(ref _mapViewModel, value);
        }
    }
}