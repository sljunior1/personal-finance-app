namespace PersonalFinanceApp;

public partial class TipoGastoPage : ContentPage
{
	public TipoGastoPage()
	{
		InitializeComponent();
	}
	private void TituloCustom_AdicionarClicked(object sender, EventArgs e)
    {
		AppShell.Current.Navigation.PushModalAsync(new TipoGastoAddPage());
    }
}