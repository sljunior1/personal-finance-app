using System.Globalization;
using System.Text.RegularExpressions;
using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp;

public partial class CartaoCreditoAddPage : ContentPage
{
	private readonly DatabaseService _databaseService;
	private bool _isFormatadoNumeroCartao;
	private bool _isFormatadoValidadeCartao;
	private bool _isFormatadoLimiteCartao;
	public CartaoCreditoAddPage()
	{
		InitializeComponent();
		_databaseService = new DatabaseService();
	}

    private void txtNumeroCartao_TextChanged(object sender, TextChangedEventArgs e)
    {
		if (_isFormatadoNumeroCartao)
                return;

            _isFormatadoNumeroCartao = true;

            var entry = sender as Entry;
            string text = Regex.Replace(entry.Text, "[^0-9]", "");

            if (string.IsNullOrEmpty(text))
            {
                entry.Text = string.Empty;
                _isFormatadoNumeroCartao = false;
                return;
            }

            if (text.Length > 16)
                text = text.Substring(0, 16);

            string formatted = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (i > 0 && i % 4 == 0)
                    formatted += " ";
                formatted += text[i];
            }

            entry.Text = formatted;
            entry.CursorPosition = formatted.Length;

            _isFormatadoNumeroCartao = false;
    }
    private void txtValidade_TextChanged(object sender, TextChangedEventArgs e)
    {
		if (_isFormatadoValidadeCartao)
                return;

            _isFormatadoValidadeCartao = true;

            var entry = sender as Entry;
            string text = Regex.Replace(entry.Text, "[^0-9]", "");

            if (string.IsNullOrEmpty(text))
            {
                entry.Text = string.Empty;
                _isFormatadoValidadeCartao = false;
                return;
            }

            if (text.Length > 4)
                text = text.Substring(0, 4);

            string formatted = text;
            if (text.Length > 2)
                formatted = text.Substring(0, 2) + "/" + text.Substring(2);
            else if (text.Length == 2 && e.OldTextValue?.Length < 2)
                formatted = text + "/";

            entry.Text = formatted;
            entry.CursorPosition = formatted.Length;

            _isFormatadoValidadeCartao = false;
    }
    private void txtLimiteCartao_TextChanged(object sender, TextChangedEventArgs e)
    {
		if (_isFormatadoLimiteCartao)
                return;

            _isFormatadoLimiteCartao = true;

            var entry = sender as Entry;
            string text = entry.Text?.Replace("R$", "").Replace(".", "").Replace(",", "").Trim();

            if (string.IsNullOrEmpty(text))
            {
                entry.Text = string.Empty;
                _isFormatadoLimiteCartao = false;
                return;
            }

            if (decimal.TryParse(text, out decimal value))
            {
                entry.Text = (value / 100).ToString("C", new CultureInfo("pt-BR"));
            }
            else
            {
                entry.Text = string.Empty;
            }

            _isFormatadoLimiteCartao = false;
    }
private async void Salvar_Clicked(object sender, EventArgs e)
    {
		try
            {
                string numeroCartao = txtNumeroCartao.Text.Replace(" ","");
                string nomeTitular = txtNomeTitular.Text?.Trim();
                string limiteSemFormatacao = txtLimiteCartao.Text.Replace("R$","");
                decimal limiteCartao = decimal.Parse(limiteSemFormatacao);
                string bandeira = selectBandeira.SelectedItem.ToString();
                int diaFatura = int.Parse(txtDataVencimentoFatura.Text);

                if (string.IsNullOrEmpty(numeroCartao))
                {
                    await DisplayAlert("Erro", "Por favor, preencha o número do cartão.", "OK");
                    return;
                }

                if (string.IsNullOrEmpty(limiteSemFormatacao))
                {
                    await DisplayAlert("Erro", "Por favor, preencha o limite do cartão.", "OK");
                    return;
                }

                if (string.IsNullOrEmpty(bandeira))
                {
                    await DisplayAlert("Erro", "Por favor, selecione a bandeira do cartão.", "OK");
                    return;
                }

                var existeCartoes = await _databaseService.ListarCartoesCreditoAsync();
                if (existeCartoes.Any(et => et.Numero.Equals(numeroCartao, StringComparison.OrdinalIgnoreCase)))
                {
                    await DisplayAlert("Erro", "Já existe um cartão de crédito com essa numeração.", "OK");
                    return;
                }

                var cartaoCredito = new CartaoCreditoModel
                {
                    Numero = numeroCartao,
                    Bandeira = bandeira,
                    CodigoSeguranca = txtCodigoSeguranca.Text,
                    DataVencimento = txtValidade.Text,
                     DiaVencimentoFatura = diaFatura,
                    Limite = limiteCartao,
                    Titular = nomeTitular
                };

                await _databaseService.SalvarCartaoCreditoAsync(cartaoCredito);

                await DisplayAlert("Sucesso", "Cartão de Crédito cadastrado com sucesso!", "OK");

                txtNumeroCartao.Text = string.Empty;
                txtCodigoSeguranca.Text = string.Empty;
                txtDataVencimentoFatura.Text = string.Empty;
                txtLimiteCartao.Text = string.Empty;
                txtNomeTitular.Text = string.Empty;
                txtValidade.Text = string.Empty;
                selectBandeira.SelectedIndex = 0;
            }
            catch (Exception)
            {
                await DisplayAlert("Erro", "Falha ao salvar o cartão de crédito. Tente novamente.", "OK");
            }
    }
}