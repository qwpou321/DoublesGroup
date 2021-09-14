using System;
using System.IO;
using Xamarin.Forms;

namespace DoublesGroup
{
    public partial class App : Application
    {
        static PlayerDatabase playerDatabase;
        static ScheduleTxt scheduleTxt;

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

        public static ScheduleTxt ScheduleTxt
        {
            get
            {
                if (scheduleTxt == null)
                {
                    scheduleTxt = new ScheduleTxt(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Schedule.txt"));
                }
                return scheduleTxt;
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
