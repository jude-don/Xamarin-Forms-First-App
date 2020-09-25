using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyPart2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Plugin.Media.CrossMedia.Current.Initialize();
            //MainPage = new MainPage();
            MainPage = new NavigationPage(new ScanDocPage());
            
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
