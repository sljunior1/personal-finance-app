using System;
using System.Runtime.CompilerServices;
using SQLite;

namespace PersonalFinanceApp.Models;

public class CategoriaModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Descricao { get; set; }
}
