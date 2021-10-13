using HostedWpf.ViewModels;

using opal.Views;

using System.Windows.Controls;

namespace opal.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private Page _currentPage;

        public Page CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        public MainWindowViewModel(DashboardPage dashboard)
        {
            _currentPage = dashboard;
        }
    }
}
