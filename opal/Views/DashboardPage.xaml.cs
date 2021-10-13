using HostedWpf.Controls;

using opal.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace opal.Views
{
    public partial class BaseDashboardPage : BasePage<DashboardViewModel>
    {
    }

    public partial class DashboardPage
    {
        public DashboardPage()
        {
            InitializeComponent();
        }

        private void ButtonStartServer_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.StartServer();
        }

        private void ButtonStopServer_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.StopServer();
        }
    }
}
