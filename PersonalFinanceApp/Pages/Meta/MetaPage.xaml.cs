namespace PersonalFinanceApp;

public partial class MetaPage : ContentPage
{
	public MetaPage()
	{
		InitializeComponent();
	}

    private void TituloCustom_AdicionarClicked(object sender, EventArgs e)
    {
		AppShell.Current.Navigation.PushModalAsync(new MetaAddPage());
    }
}