
using System.Collections.ObjectModel;
using System.Windows.Input;
using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp;

public partial class CategoriaPage : ContentPage
{
	private readonly DatabaseService _databaseService;
	private ObservableCollection<CategoriaModel> _categorias;
    public ObservableCollection<CategoriaModel> Categorias
    {
        get => _categorias;
        set
        {
            _categorias = value;
            OnPropertyChanged();
        }
    }
	public ICommand DoubleTapCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; } 
	public CategoriaPage()
	{
		InitializeComponent();
		_databaseService = new DatabaseService();
        BindingContext = this;
		DoubleTapCommand = new Command<CategoriaModel>(async (categoria) => await OnDoubleTap(categoria));
		DeleteCommand = new Command<CategoriaModel>(async (categoria) => await DeleteCategoria(categoria));
        InitializeDataAsync();
	}
	private async Task InitializeDataAsync()
	{
		try
		{
			var categoria = await _databaseService.ListarCategoriasAsync();
			Categorias = new ObservableCollection<CategoriaModel>(categoria);
		}
		catch (Exception)
		{
			await DisplayAlert("Erro", "Falha ao carregar categorias. Tente novamente.", "OK");
		}
	}
	protected override async void OnAppearing()
    {
        base.OnAppearing();
		await InitializeDataAsync();
    }
	private void TituloCustom_AdicionarClicked(object sender, EventArgs e)
    {
		AppShell.Current.Navigation.PushModalAsync(new CategoriaAddPage());
    }
	private async Task OnDoubleTap(CategoriaModel categoria)	
    {
        try
        {
            if (categoria == null)
                return;

                await Shell.Current.Navigation.PushAsync(new CategoriaEditarPage(categoria));
		}
            catch (Exception)
            {
                await DisplayAlert("Erro", "Falha ao abrir a tela de edição.", "OK");
            }
	}
	private async Task DeleteCategoria(CategoriaModel categoria)
    {
        try
        {
            if (categoria == null)
            {
                return;
            }

            bool confirm = await DisplayAlert("Confirmação", $"Deseja excluir a categoria '{categoria.Descricao}'?", "Sim", "Não");
            if (!confirm)
            {
                return;
            }

            Categorias.Remove(categoria);
            if (categoria.Id != 0)
            {
                await _databaseService.ExcluirCategoriaAsync(categoria);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Falha ao excluir a categoria: {ex.Message}", "OK");
        }
    }
}