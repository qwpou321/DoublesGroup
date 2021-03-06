using System;
using System.IO;
using Xamarin.Forms;

namespace DoublesGroup
{
    public partial class App : Application
    {
        static PlayerDatabase playerDatabase;
        static ScheduleDatabase scheduleDatabase;

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        public static PlayerDatabase PlayerDatabase
        {
            get
            {
                if (playerDatabase == null)
                {
                    playerDatabase = new PlayerDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Player.db3"));
                }
                return playerDatabase;
            }
        }

        public static ScheduleDatabase ScheduleDatabase 
        {
            get 
            {
                if (scheduleDatabase == null)
                {
                    scheduleDatabase = new ScheduleDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Schedule.db3"));
                }
                return scheduleDatabase;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
