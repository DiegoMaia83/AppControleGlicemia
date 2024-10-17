using AppControleGlicemia.Services;
using AppControleGlicemia.Views.Dextro;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppControleGlicemia.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageHome : ContentPage
    {
        ServicesDbDextro dbDextro = new ServicesDbDextro(App.DbPath);

        public PageHome()
        {
            InitializeComponent();

            var mediaHoje = dbDextro.RetornarMediaDia(DateTime.Now);
            txtMediaHoje.Text = mediaHoje.Media.ToString();
            txtQuantidadeHoje.Text = mediaHoje.Quantidade.ToString();

            var mediaOntem = dbDextro.RetornarMediaDia(DateTime.Now.AddDays(-1));
            txtMediaOntem.Text = mediaOntem.Media.ToString();
            txtQuantidadeOntem.Text = mediaOntem.Quantidade.ToString();

            var ultimaMedicao = dbDextro.RetornarUltimaAfericao();
            if(ultimaMedicao != null)
            {
                txtUltimaData.Text = ultimaMedicao.DataAferido != null ? ultimaMedicao.DataAferido.ToString() : "";
                txtUltimaMedicao.Text = ultimaMedicao.ValorAferido.ToString();
            }
        }

        private void btInserirDextro_Clicked(object sender, EventArgs e)
        {
            FlyoutPage page = (FlyoutPage)Application.Current.MainPage;
            page.Detail = new NavigationPage(new PageDextroCadastro());
        }
    }
}