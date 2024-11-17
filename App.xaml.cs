using System.Configuration;
using System.Data;
using System.Text;
using System.Windows;

namespace SavePDFApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // Register encoding provider to support encoding 1252
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }

}
