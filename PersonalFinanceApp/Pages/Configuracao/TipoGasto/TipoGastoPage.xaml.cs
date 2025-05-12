using System.Collections.ObjectModel;
using System.Windows.Input;
using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp;

public partial class TipoGastoPage : ContentPage
{
	private readonly DatabaseService _databaseService;
	private ObservableCollection<TipoGastoModel> _tipoGastos;
    public ObservableCollection<TipoGastoModel> TipoGastos
    {
        get => _tipoGastos;
        set
        {
            _tipoGastos = value;
            OnPropertyChanged();
        }
    }
	public ICommand DoubleTapCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; } 
	public TipoGastoPage()
	{
		InitializeComponent();
		_databaseService = new DatabaseService();
        BindingContext = this;
		DoubleTapCommand = new Command<TipoGastoModel>(async (tipoGasto) => await OnDoubleTap(tipoGasto));
        DeleteCommand = new Command<TipoGastoModel>(async (tipoGasto) => await DeleteTipoGasto(tipoGasto));
        InitializeDataAsync();
	}
	private async Task InitializeDataAsync()
	{
		try
		{
			var tipoGasto = await _databaseService.ListarTipoGastosAsync();
			TipoGastos = new ObservableCollection<TipoGastoModel>(tipoGasto);
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", "Falha ao carregar tipos de gasto. Tente novamente.", "OK");
		}
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
		await InitializeDataAsync();
    }
	private void TituloCustom_AdicionarClicked(object sender, EventArgs e)
    {
		AppShell.Current.Navigation.PushModalAsync(new TipoGastoAddPage());
    }
	private async Task OnDoubleTap(TipoGastoModel tipoGasto)	
    {
        try
        {
            if (tipoGasto == null)
                return;

                await Shell.Current.Navigation.PushAsync(new TipoGastoEditarPage(tipoGasto));
		}
            catch (Exception)
            {
                await DisplayAlert("Erro", "Falha ao abrir a tela de edição.", "OK");
            }
        }
    private async Task DeleteTipoGasto(TipoGastoModel tipoGasto)
    {
        try
        {
            if (tipoGasto == null)
            {
                return;
            }

            bool confirm = await DisplayAlert("Confirmação", $"Deseja excluir o tipo de gasto '{tipoGasto.Descricao}'?", "Sim", "Não");
            if (!confirm)
            {
                return;
            }

            TipoGastos.Remove(tipoGasto);
            if (tipoGasto.Id != 0)
            {
                await _databaseService.ExcluirTipoGastoAsync(tipoGasto);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Falha ao excluir o tipo de gasto: {ex.Message}", "OK");
        }
    }
	}
