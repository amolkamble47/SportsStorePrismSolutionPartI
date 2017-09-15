using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Modularity;
using Microsoft.Practices.Unity;
using Prism.Regions;
using SportsStorePrism.Module.ToolBar.Views;
using SportsStorePrism.Infrastructure;

namespace SportsStorePrism.Module.ToolBar
{
    public class ToolBarModule : IModule
    {
        private IUnityContainer _container;
        private IRegionManager _regionManager;

        public ToolBarModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            var toolbarView = _container.Resolve<ToolBarView>();
            _regionManager.Regions[RegionNames.ToolBarRegion].Add(toolbarView);
        }
    }
}
