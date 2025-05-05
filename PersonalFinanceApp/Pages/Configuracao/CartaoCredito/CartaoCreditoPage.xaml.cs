namespace PersonalFinanceApp;

public partial class CartaoCreditoPage : ContentPage
{
	public CartaoCreditoPage()
	{
		InitializeComponent();
	}
	private void TituloCustom_AdicionarClicked(object sender, EventArgs e)
    {
		AppShell.Current.Navigation.PushModalAsync(new CartaoCreditoAddPage());
    }
}