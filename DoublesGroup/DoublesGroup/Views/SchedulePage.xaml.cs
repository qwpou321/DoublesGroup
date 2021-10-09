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
        bool m_canCheckBoxEventTrigger = false;
        public SchedulePage()
        {
            InitializeComponent();
            m_divider = new Divider();
        }

        protected async override void OnAppearing()
        {
            m_canCheckBoxEventTrigger = false;
            base.OnAppearing();
            listView.ItemsSource = await App.ScheduleDatabase.GetPlayersAsync();
            m_canCheckBoxEventTrigger = true;
        }

        async void OnDivideClicked(object sender, EventArgs e)
        {
            List<Player> chosenPlayers = await App.PlayerDatabase.GetChosenPlayers();

            if (chosenPlayers.Count < 5)
            {
                await DisplayAlert("Alert", "Chosen players less than 5. Go to Choose Player Page to choose more players.", "OK");
                return;
            }

            List<Schedule> scheduleHasPlayed = new List<Schedule>();
            List<Schedule> allSchedules = await App.ScheduleDatabase.GetPlayersAsync();
            for (int i = 0; i < allSchedules.Count; i++)
            {
                if (allSchedules[i].Played == false)
                {
                    break;
                }
                scheduleHasPlayed.Add(allSchedules[i]);
            }

            List<Schedule> newAllSchedules = m_divider.DividePlayers(chosenPlayers, scheduleHasPlayed);

            await App.ScheduleDatabase.UpdateSchedue(newAllSchedules);
            listView.ItemsSource = newAllSchedules;
        }

        void OnViewCellTapped(object sender, EventArgs e)
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

        async void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (m_canCheckBoxEventTrigger == false) return;
            CheckBox checkBox = (CheckBox)sender;
            Schedule schedule = (Schedule)checkBox.BindingContext;
            if (schedule == null) return;

            m_canCheckBoxEventTrigger = false;
            List<Schedule> allSchedules = await App.ScheduleDatabase.GetPlayersAsync();

            if (schedule.Played)
            {
                for (int i = 0; i < allSchedules.Count; i++)
                {
                    allSchedules[i].Played = true;
                    if (allSchedules[i].Id == schedule.Id)
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int i = allSchedules.Count - 1; i >= 0; i--)
                {
                    allSchedules[i].Played = false;
                    if (allSchedules[i].Id == schedule.Id)
                    {
                        break;
                    }
                }
            }
            listView.ItemsSource = allSchedules;
            await App.ScheduleDatabase.UpdateSchedue(allSchedules);
            m_canCheckBoxEventTrigger = true;
        }

        async void OnUnselectClicked(object sender, EventArgs e)
        {
            List<Schedule> allSchedules = await App.ScheduleDatabase.GetPlayersAsync();
            foreach (Schedule schedule in allSchedules)
            {
                schedule.Played = false;
            }
            listView.ItemsSource = allSchedules;
            await App.ScheduleDatabase.UpdateSchedue(allSchedules);
        }
    }
}