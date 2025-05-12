using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp;

public partial class TipoGanhoAddPage : ContentPage
{
	private readonly DatabaseService _databaseService;
	public TipoGanhoAddPage()
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

                var existeTipoGastos = await _databaseService.ListarTipoGanhosAsync();
                if (existeTipoGastos.Any(et => et.Descricao.Equals(descricao, StringComparison.OrdinalIgnoreCase)))
                {
                    await DisplayAlert("Erro", "Já existe um tipo de ganho com essa descrição.", "OK");
                    return;
                }

                var tipoGasto = new TipoGanhoModel
                {
                    Descricao = descricao
                };

                await _databaseService.SalvarTipoGanhoAsync(tipoGasto);

                await DisplayAlert("Sucesso", $"Tipo de ganho '{descricao}' cadastrado com sucesso!", "OK");

                txtDescricao.Text = string.Empty;
            }
            catch (Exception)
            {
                await DisplayAlert("Erro", "Falha ao salvar o tipo de ganho. Tente novamente.", "OK");
            }
    }
}