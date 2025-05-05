namespace PersonalFinanceApp;

public partial class TipoGanhoPage : ContentPage
{
	public TipoGanhoPage()
	{
		InitializeComponent();
	}
	private void TituloCustom_AdicionarClicked(object sender, EventArgs e)
    {
		AppShell.Current.Navigation.PushModalAsync(new TipoGanhoAddPage());
    }
}