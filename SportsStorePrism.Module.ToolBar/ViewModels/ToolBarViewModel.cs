using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SportsStorePrism.Infrastructure;

namespace SportsStorePrism.Module.ToolBar.ViewModels
{
    public class ToolBarViewModel : BindableBase
    {
        private IRegionManager _regionManager;
        public ToolBarViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            NavigateToAddEditProductViewCommand = new DelegateCommand(OnNavigateToAddEditProductView);
        }

        private void OnNavigateToAddEditProductView()
        {
            var parameters = new NavigationParameters();
            parameters.Add("editFlag", bool.FalseString);
            _regionManager.RequestNavigate(RegionNames.ProductRegion, "AddEditProductView", parameters);
        }

        public DelegateCommand NavigateToAddEditProductViewCommand { get; set; }

    }
}
