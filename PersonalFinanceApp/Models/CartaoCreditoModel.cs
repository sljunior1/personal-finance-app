using System;
using SQLite;

namespace PersonalFinanceApp.Models;

public class CartaoCreditoModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Numero { get; set; }
    public string Titular { get; set; }
    public string DataVencimento { get; set; }
    public string CodigoSeguranca { get; set; }
    public decimal Limite{ get; set; }
    public int DiaVencimentoFatura { get; set; }
    public string Bandeira { get; set; }
    [Ignore]
    public string IconBandeira { get; set; }

[   Ignore]
    public string FormatacaoNumero
    {
        get
        {
            if (string.IsNullOrEmpty(Numero) || Numero.Length < 4)
                return Numero;

                string lastFour = Numero.Length >= 4 ? Numero.Substring(Numero.Length - 4) : Numero;

                string masked = "**** **** **** " + lastFour;
                return masked;
            }
        }
}
