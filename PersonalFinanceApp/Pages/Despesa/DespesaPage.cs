namespace PersonalFinanceApp;

public partial class DespesaPage : ContentPage
{
	public DespesaPage()
	{
		InitializeComponent();
	}

    private void TituloCustom_AdicionarClicked(object sender, EventArgs e)
    {
		AppShell.Current.Navigation.PushModalAsync(new DespesaAddPage());
    }
}