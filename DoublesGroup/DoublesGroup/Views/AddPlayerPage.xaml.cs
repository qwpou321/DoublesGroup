using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoublesGroup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(PlayerId), nameof(PlayerId))]
    public partial class AddPlayerPage : ContentPage
    {
        public AddPlayerPage()
        {
            InitializeComponent();
            BindingContext = new Player();
        }

        public string PlayerId
        {
            set
            {
                LoadPlayerData(value);
            }
        }

        async void LoadPlayerData(string playerId)
        {
            try
            {
                int id = Convert.ToInt32(playerId);
                // Retrieve the note and set it as the BindingContext of the page.
                Player player = await App.PlayerDatabase.GetPlayerAsync(id);
                BindingContext = player;
                name.Text = player.Name;
                level.Text = player.Level.ToString();
            }
            catch (Exception)
            {
                await Shell.Current.GoToAsync("..");
            }
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(level.Text))
            {
                await DisplayAlert("Alert", "Input is empty", "OK");
                return;
            }

            Player player = (Player)BindingContext;
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

            await App.PlayerDatabase.SavePlayerAsync(player);
            await Shell.Current.GoToAsync("..");

        }
    }
}