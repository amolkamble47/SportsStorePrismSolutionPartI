using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Mvvm;

namespace SportsStorePrism.Module.StatusBar.ViewModels
{
    public class StatusBarViewModel : BindableBase
    {
        private string _statusBarContent;

        public StatusBarViewModel()
        {
            StatusBarContent = "Message from StatusBar";
        }

        public string StatusBarContent { get => _statusBarContent; set => SetProperty(ref _statusBarContent, value); }
    }
}
