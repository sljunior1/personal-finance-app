using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace PersonalFinanceApp
{
    public class DistributionItem
    {
        public string Category { get; set; }
        public int Percentage { get; set; }
    }

    public partial class DistribuirRendaPage : ContentPage
    {
        private bool _isFormatting;
        private ObservableCollection<DistributionItem> _distributions;
        private List<string> _categories;
        private DistributionItem _editingItem;
        public ICommand DeleteCommand { get; private set; }

        public DistribuirRendaPage()
        {
            InitializeComponent();
            DeleteCommand = new Command<DistributionItem>(OnDelete);
            BindingContext = this;
            InitializeData();
        }

        private void InitializeData()
        {
            _categories = new List<string>
            {
                "Alimentação",
                "Transporte",
                "Moradia",
                "Lazer",
                "Saúde"
            };

            _distributions = new ObservableCollection<DistributionItem>();
            DistributionList.ItemsSource = _distributions;
            _editingItem = null;
            AtualizarCategoria();
            AtualizarTotalPorcentagem();
        }

        private void AtualizarCategoria()
        {
            var availableCategories = _categories
                .Where(c => !_distributions.Any(d => d.Category == c) || (_editingItem != null && c == _editingItem.Category))
                .ToList();
            listGasto.ItemsSource = availableCategories;
            listGasto.SelectedIndex = -1;
            if (_editingItem != null)
            {
                listGasto.SelectedItem = _editingItem.Category;
                PercentageEntry.Text = $"{_editingItem.Percentage}%";
                AddUpdateButton.Text = "Atualizar";
            }
            else
            {
                PercentageEntry.Text = string.Empty;
                AddUpdateButton.Text = "Adicionar";
            }
        }

        private void AtualizarTotalPorcentagem()
        {
            int total = _distributions.Sum(d => d.Percentage);
            TotalPercentageLabel.Text = $"Total: {total}%";
        }

        private void OnCategorySelected(object sender, EventArgs e)
        {
            if (_editingItem == null)
            {
                PercentageEntry.Text = string.Empty;
            }
        }

        private void OnPercentageTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isFormatting)
                return;

            _isFormatting = true;

            var entry = sender as Entry;
            string text = Regex.Replace(entry.Text, "[^0-9]", "");

            if (string.IsNullOrEmpty(text))
            {
                entry.Text = string.Empty;
                _isFormatting = false;
                return;
            }

            if (int.TryParse(text, out int value))
            {
                if (value > 100)
                    value = 100;
                entry.Text = $"{value}%";
            }
            else
            {
                entry.Text = string.Empty;
            }

            _isFormatting = false;
        }

        private async void OnAddUpdateClicked(object sender, EventArgs e)
        {
            string category = listGasto.SelectedItem?.ToString();
            string percentageText = Regex.Replace(PercentageEntry.Text ?? "", "[^0-9]", "");

            if (string.IsNullOrEmpty(category))
            {
                await DisplayAlert("Erro", "Por favor, selecione um tipo de gasto.", "OK");
                return;
            }

            if (string.IsNullOrEmpty(percentageText) || !int.TryParse(percentageText, out int percentage) || percentage <= 0)
            {
                await DisplayAlert("Erro", "Por favor, insira uma porcentagem válida (1-100).", "OK");
                return;
            }

            int currentTotal = _distributions.Where(d => d != _editingItem).Sum(d => d.Percentage);
            if (currentTotal + percentage > 100)
            {
                await DisplayAlert("Erro", "A soma das porcentagens não pode exceder 100%.", "OK");
                return;
            }

            if (_editingItem != null)
            {
                _distributions.Remove(_editingItem);
                _distributions.Add(new DistributionItem
                {
                    Category = category,
                    Percentage = percentage
                });
                _editingItem = null;
            }
            else
            {
                _distributions.Add(new DistributionItem
                {
                    Category = category,
                    Percentage = percentage
                });
            }

            AtualizarCategoria();
            AtualizarTotalPorcentagem();
        }

        private void OnItemDoubleTapped(object sender, EventArgs e)
        {
            var label = sender as Label;
            _editingItem = label?.BindingContext as DistributionItem;
            AtualizarCategoria();
        }

        private void OnDelete(DistributionItem item)
        {
            if (item != null)
            {
                _distributions.Remove(item);
                if (_editingItem == item)
                {
                    _editingItem = null;
                }
                AtualizarCategoria();
                AtualizarTotalPorcentagem();
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            if (!_distributions.Any())
            {
                await DisplayAlert("Erro", "Adicione pelo menos uma distribuição antes de salvar.", "OK");
                return;
            }

            int total = _distributions.Sum(d => d.Percentage);
            if (total < 100)
            {
                await DisplayAlert("Erro", "A soma das porcentagens deve ser igual a 100%.", "OK");
                return;
            }

            await DisplayAlert("Sucesso", "Distribuição de renda cadastrada com sucesso!", "OK");

            _distributions.Clear();
            _editingItem = null;
            AtualizarCategoria();
            AtualizarTotalPorcentagem();
        }
    }
}