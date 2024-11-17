using SavePDFApp.ViewModels;
using System.Windows;

namespace SavePDFApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
    }
}