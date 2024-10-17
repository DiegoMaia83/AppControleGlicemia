using AppControleGlicemia.Services;
using AppControleGlicemia.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppControleGlicemia.Views.Dextro
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageDextroCadastro : ContentPage
    {
        public PageDextroCadastro()
        {
            InitializeComponent();

            // Popula os campos data e hora com o horário atual
            DateTime now = DateTime.Now;
            txtData.Date = now.Date;
            txtHora.Time = new TimeSpan(now.Hour, now.Minute, now.Second);

            // Preenche o picker com os tipos de insulina
            PopularPickerInsulina();

            RetornarUltimaAfericao();
        }

        public PageDextroCadastro(ModelDextro dextro)
        {
            InitializeComponent();

            // Popula os campos data e hora com o horário que retorna da tabela
            DateTime now = dextro.DataAferido;
            txtData.Date = now.Date;
            txtHora.Time = new TimeSpan(now.Hour, now.Minute, now.Second);

            gridBtAlterar.IsVisible = true;
            gridBtInserir.IsVisible = false;

            txtDextroId.Text = dextro.DextroId.ToString();
            txtValorAferido.Text = dextro.ValorAferido.ToString();
            pckInsulina.SelectedItem = dextro.InsulinaTipo;
            qtdInsulina.Text = dextro.InsulinaQuantidade.ToString();

            // Preenche o picker com os tipos de insulina
            PopularPickerInsulina(dextro.InsulinaTipo);
        }

        private void btInserir_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Agrupa a data e hora selecionada em um DateTime
                var date = txtData.Date;
                var hour = txtHora.Time;
                DateTime datetime = date + hour;

                var dextro = new ModelDextro();
                dextro.ValorAferido = Convert.ToInt32(txtValorAferido.Text);
                dextro.DataAferido = datetime;
                dextro.InsulinaTipo = pckInsulina.SelectedItem != null ? pckInsulina.SelectedItem.ToString() : "";
                dextro.InsulinaQuantidade = !String.IsNullOrEmpty(qtdInsulina.Text) ? Convert.ToInt32(qtdInsulina.Text) : 0;

                ServicesDbDextro dbDextro = new ServicesDbDextro(App.DbPath);

                dbDextro.Inserir(dextro);

                DisplayAlert("Resultado da operação", dbDextro.StatusMessage, "OK");

                FlyoutPage page = (FlyoutPage)Application.Current.MainPage;
                page.Detail = new NavigationPage(new PageDextroLista());
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro", ex.Message, "Ok");
            };
        }

        private void BtAtualizaValor(object sender, EventArgs e)
        {
            Button bt = (Button)sender;

            var valor = Convert.ToInt32(txtValorAferido.Text);

            var valorAtual = valor;

            if (bt.Text == "+")
            {
                valorAtual = Adicao(valor);
            }
            else if (bt.Text == "-")
            {
                valorAtual = Subtracao(valor);
            }

            txtValorAferido.Text = valorAtual.ToString();
        }

        private int Subtracao(int valor)
        {
            return valor - 1;
        }

        private int Adicao(int valor)
        {
            return valor + 1;
        }

        private void RetornarUltimaAfericao()
        {
            ServicesDbDextro dbDextro = new ServicesDbDextro(App.DbPath);

            ModelDextro dextro = dbDextro.RetornarUltimaAfericao();

            if (dextro != null)
            {
                txtValorAferido.Text = dextro.ValorAferido.ToString();
            }
        }

        private void btAlterar_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Agrupa a data e hora selecionada em um DateTime
                var date = txtData.Date;
                var hour = txtHora.Time;
                DateTime datetime = date + hour;

                var dextro = new ModelDextro()
                {
                    DextroId = Convert.ToInt32(txtDextroId.Text),
                    ValorAferido = Convert.ToInt32(txtValorAferido.Text),
                    DataAferido = datetime,
                    InsulinaTipo = pckInsulina.SelectedItem != null ? pckInsulina.SelectedItem.ToString() : "",
                    InsulinaQuantidade = !String.IsNullOrEmpty(qtdInsulina.Text) ? Convert.ToInt32(qtdInsulina.Text) : 0
                };

                ServicesDbDextro dbDextro = new ServicesDbDextro(App.DbPath);

                dbDextro.Alterar(dextro);

                DisplayAlert("Resultado da operação", dbDextro.StatusMessage, "OK");

                FlyoutPage page = (FlyoutPage)Application.Current.MainPage;
                page.Detail = new NavigationPage(new PageDextroLista());
            }
            catch (Exception ex)
            {
                DisplayAlert("Erro", ex.Message, "Ok");
            };
        }

        private async void btExcluir_Clicked(object sender, EventArgs e)
        {
            var resp = await DisplayAlert("Excluir registro", "Deseja realmente excluir essa medição? Essa operação é irreversível!", "Sim", "Não");

            if(resp)
            {
                var id = Convert.ToInt32(txtDextroId.Text);
                ServicesDbDextro dbDextro = new ServicesDbDextro(App.DbPath);
                dbDextro.Excluir(id);
                await DisplayAlert("Resultado da operação", dbDextro.StatusMessage, "OK");

                FlyoutPage page = (FlyoutPage)Application.Current.MainPage;
                page.Detail = new NavigationPage(new PageHome());
            }
        }

        public void PopularPickerInsulina()
        {
            ServicesDbInsulina dbInsulina = new ServicesDbInsulina(App.DbPath);

            var lista = dbInsulina.Listar();

            foreach (var item in lista)
            {
                pckInsulina.Items.Add(item.Tipo);
            }

            pckInsulina.Items.Add("Adicionar novo");
        }

        public void PopularPickerInsulina(string selecionado = "")
        {
            ServicesDbInsulina dbInsulina = new ServicesDbInsulina(App.DbPath);

            var lista = dbInsulina.Listar();

            foreach (var item in lista)
            {
                pckInsulina.Items.Add(item.Tipo);
            }

            pckInsulina.SelectedItem = selecionado;

            pckInsulina.Items.Add("Adicionar novo");
        }

        private void pckInsulina_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pck = (Picker)sender;

            if (pck.SelectedItem.ToString() == "Adicionar novo")
            {
                FlyoutPage page = (FlyoutPage)Application.Current.MainPage;
                page.Detail = new NavigationPage(new PageInsulina());
            };
        }
    }
}