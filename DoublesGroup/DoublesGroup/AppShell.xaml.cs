using DoublesGroup.Views;
using Xamarin.Forms;

namespace DoublesGroup
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddPlayerPage), typeof(AddPlayerPage));
        }

    }
}
