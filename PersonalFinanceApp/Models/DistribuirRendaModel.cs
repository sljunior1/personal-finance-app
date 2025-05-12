using System;
using SQLite;

namespace PersonalFinanceApp.Models;

public class DistribuirRendaModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int Porcentagem { get; set; }
    [Ignore]
    public string TipoGastoDescricao { get; set; } 
    public int TipoGastoId { get; set; }
}
