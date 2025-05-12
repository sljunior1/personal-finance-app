using System.Collections.ObjectModel;
using System.Windows.Input;
using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp;

public partial class TipoGanhoPage : ContentPage
{
	private readonly DatabaseService _databaseService;
	private ObservableCollection<TipoGanhoModel> _tipoGanhos;
    public ObservableCollection<TipoGanhoModel> TipoGanhos
    {
        get => _tipoGanhos;
        set
        {
            _tipoGanhos = value;
            OnPropertyChanged();
        }
    }
    public ICommand DoubleTapCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; } 
	public TipoGanhoPage()
	{
		InitializeComponent();
		_databaseService = new DatabaseService();
        BindingContext = this;
		DoubleTapCommand = new Command<TipoGanhoModel>(async (tipoGanho) => await OnDoubleTap(tipoGanho));
        DeleteCommand = new Command<TipoGanhoModel>(async (tipoGanho) => await DeleteTipoGanho(tipoGanho));
        InitializeDataAsync();
	}
	private async Task InitializeDataAsync()
	{
		try
		{
			var tipoGanho = await _databaseService.ListarTipoGanhosAsync();
			TipoGanhos = new ObservableCollection<TipoGanhoModel>(tipoGanho);
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", "Falha ao carregar tipos de ganho. Tente novamente.", "OK");
		}
	}
	protected override async void OnAppearing()
    {
        base.OnAppearing();
		await InitializeDataAsync();
    }	
	private void TituloCustom_AdicionarClicked(object sender, EventArgs e)
    {
		AppShell.Current.Navigation.PushModalAsync(new TipoGanhoAddPage());
    }
	private async Task OnDoubleTap(TipoGanhoModel tipoGanho)	
    {
        try
        {
            if (tipoGanho == null)
                return;

                await Shell.Current.Navigation.PushAsync(new TipoGanhoEditarPage(tipoGanho));
		}
            catch (Exception)
            {
                await DisplayAlert("Erro", "Falha ao abrir a tela de edição.", "OK");
            }
        }
    private async Task DeleteTipoGanho(TipoGanhoModel tipoGanho)
    {
        try
        {
            if (tipoGanho == null)
            {
                return;
            }

            bool confirm = await DisplayAlert("Confirmação", $"Deseja excluir o tipo de ganho '{tipoGanho.Descricao}'?", "Sim", "Não");
            if (!confirm)
            {
                return;
            }

            TipoGanhos.Remove(tipoGanho);
            if (tipoGanho.Id != 0)
            {
                await _databaseService.ExcluirTipoGanhoAsync(tipoGanho);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Falha ao excluir o tipo de ganho: {ex.Message}", "OK");
        }
    }
	}