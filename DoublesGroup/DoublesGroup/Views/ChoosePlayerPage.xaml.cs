using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoublesGroup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChoosePlayerPage : ContentPage
    {
        Player m_player;
        ViewCell m_lastCell;

        public ChoosePlayerPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listView.ItemsSource = await App.PlayerDatabase.GetPlayerAsync();
        }

        async void OnAddClicked(object sender, EventArgs e)
        {
            // Navigate to the NoteEntryPage.
            await Shell.Current.GoToAsync(nameof(AddPlayerPage));
        }

        async void OnDeleteClicked(object sender, EventArgs e)
        {
            // Navigate to the NoteEntryPage.
            if (m_player != null)
            {
                await App.PlayerDatabase.DeletePlayerAsync(m_player);
                listView.ItemsSource = await App.PlayerDatabase.GetPlayerAsync();
            }
        }

        async void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            Player player = (Player)checkBox.BindingContext;
            if (player == null) return;
            await App.PlayerDatabase.SavePersonAsync(player);
        }

        void OnViewCellTapped(object sender, System.EventArgs e)
        {
            if (m_lastCell != null)
                m_lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.LightSkyBlue;
                m_lastCell = viewCell;
            }
        }

        async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (m_player == e.Item as Player) 
            {
                await Shell.Current.GoToAsync($"{nameof(AddPlayerPage)}?{nameof(AddPlayerPage.PlayerId)}={m_player.Id.ToString()}");
            }
            m_player = e.Item as Player;
        }
    }
}