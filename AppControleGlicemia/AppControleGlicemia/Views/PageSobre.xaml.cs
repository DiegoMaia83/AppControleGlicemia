﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppControleGlicemia.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageSobre : ContentPage
    {
        public PageSobre()
        {
            InitializeComponent();

            txtLocalArmazenamento.Text = App.DbPath;
        }
    }
}