using AppControleGlicemia.Views.Dextro;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppControleGlicemia.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageMenu : FlyoutPage
    {
        public PageMenu()
        {
            InitializeComponent();

            this.Detail = new NavigationPage(new PageHome());
            IsPresented = false;
        }

        private void btHome_Clicked(object sender, EventArgs e)
        {
            this.Detail = new NavigationPage(new PageHome());
            IsPresented = false;
        }

        private void btDextroCadastro_Clicked(object sender, EventArgs e)
        {
            this.Detail = new NavigationPage(new PageDextroCadastro());
            IsPresented = false;
        }

        private void btDextroLista_Clicked(object sender, EventArgs e)
        {
            this.Detail = new NavigationPage(new PageDextroLista());
            IsPresented = false;
        }

        private void btSobre_Tapped(object sender, EventArgs e)
        {
            this.Detail = new NavigationPage(new PageSobre());
            IsPresented = false;
        }

        private void btInsulinaTipo_Tapped(object sender, EventArgs e)
        {
            this.Detail = new NavigationPage(new PageInsulina());
            IsPresented = false;
        }

        private void btPrivacidade_Tapped(object sender, EventArgs e)
        {
            Browser.OpenAsync("http://www.devmaia.com.br/privacidade/glicapp");
        }
    }
}