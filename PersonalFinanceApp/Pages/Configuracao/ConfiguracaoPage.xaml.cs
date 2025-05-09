namespace PersonalFinanceApp;

public partial class ConfiguracaoPage : ContentPage
{
	public ConfiguracaoPage()
	{
		InitializeComponent();
		MenuListView.ItemsSource = new List<MenuItemModel>
{
    new MenuItemModel { Titulo = "Categorias", Icon = "ic_categoria.png", TipoPage = typeof(CategoriaPage) },
    new MenuItemModel { Titulo = "Tipos de Gastos", Icon = "ic_gastos.png", TipoPage = typeof(TipoGastoPage)  },
	new MenuItemModel { Titulo = "Tipos de Ganhos", Icon = "ic_ganhos.png", TipoPage = typeof(TipoGanhoPage)  },
	new MenuItemModel { Titulo = "Cartões de Crédito", Icon = "ic_cartao.png", TipoPage = typeof(CartaoCreditoPage)  },
	new MenuItemModel { Titulo = "Contas Bancárias", Icon = "ic_conta.png" , TipoPage = typeof(ContaBancariaPage)  },
    new MenuItemModel { Titulo = "Distribuição da renda", Icon = "ic_divisao.png" , TipoPage = typeof(DistribuirRendaPage)  }
};
	MenuListView.ItemTapped += async (s, e) =>
{
    if (e.Item is MenuItemModel item && item.TipoPage != null)
    {
        var page = (Page)Activator.CreateInstance(item.TipoPage);
        await Navigation.PushAsync(page);
        MenuListView.SelectedItem = null; 
    }
};
	}
}
public class MenuItemModel
{
    public string Titulo { get; set; }
	public string Icon { get; set; }
	public Type TipoPage { get; set; }
}