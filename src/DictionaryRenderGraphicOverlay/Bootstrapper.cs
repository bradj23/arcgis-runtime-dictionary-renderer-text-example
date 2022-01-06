using System.Collections.Generic;
using Caliburn.Micro;
using DictionaryRenderGraphicOverlay.ViewModels;
using System.Windows;

namespace DictionaryRenderGraphicOverlay
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var windowSettings = new Dictionary<string, object>
            {
                {"SizeToContent" , SizeToContent.Manual},
                { "Width", 1300 },
                { "Height", 900 },
            };

            DisplayRootViewFor<ShellViewModel>(windowSettings);

        }
    }
}