using AppControleGlicemia.Models;
using SQLite;
using System;
using System.Collections.Generic;

namespace AppControleGlicemia.Services
{
    public class ServicesDbDextro
    {
        SQLiteConnection conn;

        public string StatusMessage { get; set; }

        public ServicesDbDextro(string dbPath)
        {
            if (dbPath == "")
                dbPath = App.DbPath;

            conn = new SQLiteConnection(dbPath);
            conn.CreateTable<ModelDextro>();
        }

        public void Inserir(ModelDextro dextro)
        {
            try
            {
                if (dextro.DataAferido > DateTime.Now)
                    throw new Exception("Não é possível inserir uma data futura");

                int result = conn.Insert(dextro);

                if (result > 0)
                {
                    this.StatusMessage = string.Format("{0} registro(s) adicionado(s)", result);
                }
                else
                {
                    this.StatusMessage = string.Format("0 registro(s) adicionado(s)");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Alterar(ModelDextro dextro)
        {
            try
            {
                int result = conn.Update(dextro);

                if (result > 0)
                {
                    this.StatusMessage = string.Format("{0} registro(s) alterado(s)", result);
                }
                else
                {
                    this.StatusMessage = string.Format("0 registro(s) alterado(s)");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ModelDextro> Listar(int periodo)
        {
            //List<ModelDextro> lista = new List<ModelDextro>();

            /*
             * Período:
             * 1 - Hoje
             * 2 - Ontem
             * 3 - Últimos 5 dias
             * 4 - Últimos 30 dias
             */

            DateTime dayStart = DateTime.Now.Date;
            DateTime dayEnd = DateTime.Now.Date.AddDays(+1);

            switch (periodo)
            {
                case 0:
                    dayStart = DateTime.Now.Date;
                    dayEnd = DateTime.Now.Date.AddDays(+1);
                    break;
                case 1:
                    dayStart = DateTime.Now.Date.AddDays(-1);
                    dayEnd = DateTime.Now.Date;
                    break;
                case 2:
                    dayStart = DateTime.Now.Date.AddDays(-4);
                    dayEnd = DateTime.Now.Date.AddDays(+1);
                    break;
                case 3:
                    dayStart = DateTime.Now.Date.AddDays(-29);
                    dayEnd = DateTime.Now.Date.AddDays(+1);
                    break;
            }

            try
            {
                var resp = conn.Table<ModelDextro>().Where(x => x.DataAferido >= dayStart && x.DataAferido < dayEnd).ToList();

                List<ModelDextro> lista = new List<ModelDextro>();

                var i = 0;

                foreach (var item in resp)
                {
                    var dextro = new ModelDextro();
                    dextro = item;
                    dextro.Stats = dextro.ValorAferido > i ? "IconUp" : dextro.ValorAferido < i ? "IconDown" : "IconEqual";
                    
                    if (String.IsNullOrEmpty(dextro.InsulinaTipo))
                    {
                        dextro.MostraInsulina = false;
                    }
                    else
                    {
                        dextro.MostraInsulina = true;
                    }
                    

                    i = item.ValorAferido;

                    lista.Add(dextro);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ModelDextro Retornar(int id)
        {
            try
            {
                return conn.Table<ModelDextro>().Where(x => x.DextroId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ModelDextro RetornarUltimaAfericao()
        {
            try
            {
                return conn.Table<ModelDextro>().OrderByDescending(x => x.DataAferido).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ModelEstatistica RetornarMediaDia(DateTime data)
        {
            try
            {
                var dayStart = data.Date;
                var dayEnd = data.Date.AddDays(+1);

                var lista = conn.Table<ModelDextro>().Where(x => x.DataAferido >= dayStart && x.DataAferido < dayEnd).ToList();

                int quantidade = 0;
                int soma = 0;
                foreach (var item in lista){
                    soma += item.ValorAferido;
                    quantidade++;
                }

                var estatistica = new ModelEstatistica()
                {
                    Media = quantidade > 0 ? soma / quantidade : 0,
                    Quantidade = quantidade
                };

                return estatistica;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Excluir(int id)
        {
            try
            {
                int result = conn.Table<ModelDextro>().Delete(x => x.DextroId == id);

                StatusMessage = string.Format("{0} Registro(s) excluídos(s)", result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
