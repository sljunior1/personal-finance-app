using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp;

public partial class TipoGanhoEditarPage : ContentPage
{
	private readonly DatabaseService _databaseService;
    private readonly TipoGanhoModel _tipoGanho;
	public TipoGanhoEditarPage(TipoGanhoModel tipoGanho)
	{
		InitializeComponent();
		_databaseService = new DatabaseService();
        _tipoGanho = tipoGanho ?? throw new ArgumentNullException(nameof(tipoGanho));
        BindingContext = this;
        txtDescricao.Text = _tipoGanho.Descricao;
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

                var existingExpenseTypes = await _databaseService.ListarTipoGanhosAsync();


                if (existingExpenseTypes.Any(et => et.Id != _tipoGanho.Id && et.Descricao.Equals(description, StringComparison.OrdinalIgnoreCase)))
                {
                    await DisplayAlert("Erro", "Já existe um tipo de ganho com essa descrição.", "OK");
                    return;
                }

                _tipoGanho.Descricao = description;

                await _databaseService.SalvarTipoGanhoAsync(_tipoGanho);

                await DisplayAlert("Sucesso", $"Tipo de ganho '{description}' atualizado com sucesso!", "OK");

                await Shell.Current.GoToAsync(nameof(TipoGanhoPage));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao salvar o tipo de ganho: {ex.Message}", "OK");
            }
        }
}