using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp;

public partial class CategoriaEditarPage : ContentPage
{
	private readonly DatabaseService _databaseService;
    private readonly CategoriaModel _categoria;
	public CategoriaEditarPage(CategoriaModel categoria)
	{
		InitializeComponent();
		_databaseService = new DatabaseService();
        _categoria = categoria ?? throw new ArgumentNullException(nameof(categoria));
        BindingContext = this;
        txtDescricao.Text = _categoria.Descricao;
	}
    private async void Atualizar_Clicked(object sender, EventArgs e)
    {
        try
        {
            string descricao = txtDescricao.Text?.Trim();

            if (string.IsNullOrEmpty(descricao))
            {
                await DisplayAlert("Erro", "Por favor, preencha a descrição.", "OK");
                return;
            }

                var listaCategorias = await _databaseService.ListarCategoriasAsync();


                if (listaCategorias.Any(et => et.Id != _categoria.Id && et.Descricao.Equals(descricao, StringComparison.OrdinalIgnoreCase)))
                {
                    await DisplayAlert("Erro", "Já existe uma categoria com essa descrição.", "OK");
                    return;
                }

                _categoria.Descricao = descricao;

                await _databaseService.SalvarCategoriaAsync(_categoria);

                await DisplayAlert("Sucesso", $"Categoria '{descricao}' atualizada com sucesso!", "OK");

                await Shell.Current.GoToAsync(nameof(CategoriaPage));
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao salvar a categoria: {ex.Message}", "OK");
            }
        }
}