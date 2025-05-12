using System.Collections.ObjectModel;
using System.Windows.Input;
using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp;

public partial class ContaBancariaPage : ContentPage
{
	private readonly DatabaseService _databaseService;
	private ObservableCollection<ContaBancariaModel> _contasBancaria;
    public ObservableCollection<ContaBancariaModel> ContasBancaria
    {
        get => _contasBancaria;
        set
        {
            _contasBancaria = value;
            OnPropertyChanged();
        }
    }
	public ICommand DoubleTapCommand { get; private set; }
    public ICommand DeleteCommand { get; private set; } 
	public ContaBancariaPage()
	{
		InitializeComponent();
		_databaseService = new DatabaseService();
        BindingContext = this;
		DoubleTapCommand = new Command<ContaBancariaModel>(async (contaBancaria) => await OnDoubleTap(contaBancaria));
		DeleteCommand = new Command<ContaBancariaModel>(async (contaBancaria) => await DeleteContaBancaria(contaBancaria));
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
			var contasBancaria = await _databaseService.ListarContasBancariaAsync();
            foreach(var item in contasBancaria)
            {
				if(item.NomeBanco.Equals("C6 Bank"))
					item.IconBanco = "c6bank.png";
				else if(item.NomeBanco.Equals("Itaú"))
					item.IconBanco = "itau.png";
				else if(item.NomeBanco.Equals("Caixa Econômica"))
					item.IconBanco = "ic_caixaeconomica.png";
				else if(item.NomeBanco.Equals("Genial"))
					item.IconBanco = "ic_genial.png";
				else if(item.NomeBanco.Equals("Nubank"))
					item.IconBanco = "ic_nubank.png";
                else if(item.NomeBanco.Equals("Hantec"))
					item.IconBanco = "ic_hantec.png";
            }

			ContasBancaria = new ObservableCollection<ContaBancariaModel>(contasBancaria);
		}
		catch (Exception)
		{
			await DisplayAlert("Erro", "Falha ao carregar contas bancárias. Tente novamente.", "OK");
		}
	}
		private void TituloCustom_AdicionarClicked(object sender, EventArgs e)
    	{
			AppShell.Current.Navigation.PushModalAsync(new ContaBancariaAddPage());
    	}
	private async Task OnDoubleTap(ContaBancariaModel contaBancaria)	
    {
        try
        {
            if (contaBancaria == null)
                return;

                await Shell.Current.Navigation.PushAsync(new ContaBancariaEditarPage(contaBancaria));
		}
            catch (Exception)
            {
                await DisplayAlert("Erro", "Falha ao abrir a tela de edição.", "OK");
            }
        }
	private async Task DeleteContaBancaria(ContaBancariaModel contaBancaria)
    {
        try
        {
            if (contaBancaria == null)
            {
                return;
            }

            bool confirm = await DisplayAlert("Confirmação", $"Deseja excluir a conta bancária 'agência? {contaBancaria.Agencia} | conta: {contaBancaria.Conta}'?", "Sim", "Não");
            if (!confirm)
            {
                return;
            }

            ContasBancaria.Remove(contaBancaria);
            if (contaBancaria.Id != 0)
            {
                await _databaseService.ExcluirContaBancariaAsync(contaBancaria);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Falha ao excluir a conta bancária: {ex.Message}", "OK");
        }
    }
	}