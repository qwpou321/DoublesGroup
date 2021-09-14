using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoublesGroup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPlayerPage : ContentPage
    {
        public AddPlayerPage()
        {
            InitializeComponent();
        }

        async void OnAddButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(level.Text))
            {
                await DisplayAlert("Alert", "Input is empty", "OK");
                return;
            }

            Player player = new Player();
            player.Name = name.Text;
            try
            {
                player.Level = Convert.ToInt32(level.Text);
            }
            catch
            {
                await DisplayAlert("Alert", "Level should be integer", "OK");
                return;
            }

            if (player.Level > 4 || player.Level < 1)
            {
                await DisplayAlert("Alert", "Level should be between 1 and 4", "OK");
                return;
            }

            await App.PlayerDatabase.SavePersonAsync(player);
            await Shell.Current.GoToAsync("..");

        }
    }
}