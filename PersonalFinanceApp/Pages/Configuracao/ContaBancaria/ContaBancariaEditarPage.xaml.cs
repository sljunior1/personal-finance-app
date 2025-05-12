using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp;

public partial class ContaBancariaEditarPage : ContentPage
{
	private readonly DatabaseService _databaseService;
    private readonly ContaBancariaModel _contaBancaria;
	public ContaBancariaEditarPage(ContaBancariaModel contaBancaria)
	{
		InitializeComponent();
		_databaseService = new DatabaseService();
        _contaBancaria = contaBancaria ?? throw new ArgumentNullException(nameof(contaBancaria));
        BindingContext = this;
        txtApelido.Text = _contaBancaria.Apelido;
        txtAgencia.Text = _contaBancaria.Agencia;
		txtConta.Text = _contaBancaria.Conta;
		selectBanco.SelectedItem = _contaBancaria.NomeBanco;
		selectTipoConta.SelectedItem = _contaBancaria.TipoConta;
	}
	private async void Atualizar_Clicked(object sender, EventArgs e)
    {
        try
        {
                string apelido = txtApelido.Text.Trim();
                string banco = selectBanco.SelectedItem.ToString();
				string conta = txtConta.Text.Replace("-","");
				string tipoConta = selectTipoConta.SelectedItem.ToString();

                if (string.IsNullOrEmpty(apelido))
                {
                    await DisplayAlert("Erro", "Por favor, preencha o apelido da conta.", "OK");
                    return;
                }
                if (string.IsNullOrEmpty(banco))
                {
                    await DisplayAlert("Erro", "Por favor, preencha o nome do Banco.", "OK");
                    return;
                }

                if (string.IsNullOrEmpty(conta))
                {
                    await DisplayAlert("Erro", "Por favor, preencha a conta.", "OK");
                    return;
                }

                var existeContaBancaria = await _databaseService.ListarContasBancariaAsync();
                if (existeContaBancaria.Any(et => et.NomeBanco.Equals(banco, StringComparison.OrdinalIgnoreCase) ||
					et.Conta.Equals(conta, StringComparison.OrdinalIgnoreCase)))
                {
                    await DisplayAlert("Erro", "Já existe uma conta bancária cadastrada.", "OK");
                    return;
                }
                if (existeContaBancaria.Any(et => et.Apelido.Equals(apelido, StringComparison.OrdinalIgnoreCase)))
                {
                    await DisplayAlert("Erro", "Já existe uma conta bancária cadastrada para esse apelido.", "OK");
                    return;
                }

                _contaBancaria.Apelido = apelido;
                _contaBancaria.Agencia = txtAgencia.Text;
				_contaBancaria.Conta = conta;
				_contaBancaria.NomeBanco = banco;
				_contaBancaria.TipoConta = tipoConta;

                await _databaseService.SalvarContaBancariaAsync(_contaBancaria);

                await DisplayAlert("Sucesso", $"Conta bancária atualizada com sucesso!", "OK");

                await Shell.Current.GoToAsync(nameof(TipoGastoPage));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao salvar conta bancária: {ex.Message}", "OK");
            }
        }
}