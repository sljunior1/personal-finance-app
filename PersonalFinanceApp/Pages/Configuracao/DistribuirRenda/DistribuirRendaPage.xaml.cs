using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp;

public partial class DistribuirRendaPage : ContentPage
{
        private readonly DatabaseService _databaseService;
        private ObservableCollection<DistribuirRendaModel> _distribuirRenda;
        private ObservableCollection<TipoGastoModel> _tipoGastos;
        private DistribuirRendaModel _selectDistribuirRenda;
        public ObservableCollection<DistribuirRendaModel> DistribuirRenda
        {
            get => _distribuirRenda;
            set
            {
                _distribuirRenda = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TipoGastoModel> TipoGastos
        {
            get => _tipoGastos;
            set
            {
                _tipoGastos = value;
                OnPropertyChanged();
            }
        }

    public ICommand DeleteCommand { get; private set; } 
	public DistribuirRendaPage()
	{
		InitializeComponent();
            _databaseService = new DatabaseService();
            DistribuirRenda = new ObservableCollection<DistribuirRendaModel>();
            TipoGastos = new ObservableCollection<TipoGastoModel>();
            DeleteCommand = new Command<DistribuirRendaModel>(async (distribuirRenda) => await DeleteDistribuirRenda(distribuirRenda));
            BindingContext = this;
            InitializeDataAsync();
	}
	private async Task InitializeDataAsync()
    {
       try
            {
                var tipoGastos = await _databaseService.ListarTipoGastosAsync();
                TipoGastos.Clear();

                foreach (var type in tipoGastos)
                {
                    TipoGastos.Add(type);
                }
                listGasto.ItemsSource = TipoGastos;

                var distribuir = await _databaseService.ListarDistribuirRendaAsync();
                DistribuirRenda.Clear();
                foreach (var dist in distribuir)
                {
                    var tipoGasto = TipoGastos.FirstOrDefault(t => t.Id == dist.TipoGastoId);
                    dist.TipoGastoDescricao = tipoGasto?.Descricao ?? "Desconhecido";
                    DistribuirRenda.Add(dist);
                }

                UpdateTotalPercentage();
                UpdateCategoryPicker();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Falha ao carregar dados. Tente novamente.", "OK");
            }
    }
	private void UpdateCategoryPicker()
    {
        listGasto.ItemsSource = TipoGastos
            .Where(c => !_distribuirRenda.Any(d => d.TipoGastoId == c.Id))
            .ToList();
            listGasto.SelectedItem = null;
            AddUpdateButton.Text = "Adicionar";
            PercentageEntry.Text = string.Empty;
            _selectDistribuirRenda = null;
    }
	private void UpdateTotalPercentage()
        {
            int total = DistribuirRenda.Sum(d => d.Porcentagem);
            TotalPercentageLabel.Text = $"Total: {total}%";
        }

        private void OnCategorySelected(object sender, EventArgs e)
        {
            UpdateAddButtonState();
        }

        private void OnPercentageTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAddButtonState();
        }
        private void UpdateAddButtonState()
        {
            bool isValid = listGasto.SelectedItem != null && !string.IsNullOrEmpty(PercentageEntry.Text) && int.TryParse(PercentageEntry.Text, out int percentage) && percentage > 0;
            AddUpdateButton.IsEnabled = isValid;
        }
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            try
            {
                foreach (var distribution in DistribuirRenda)
                {
                    if (distribution.Id == 0)
                    {
                        await _databaseService.SalvarDistribuirRendaAsync(distribution);
                    }
                    else
                    {
                        await _databaseService.SalvarDistribuirRendaAsync(distribution);
                    }
                }

                await DisplayAlert("Sucesso", "Distribuições salvas com sucesso!", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Falha ao salvar as distribuições.", "OK");
            }
        }
private async void OnAddUpdateClicked(object sender, EventArgs e)
        {
            try
            {
                if (listGasto.SelectedItem is not TipoGastoModel selectedType)
                {
                    await DisplayAlert("Erro", "Selecione um tipo de gasto.", "OK");
                    return;
                }

                if (!int.TryParse(PercentageEntry.Text, out int percentage) || percentage <= 0 || percentage > 100)
                {
                    await DisplayAlert("Erro", "A porcentagem deve ser um número entre 1 e 100.", "OK");
                    return;
                }

                int currentTotal = DistribuirRenda.Where(d => d != _selectDistribuirRenda).Sum(d => d.Porcentagem);
                if (currentTotal + percentage > 100)
                {
                    await DisplayAlert("Erro", "A soma das porcentagens não pode exceder 100%.", "OK");
                    return;
                }

                var distribution = _selectDistribuirRenda ?? new DistribuirRendaModel();
                distribution.TipoGastoId = selectedType.Id;
                distribution.Porcentagem = percentage;
                distribution.TipoGastoDescricao = selectedType.Descricao;

                if (_selectDistribuirRenda == null)
                {
                    // Adicionar nova distribuição
                    DistribuirRenda.Add(distribution);
                }

                UpdateTotalPercentage();
                UpdateCategoryPicker();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Falha ao processar a distribuição.", "OK");
            }
        }
    private async Task DeleteDistribuirRenda(DistribuirRendaModel distribution)
        {
            try
            {
                if (distribution == null)
                {
                    return;
                }

                bool confirm = await DisplayAlert("Confirmação", $"Deseja excluir a distribuição para '{distribution.TipoGastoDescricao}'?", "Sim", "Não");
                if (!confirm)
                {
                    return;
                }

                DistribuirRenda.Remove(distribution);
                if (distribution.Id != 0)
                {
                    await _databaseService.ExcluiristribuirRendaAsync(distribution);
                }

                UpdateTotalPercentage();
                UpdateCategoryPicker();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Falha ao excluir a distribuição.", "OK");
            }
        }
    private async void OnItemDoubleTapped(object sender, EventArgs e)
        {
            try
            {
                var label = sender as Label;
                var distribution = label?.BindingContext as DistribuirRendaModel;
                if (distribution == null)
                {
                    return;
                }

                _selectDistribuirRenda = distribution;
                listGasto.ItemsSource = TipoGastos;
                listGasto.SelectedItem = TipoGastos.FirstOrDefault(t => t.Id == distribution.TipoGastoId);
                PercentageEntry.Text = distribution.Porcentagem.ToString();
                AddUpdateButton.Text = "Atualizar";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", "Falha ao carregar a distribuição para edição.", "OK");
            }
        }
}
