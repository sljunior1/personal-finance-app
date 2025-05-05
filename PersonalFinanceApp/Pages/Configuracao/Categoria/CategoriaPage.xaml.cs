
namespace PersonalFinanceApp;

public partial class CategoriaPage : ContentPage
{
	public CategoriaPage()
	{
		InitializeComponent();
	}
	private void TituloCustom_AdicionarClicked(object sender, EventArgs e)
    {
		AppShell.Current.Navigation.PushModalAsync(new CategoriaAddPage());
    }
}