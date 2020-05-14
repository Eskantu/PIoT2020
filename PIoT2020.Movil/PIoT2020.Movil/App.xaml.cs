using Com.OneSignal;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PIoT2020.Movil
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);

            MainPage =new NavigationPage(new MainPage());
            OneSignal.Current.StartInit("238118d4-da17-4dc2-94cf-31ca9df6a7ed");
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
