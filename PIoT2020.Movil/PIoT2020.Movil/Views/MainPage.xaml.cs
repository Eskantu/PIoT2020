using PIoT2020.Movil.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PIoT2020.Movil
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnRegistro_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Registro(), true);
        }

        private void BtnIniciarSesion_Clicked(object sender, EventArgs e)
        {

        }
    }
}
