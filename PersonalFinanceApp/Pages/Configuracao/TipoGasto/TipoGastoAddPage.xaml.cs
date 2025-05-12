using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp;

public partial class TipoGastoAddPage : ContentPage
{
	private readonly DatabaseService _databaseService;
	public TipoGastoAddPage()
	{
		InitializeComponent();
		_databaseService = new DatabaseService();
	}

    private async void Salvar_Clicked(object sender, EventArgs e)
    {
		try
            {
                string descricao = txtDescricao.Text?.Trim();

                if (string.IsNullOrEmpty(descricao))
                {
                    await DisplayAlert("Erro", "Por favor, preencha a descrição.", "OK");
                    return;
                }

                var existeTipoGastos = await _databaseService.ListarTipoGastosAsync();
                if (existeTipoGastos.Any(et => et.Descricao.Equals(descricao, StringComparison.OrdinalIgnoreCase)))
                {
                    await DisplayAlert("Erro", "Já existe um tipo de gasto com essa descrição.", "OK");
                    return;
                }

                var tipoGasto = new TipoGastoModel
                {
                    Descricao = descricao
                };

                await _databaseService.SalvarTipoGastoAsync(tipoGasto);

                await DisplayAlert("Sucesso", $"Tipo de gasto '{descricao}' cadastrado com sucesso!", "OK");

                txtDescricao.Text = string.Empty;
            }
            catch (Exception)
            {
                await DisplayAlert("Erro", "Falha ao salvar o tipo de gasto. Tente novamente.", "OK");
            }
    }
}