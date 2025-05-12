using System.Globalization;
using System.Text.RegularExpressions;
using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp;

public partial class CartaoCreditoEditarPage : ContentPage
{
	private bool _isFormatadoNumeroCartao;
	private bool _isFormatadoValidadeCartao;
	private bool _isFormatadoLimiteCartao;
	private readonly DatabaseService _databaseService;
    private readonly CartaoCreditoModel _cartaoCredito;
	public CartaoCreditoEditarPage(CartaoCreditoModel cartaoCredito)
	{
		InitializeComponent();
		_databaseService = new DatabaseService();
        _cartaoCredito = cartaoCredito ?? throw new ArgumentNullException(nameof(cartaoCredito));
        BindingContext = this;
        txtCodigoSeguranca.Text = _cartaoCredito.CodigoSeguranca;
		txtDataVencimentoFatura.Text = _cartaoCredito.DiaVencimentoFatura.ToString();
		txtLimiteCartao.Text = "R$ " + _cartaoCredito.Limite.ToString("C");
		txtNomeTitular.Text = _cartaoCredito.Titular;
		txtValidade.Text = _cartaoCredito.DataVencimento;
		selectBandeira.SelectedItem = _cartaoCredito.Bandeira;
        txtNumeroCartao.Text = _cartaoCredito.Numero;
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
	private async void Atualizar_Clicked(object sender, EventArgs e)
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

                var existingExpenseTypes = await _databaseService.ListarCartoesCreditoAsync();


                if (existingExpenseTypes.Any(et => et.Id != _cartaoCredito.Id && et.Numero.Equals(numeroCartao, StringComparison.OrdinalIgnoreCase)))
                {
                    await DisplayAlert("Erro", "Já existe um cartão de crédito com esse número.", "OK");
                    return;
                }

                _cartaoCredito.Numero = numeroCartao;
				_cartaoCredito.Bandeira = bandeira;
				_cartaoCredito.CodigoSeguranca = txtCodigoSeguranca.Text;
				_cartaoCredito.DataVencimento = txtValidade.Text;
				_cartaoCredito.DiaVencimentoFatura = diaFatura;
				_cartaoCredito.Limite = limiteCartao;
				_cartaoCredito.Titular = nomeTitular;

                await _databaseService.SalvarCartaoCreditoAsync(_cartaoCredito);

                await DisplayAlert("Sucesso", "Cartão de crédito atualizado com sucesso!", "OK");

                await Shell.Current.GoToAsync(nameof(CartaoCreditoPage));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao salvar o cartão de crédito: {ex.Message}", "OK");
            }
        }
}