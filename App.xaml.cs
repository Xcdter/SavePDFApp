using System.Text;
using System.Windows;

namespace SavePDFApp
{
    public partial class App : Application
    {
        public App()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }

}
