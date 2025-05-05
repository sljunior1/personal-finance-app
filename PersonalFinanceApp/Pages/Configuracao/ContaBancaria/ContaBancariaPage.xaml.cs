namespace PersonalFinanceApp;

public partial class ContaBancariaPage : ContentPage
{
	public ContaBancariaPage()
	{
		InitializeComponent();
	}
		private void TituloCustom_AdicionarClicked(object sender, EventArgs e)
    {
		AppShell.Current.Navigation.PushModalAsync(new ContaBancariaAddPage());
    }
}