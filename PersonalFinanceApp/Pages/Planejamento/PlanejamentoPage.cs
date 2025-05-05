namespace PersonalFinanceApp;

public partial class PlanejamentoPage : ContentPage
{
	public PlanejamentoPage()
	{
		InitializeComponent();
	}

    private void TituloCustom_AdicionarClicked(object sender, EventArgs e)
    {
		AppShell.Current.Navigation.PushModalAsync(new PlanejamentoAddPage());
    }
}