namespace PersonalFinanceApp;

public partial class FormaPagamentoPage : ContentPage
{
	public FormaPagamentoPage()
	{
		InitializeComponent();
	}
	private void TituloCustom_AdicionarClicked(object sender, EventArgs e)
    {
		AppShell.Current.Navigation.PushModalAsync(new FormaPagamentoAddPage());
    }
}