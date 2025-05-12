using System;
using Microsoft.Maui.Controls.Internals;
using SQLite;

namespace PersonalFinanceApp.Models;

public class ContaBancariaModel
{
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NomeBanco { get; set; }
         public string Apelido { get; set; }       
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string TipoConta { get; set; }
        public string IconBanco { get; set; }
}
