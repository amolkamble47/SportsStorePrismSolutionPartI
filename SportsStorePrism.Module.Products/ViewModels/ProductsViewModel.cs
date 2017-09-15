using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Prism.Mvvm;
using SportsStorePrism.Infrastructure.Abstract;
using SportsStorePrism.Infrastructure.Entities;
using Prism.Commands;
using Prism.Events;
using SportsStorePrism.Infrastructure;
using Prism.Interactivity.InteractionRequest;
using System.Windows.Input;
using Prism.Regions;

namespace SportsStorePrism.Module.Products.ViewModels
{
    public class ProductViewModel : BindableBase, INavigationAware
    {
        private IEventAggregator _eventAggregator;
        private IRegionManager _regionManager;
        private IProductRepository _productRepository;
        private ObservableCollection<Product> _products;
        private List<Product> _allProducts;
        private string _displayMessage;
        private bool _messageFlag;
        private string _searchInput;
        public ProductViewModel(IProductRepository productRepository, IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            _productRepository = productRepository;

            EditProductCommand = new DelegateCommand<Product>(OnEdit);
            DeleteProductCommand = new DelegateCommand<Product>(OnDelete);

            NotificationRequest = new InteractionRequest<INotification>();
            //NotificationCommand = new DelegateCommand(RaiseNotification);
            NotificationCommand = new DelegateCommand<Product>(OnDelete);

            ConfirmationRequest = new InteractionRequest<IConfirmation>();
            //ConfiramtionCommand = new DelegateCommand(RaiseComfirmation);
            ConfiramtionCommand = new DelegateCommand<Product>(OnDelete);

            _allProducts = _productRepository.GetProductsAsync().Result;//Using Result to get the data in a ctor even when the method return Async
            GetProducts();
        }

        public ObservableCollection<Product> Products { get => _products; set => SetProperty(ref _products, value); }
        private async Task GetProducts(string currentCategory = null)
        {
            Products = new ObservableCollection<Product>(currentCategory == null || currentCategory == "Home" ? _allProducts : _allProducts.Where(c => c.Category == currentCategory));
            //Products = new ObservableCollection<Product>(currentCategory == null || currentCategory == "Home" ? await _productRepository.GetProductsAsync() : await _productRepository.GetProductsByCategoryAsync(currentCategory) );
        }

        public string DisplayMessage { get => _displayMessage; set => SetProperty(ref _displayMessage, value); }
        public bool MessageFlag { get => _messageFlag; set => SetProperty(ref _messageFlag, value); }

        #region Edit and Delete

        public InteractionRequest<INotification> NotificationRequest { get; set; }
        public ICommand NotificationCommand { get; set; }
        void RaiseNotification()
        {
            //NotificationRequest.Raise(new Notification { Title = "Notification", Content = "Notification Message" }, (result) => _eventAggregator.GetEvent<ProductsMessagingEvent>().Publish("Notified"));
        }

        public InteractionRequest<IConfirmation> ConfirmationRequest { get; set; }
        public ICommand ConfiramtionCommand { get; set; }
        void RaiseComfirmation()
        {
            ConfirmationRequest.Raise(new Confirmation() { Title = "Confiramation", Content = "Confiramation Message" }, (result) => DisplayMessage = result.Confirmed ? "Confiramed" : "Not Comdiramed");
        }


        public DelegateCommand<Product> EditProductCommand { get; set; }

        public event Action<Product> EditProductRequested = delegate { };
        public void OnEdit(Product product)
        {
            //Using Prism INavigationAware 
            if (product != null)
            {
                var parameters = new NavigationParameters();
                parameters.Add("prod", product);
                parameters.Add("editFlag", bool.TrueString);
                //The one given in the ProductModule
                _regionManager.RequestNavigate(RegionNames.ProductRegion, new Uri("AddEditProductView", UriKind.Relative), parameters);
            }

            //EditProductRequested(product);
        }
        public DelegateCommand<Product> DeleteProductCommand { get; set; }
        public async void OnDelete(Product product)
        {
            //await _productRepository.DeleteProductAsync(product.ProductId);

            #region With Confirmation Popup
            string question = string.Format($"Do you wish to delete\nProduct: {product.ProductName}, with the Id: {product.ProductId}");
            string message = string.Format($"Product: {product.ProductName}, with the Id: {product.ProductId}, Deleted Successfully");
            ConfirmationRequest.Raise(new Confirmation { Title = "Delete Product", Content = question },
                (result) =>
                {
                    if (result.Confirmed)
                    {
                        _productRepository.DeleteProductAsync(product.ProductId);
                        //await _productRepository.DeleteProductAsync(product.ProductId);
                        //_eventAggregator.GetEvent<ProductsMessagingEvent>().Publish(message);
                    }
                    else
                    {
                        //_eventAggregator.GetEvent<ProductsMessagingEvent>().Publish("Did not delete");
                    }
                }
                );
            _allProducts = _productRepository.GetProductsAsync().Result;
            GetProducts();

            #endregion

            #region With Notification Box MessageBox
            //string message = string.Format($"Product: {product.ProductName}, with the Id: {product.ProductId}, Deleted Successfully");
            //NotificationRequest.Raise(new Notification { Title = "Delete Product", Content = message }, (result) => _eventAggregator.GetEvent<ProductsMessagingEvent>().Publish(message));
            //GetProducts(); 
            #endregion

            #region First only in the Status Bar
            //string message = string.Format($"Product: {product.ProductName}, with the Id: {product.ProductId}, Deleted Successfully");
            //_eventAggregator.GetEvent<ProductsMessagingEvent>().Publish(message);
            //GetProducts();

            #endregion
        }

        #endregion

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.Count() != 0)
            {
                var editFlag = navigationContext.Parameters["editFlag"].ToString();
                if (editFlag != "true")
                {
                    _allProducts = _productRepository.GetProductsAsync().Result;
                }
                GetProducts();
            }

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }


        #endregion

        #region Search Code
        public DelegateCommand ClearSearchCommand { get; private set; }
        private void OnClearSearch() { SearchInput = null; }
        private void SearchProducts(string searchInput)
        {
            Products = new ObservableCollection<Product>(string.IsNullOrWhiteSpace(searchInput) ? _allProducts : _allProducts.Where(c => c.ProductName.ToLower().Contains(searchInput.ToLower())));
        }
        public string SearchInput
        {
            get => _searchInput;
            set
            {
                SetProperty(ref _searchInput, value);
                SearchProducts(_searchInput);
            }
        }

        #endregion

        #region Categories Code
        //private ObservableCollection<string> _categories;
        //private string _currentCategory;
        //public ObservableCollection<string> Categories { get => _categories; set => SetProperty(ref _categories, value); }
        //public string CurrentCategory { get => _currentCategory; set => SetProperty(ref _currentCategory, value); }
        //private async Task GetCategories()
        //{
        //    var result = await _productRepository.GetProductsAsync();
        //    Categories = new ObservableCollection<string>(result.Select(c => c.Category).Distinct().OrderBy(c => c));
        //    Categories.Insert(0, "Home");
        //} 
        #endregion


    }
}
