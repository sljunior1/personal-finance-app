using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp;

public partial class CategoriaAddPage : ContentPage
{
	private readonly DatabaseService _databaseService;
	public CategoriaAddPage()
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

                var existeCategorias = await _databaseService.ListarCategoriasAsync();
                if (existeCategorias.Any(et => et.Descricao.Equals(descricao, StringComparison.OrdinalIgnoreCase)))
                {
                    await DisplayAlert("Erro", "Já existe uma categoria com essa descrição.", "OK");
                    return;
                }

                var categoria = new CategoriaModel
                {
                    Descricao = descricao
                };

                await _databaseService.SalvarCategoriaAsync(categoria);

                await DisplayAlert("Sucesso", $"Categoria '{descricao}' cadastrada com sucesso!", "OK");

                txtDescricao.Text = string.Empty;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao salvar a categoria: {ex.Message}", "OK");
            }
    }
}