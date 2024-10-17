using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppControleGlicemia.Models
{
    [Preserve(AllMembers = true)]
    [Table("Dextro")]
    public class ModelDextro
    {
        [PrimaryKey, AutoIncrement]
        public int DextroId { get; set; }

        [NotNull]
        public int ValorAferido { get; set; }

        [NotNull]
        public DateTime DataAferido { get; set; }

        public string InsulinaTipo { get; set; }

        public int InsulinaQuantidade { get; set; }

        [Ignore]
        public string Stats { get; set; }

        [Ignore]
        public bool MostraInsulina { get; set; }
    }
}
