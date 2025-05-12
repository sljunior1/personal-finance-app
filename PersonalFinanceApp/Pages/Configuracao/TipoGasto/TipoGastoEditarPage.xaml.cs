using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp;

public partial class TipoGastoEditarPage : ContentPage
{
	private readonly DatabaseService _databaseService;
    private readonly TipoGastoModel _tipoGasto;
	public TipoGastoEditarPage(TipoGastoModel tipoGasto)
	{
		InitializeComponent();
		_databaseService = new DatabaseService();
        _tipoGasto = tipoGasto ?? throw new ArgumentNullException(nameof(tipoGasto));
        BindingContext = this;
        txtDescricao.Text = _tipoGasto.Descricao;
	}
	private async void Atualizar_Clicked(object sender, EventArgs e)
    {
        try
        {
            string description = txtDescricao.Text?.Trim();

            if (string.IsNullOrEmpty(description))
            {
                await DisplayAlert("Erro", "Por favor, preencha a descrição.", "OK");
                return;
            }

                var existingExpenseTypes = await _databaseService.ListarTipoGastosAsync();


                if (existingExpenseTypes.Any(et => et.Id != _tipoGasto.Id && et.Descricao.Equals(description, StringComparison.OrdinalIgnoreCase)))
                {
                    await DisplayAlert("Erro", "Já existe um tipo de gasto com essa descrição.", "OK");
                    return;
                }

                _tipoGasto.Descricao = description;

                await _databaseService.SalvarTipoGastoAsync(_tipoGasto);

                await DisplayAlert("Sucesso", $"Tipo de gasto '{description}' atualizado com sucesso!", "OK");

                await Shell.Current.GoToAsync(nameof(TipoGastoPage));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao salvar o tipo de gasto: {ex.Message}", "OK");
            }
        }
}