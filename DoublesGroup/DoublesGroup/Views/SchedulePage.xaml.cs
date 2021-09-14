using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DoublesGroup.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchedulePage : ContentPage
    {
        Divider m_divider;
        ViewCell m_lastCell;
        public SchedulePage()
        {
            InitializeComponent();
            m_divider = new Divider();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            listView.ItemsSource = await App.ScheduleTxt.ReadTextFileToList();
        }

        async void OnDivideClicked(object sender, EventArgs e)
        {
            List<Player> chosenPlayers = await App.PlayerDatabase.GetChosenPlayers();

            if (chosenPlayers.Count < 5)
            {
                await DisplayAlert("Alert", "Chosen player less than 5. Go to Choose Player Page to choose more player.", "OK");
                return;
            }

            List<string> schedule = m_divider.DividePlayers(chosenPlayers);
            listView.ItemsSource = schedule;

            await App.ScheduleTxt.WriteListToTextFile(schedule);
        }

        private void OnViewCellTapped(object sender, System.EventArgs e)
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
    }
}