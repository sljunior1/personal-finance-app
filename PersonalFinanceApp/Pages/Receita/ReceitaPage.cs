
namespace PersonalFinanceApp;

public partial class ReceitaPage : ContentPage
{
	public ReceitaPage()
	{
		InitializeComponent();
		ReceitaListView.ItemsSource = new List<ReceitaModel>
		{
    		new ReceitaModel { Titulo = "Salário Deal",TipoRenda = TipoRendaEnum.SalarioCLT },
			new ReceitaModel { Titulo = "Salário Movisis",TipoRenda = TipoRendaEnum.SalarioPJ }
		};
	}
    private void TituloCustom_AdicionarClicked(object sender, EventArgs e)
    {
		AppShell.Current.Navigation.PushModalAsync(new ReceitaAddPage());
    }

    private void SwipeItem_Invoked(object sender, EventArgs e)
    {
    }
}

public class ReceitaModel
{
	public int Id { get; set; }
	public string Titulo { get; set; }
	public string Descricao { get; set; }
	public TipoRendaEnum TipoRenda { get; set; }
}
public enum TipoRendaEnum
{
	SalarioCLT,
	SalarioPJ,
	RentabilidadeInvestimento,
	Freelancer, 
	Tradeß
}