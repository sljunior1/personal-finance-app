namespace PersonalFinanceApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		InitializeRoutes();
	}
	private void InitializeRoutes()
        {
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
			Routing.RegisterRoute(nameof(AjudaPage), typeof(AjudaPage));
            Routing.RegisterRoute(nameof(ReceitaPage), typeof(ReceitaPage));
            Routing.RegisterRoute(nameof(ReceitaAddPage), typeof(ReceitaAddPage));
            Routing.RegisterRoute(nameof(ReceitaEditarPage), typeof(ReceitaEditarPage));
            Routing.RegisterRoute(nameof(PlanejamentoAddPage), typeof(PlanejamentoAddPage));
			Routing.RegisterRoute(nameof(PlanejamentoEditarPage), typeof(PlanejamentoEditarPage));
			Routing.RegisterRoute(nameof(PlanejamentoPage), typeof(PlanejamentoPage));
			Routing.RegisterRoute(nameof(DespesaAddPage), typeof(DespesaAddPage));
			Routing.RegisterRoute(nameof(DespesaEditarPage), typeof(DespesaEditarPage));
			Routing.RegisterRoute(nameof(DespesaPage), typeof(DespesaPage));
			Routing.RegisterRoute(nameof(MetaAddPage), typeof(MetaAddPage));
			Routing.RegisterRoute(nameof(MetaEditarPage), typeof(MetaEditarPage));
			Routing.RegisterRoute(nameof(MetaPage), typeof(MetaPage));
			Routing.RegisterRoute(nameof(CartaoCreditoAddPage), typeof(CartaoCreditoAddPage));
			Routing.RegisterRoute(nameof(CartaoCreditoEditarPage), typeof(CartaoCreditoEditarPage));
			Routing.RegisterRoute(nameof(CartaoCreditoPage), typeof(CartaoCreditoPage));
			Routing.RegisterRoute(nameof(CategoriaAddPage), typeof(CategoriaAddPage));
			Routing.RegisterRoute(nameof(CategoriaEditarPage), typeof(CategoriaEditarPage));
			Routing.RegisterRoute(nameof(CategoriaPage), typeof(CategoriaPage));
			Routing.RegisterRoute(nameof(ContaBancariaAddPage), typeof(ContaBancariaAddPage));
			Routing.RegisterRoute(nameof(ContaBancariaEditarPage), typeof(ContaBancariaEditarPage));
			Routing.RegisterRoute(nameof(ContaBancariaPage), typeof(ContaBancariaPage));
			Routing.RegisterRoute(nameof(DistribuirRendaPage), typeof(DistribuirRendaPage));
			Routing.RegisterRoute(nameof(TipoGanhoAddPage), typeof(TipoGanhoAddPage));
			Routing.RegisterRoute(nameof(TipoGanhoEditarPage), typeof(TipoGanhoEditarPage));
			Routing.RegisterRoute(nameof(TipoGanhoPage), typeof(TipoGanhoPage));
			Routing.RegisterRoute(nameof(TipoGastoAddPage), typeof(TipoGastoAddPage));
			Routing.RegisterRoute(nameof(TipoGastoEditarPage), typeof(TipoGastoEditarPage));
			Routing.RegisterRoute(nameof(TipoGastoPage), typeof(TipoGastoPage));
			Routing.RegisterRoute(nameof(ConfiguracaoPage), typeof(ConfiguracaoPage));
        }
}
