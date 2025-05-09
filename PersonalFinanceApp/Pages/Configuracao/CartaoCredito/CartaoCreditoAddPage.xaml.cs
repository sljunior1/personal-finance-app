using System.Globalization;
using System.Text.RegularExpressions;

namespace PersonalFinanceApp;

public partial class CartaoCreditoAddPage : ContentPage
{
	private bool _isFormatadoNumeroCartao;
	private bool _isFormatadoValidadeCartao;
	private bool _isFormatadoLimiteCartao;
	public CartaoCreditoAddPage()
	{
		InitializeComponent();
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
}