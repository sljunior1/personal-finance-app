using System;
using SQLite;

namespace PersonalFinanceApp.Models;

public class TipoGastoModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Descricao { get; set; }
}
