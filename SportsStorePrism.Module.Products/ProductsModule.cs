using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using SportsStorePrism.Infrastructure;
using SportsStorePrism.Module.Products.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStorePrism.Module.Products
{
    public class ProductsModule : IModule
    {
        private IUnityContainer _container;
        private IRegionManager _regionManager;

        public ProductsModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType(typeof(object), typeof(ProductsView), "ProductView");

            _regionManager.RequestNavigate(RegionNames.ProductRegion, "ProductView");
        }
    }
}
