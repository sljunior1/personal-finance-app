using System.Collections.ObjectModel;
using System.Windows.Input;
using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp;

public partial class CartaoCreditoPage : ContentPage
{
	private readonly DatabaseService _databaseService;
	private ObservableCollection<CartaoCreditoModel> _cartoesCredito;
    public ObservableCollection<CartaoCreditoModel> CartoesCredito
    {
        get => _cartoesCredito;
        set
        {
            _cartoesCredito = value;
            OnPropertyChanged();
        }
    }
	public ICommand DoubleTapCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; } 
	public CartaoCreditoPage()
	{
		InitializeComponent();
		_databaseService = new DatabaseService();
        BindingContext = this;
		DoubleTapCommand = new Command<CartaoCreditoModel>(async (cartaoCredito) => await OnDoubleTap(cartaoCredito));
        DeleteCommand = new Command<CartaoCreditoModel>(async (cartaoCredito) => await DeleteCartaoCredito(cartaoCredito));
        InitializeDataAsync();
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
		await InitializeDataAsync();
    }
	private async Task InitializeDataAsync()
	{
		try
		{
			var cartaoCredito = await _databaseService.ListarCartoesCreditoAsync();
            foreach(var item in cartaoCredito)
            {
                item.IconBandeira = item.Bandeira.Equals("Visa") ? "ic_visa.png" : "ic_mastercard.png";
            }
			CartoesCredito = new ObservableCollection<CartaoCreditoModel>(cartaoCredito);
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", "Falha ao carregar cartões de crédito. Tente novamente.", "OK");
		}
	}
	private void TituloCustom_AdicionarClicked(object sender, EventArgs e)
    {
		AppShell.Current.Navigation.PushModalAsync(new CartaoCreditoAddPage());
    }
private async Task OnDoubleTap(CartaoCreditoModel cartaoCredito)	
    {
        try
        {
            if (cartaoCredito == null)
                return;

                await Shell.Current.Navigation.PushAsync(new CartaoCreditoEditarPage(cartaoCredito));
		}
            catch (Exception)
            {
                await DisplayAlert("Erro", "Falha ao abrir a tela de edição.", "OK");
            }
        }
private async Task DeleteCartaoCredito(CartaoCreditoModel cartaoCredito)
    {
        try
        {
            if (cartaoCredito == null)
            {
                return;
            }

            bool confirm = await DisplayAlert("Confirmação", $"Deseja excluir o cartão de crédito '{cartaoCredito.Numero}'?", "Sim", "Não");
            if (!confirm)
            {
                return;
            }

            CartoesCredito.Remove(cartaoCredito);
            if (cartaoCredito.Id != 0)
            {
                await _databaseService.ExcluirCartaoCreditoAsync(cartaoCredito);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Falha ao excluir o cartão de crédito: {ex.Message}", "OK");
        }
    }
	}