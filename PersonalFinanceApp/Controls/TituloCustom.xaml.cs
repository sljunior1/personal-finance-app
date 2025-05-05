namespace PersonalFinanceApp.Controls;

public partial class TituloCustom : ContentView
{
    public event EventHandler AdicionarClicked;
	public static readonly BindableProperty TituloProperty =
    BindableProperty.Create(nameof(Titulo), typeof(string), typeof(string), string.Empty);
    public static readonly BindableProperty IsVisibleConfigProperty =
    BindableProperty.Create(nameof(IsVisibleConfiguracao), typeof(bool), typeof(bool), true);
    public static readonly BindableProperty IsVisibleAjudaProperty =
    BindableProperty.Create(nameof(IsVisibleAjuda), typeof(bool), typeof(bool), false);

    public static readonly BindableProperty IsVisibleAdicionarProperty =
    BindableProperty.Create(nameof(IsVisibleAdicionar), typeof(bool), typeof(bool), false);
	public string Titulo
    {
        get => (string)GetValue(TituloProperty);
        set => SetValue(TituloProperty, value);
    }
    public bool IsVisibleConfiguracao
    {
        get => (bool)GetValue(IsVisibleConfigProperty);
        set => SetValue(IsVisibleConfigProperty, value);
    }
    public bool IsVisibleAjuda
    {
        get => (bool)GetValue(IsVisibleAjudaProperty);
        set => SetValue(IsVisibleAjudaProperty, value);
    }
    public bool IsVisibleAdicionar
    {
        get => (bool)GetValue(IsVisibleAdicionarProperty);
        set => SetValue(IsVisibleAdicionarProperty, value);
    }
	public TituloCustom()
	{
		InitializeComponent();
		BindingContext = this;
	}

    private async void ImageButtonConfig_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.Navigation.PushAsync(new ConfiguracaoPage());
    }
    private async void ImageButtonAjuda_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.Navigation.PushModalAsync(new AjudaPage());
    }
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if(AppShell.Current.Navigation.NavigationStack.Count > 1 ||
           AppShell.Current.Navigation.ModalStack.Count >= 1)
        {
            await AppShell.Current.Navigation.PopAsync();
        }
    }
    private void ImageButtonAdicionar_Clicked(object sender, EventArgs e)
    {
        AdicionarClicked?.Invoke(this, EventArgs.Empty);
    }
}