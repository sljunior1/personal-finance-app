namespace PersonalFinanceApp;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}
	protected override void OnAppearing()
    {
        base.OnAppearing();
		AtualizarStatusFinanceiro("verde");

    }
	private void AtualizarStatusFinanceiro(string status)
    {
        switch (status.ToLower())
        {
            case "verde":
				panelSaudeFinanceira.BackgroundColor= Color.FromArgb("#00CA4E");
				lblSaudeFinanceira.Text = "A sua saude financeira está ótima! Veja mais!";
                break;
            case "amarelo":
				panelSaudeFinanceira.BackgroundColor= Color.FromArgb("#FFBD44");
				lblSaudeFinanceira.Text = "A sua saude financeira precisa de atenção! Veja mais!";
                break;
            case "vermelho":
				panelSaudeFinanceira.BackgroundColor= Color.FromArgb("#FF605C");
				lblSaudeFinanceira.Text = "A sua saude financeira não está bem! Veja mais!";
                break;
        }
    }

    private void SaudeFinanceiraTapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
    }
}

