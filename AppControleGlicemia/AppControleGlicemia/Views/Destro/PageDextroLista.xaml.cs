using AppControleGlicemia.Models;
using AppControleGlicemia.Services;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppControleGlicemia.Views.Dextro
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageDextroLista : ContentPage
    {
        public PageDextroLista()
        {
            InitializeComponent();

            pckPeriodo.SelectedIndex = 2;

            AtualizaLista(2);
        }

        public void AtualizaLista(int idxPeriodo)
        {
            ServicesDbDextro dbDextro = new ServicesDbDextro(App.DbPath);

            var lista = dbDextro.Listar(idxPeriodo).OrderByDescending(x => x.DataAferido).ToList();

            ListaDextro.ItemsSource = lista;
        }

        private void pckPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var idxPeriodo = pckPeriodo.SelectedIndex;

            AtualizaLista(idxPeriodo);
        }

        private void ListaDextro_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ModelDextro dextro = (ModelDextro)ListaDextro.SelectedItem;

            FlyoutPage page = (FlyoutPage)Application.Current.MainPage;
            page.Detail = new NavigationPage(new PageDextroCadastro(dextro));
        }
    }
}