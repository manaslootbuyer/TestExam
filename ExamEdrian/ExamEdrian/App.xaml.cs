using System;
using MvvmAspire;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExamEdrian
{
    public partial class App : Application
    {
        public static string WebUrl = "https://discoverycodetest.azurewebsites.net";

        public App()
        {
            InitializeComponent();
            var navigation = Resolver.Get<MvvmAspire.Services.INavigation>();
            MainPage = ((MvvmAspire.Services.XamarinFormsNavigation)navigation).NavigationPage;
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
