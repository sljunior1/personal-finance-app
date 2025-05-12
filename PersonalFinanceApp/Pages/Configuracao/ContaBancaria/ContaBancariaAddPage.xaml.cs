using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp;

public partial class ContaBancariaAddPage : ContentPage
{
	private readonly DatabaseService _databaseService;
	public ContaBancariaAddPage()
	{
		InitializeComponent();
		_databaseService = new DatabaseService();
	}
	private async void Salvar_Clicked(object sender, EventArgs e)
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
                if (existeContaBancaria.Any(et => et.Agencia.Equals(txtAgencia.Text, StringComparison.OrdinalIgnoreCase) ||
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

                var contaBancaria = new ContaBancariaModel
                {
                    Apelido = apelido,
                     Agencia = txtAgencia.Text,
					 Conta = conta,
					 NomeBanco = banco,
					 TipoConta = tipoConta
                };

                await _databaseService.SalvarContaBancariaAsync(contaBancaria);

                await DisplayAlert("Sucesso", "Conta bancária cadastrada com sucesso!", "OK");

                txtApelido.Text = string.Empty;
                txtAgencia.Text = string.Empty;
				txtConta.Text = string.Empty;
				selectBanco.SelectedItem = string.Empty;
				selectTipoConta.SelectedItem = string.Empty;
            }
            catch (Exception)
            {
                await DisplayAlert("Erro", "Falha ao salvar conta bancária. Tente novamente.", "OK");
            }
    }
}