using HostedWpf.Windows;

using opal.ViewModels;

namespace opal.Views
{
    public partial class BaseMainWindow : BaseWindow<MainWindowViewModel>
    {
    }

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
