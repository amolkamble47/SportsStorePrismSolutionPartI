using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Modularity;
using Prism.Unity;
using SportsStorePrism.Module.ToolBar;
using SportsStorePrism.Module.StatusBar;
using SportsStorePrism.Module.Products;
using SportsStorePrism.Module.Services;

namespace SportsStorePrismWpfApp
{
    public class Bootstrapper: UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.TryResolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            App.Current.MainWindow = Shell as Window;
            App.Current.MainWindow.Show();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            ModuleCatalog moduleCatalog = new ModuleCatalog();
            moduleCatalog.AddModule(typeof(ToolBarModule));
            moduleCatalog.AddModule(typeof(ServicesModule));
            moduleCatalog.AddModule(typeof(StatusBarModule));
            moduleCatalog.AddModule(typeof(ProductsModule));
            return moduleCatalog;
        }
    }
}
