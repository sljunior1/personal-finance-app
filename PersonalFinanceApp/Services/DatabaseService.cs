using System;
using PersonalFinanceApp.Models;
using SQLite;

namespace PersonalFinanceApp.Services;

public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PersonalFinance.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            InitializeDatabase();
        }

        private async void InitializeDatabase()
        {
            await _database.CreateTableAsync<CategoriaModel>();
            await _database.CreateTableAsync<TipoGastoModel>();
            await _database.CreateTableAsync<TipoGanhoModel>();
            await _database.CreateTableAsync<CartaoCreditoModel>();
            await _database.CreateTableAsync<ContaBancariaModel>();
            await _database.CreateTableAsync<DistribuirRendaModel>();
        }

        public Task<List<CategoriaModel>> ListarCategoriasAsync() => _database.Table<CategoriaModel>().ToListAsync();
        public Task<CategoriaModel> ListarCategoriaPorIdAsync(int id) => _database.Table<CategoriaModel>().Where(i => i.Id == id).FirstOrDefaultAsync();
        public Task<int> SalvarCategoriaAsync(CategoriaModel CategoriaModel) => CategoriaModel.Id > 0 ? _database.UpdateAsync(CategoriaModel) : _database.InsertAsync(CategoriaModel);
        public Task<int> ExcluirCategoriaAsync(CategoriaModel CategoriaModel) => _database.DeleteAsync(CategoriaModel);

        public Task<List<TipoGastoModel>> ListarTipoGastosAsync() => _database.Table<TipoGastoModel>().ToListAsync(); 
        public Task<TipoGastoModel> TipoGastoPorIdAsync(int id) => _database.Table<TipoGastoModel>().Where(i => i.Id == id).FirstOrDefaultAsync();
        public Task<int> SalvarTipoGastoAsync(TipoGastoModel TipoGastoModel) => TipoGastoModel.Id > 0 ? _database.UpdateAsync(TipoGastoModel) : _database.InsertAsync(TipoGastoModel);
        public Task<int> ExcluirTipoGastoAsync(TipoGastoModel TipoGastoModel) => _database.DeleteAsync(TipoGastoModel);

        public Task<List<TipoGanhoModel>> ListarTipoGanhosAsync() => _database.Table<TipoGanhoModel>().ToListAsync();
        public Task<TipoGanhoModel> ListarTipoGanhoPorIdAsync(int id) => _database.Table<TipoGanhoModel>().Where(i => i.Id == id).FirstOrDefaultAsync();
        public Task<int> SalvarTipoGanhoAsync(TipoGanhoModel TipoGanhoModel) => TipoGanhoModel.Id > 0 ? _database.UpdateAsync(TipoGanhoModel) : _database.InsertAsync(TipoGanhoModel);
        public Task<int> ExcluirTipoGanhoAsync(TipoGanhoModel TipoGanhoModel) => _database.DeleteAsync(TipoGanhoModel);

        public Task<List<CartaoCreditoModel>> ListarCartoesCreditoAsync() => _database.Table<CartaoCreditoModel>().ToListAsync();
        public Task<CartaoCreditoModel> ListarCartaoCreditoPorIdAsync(int id) => _database.Table<CartaoCreditoModel>().Where(i => i.Id == id).FirstOrDefaultAsync();
        public Task<int> SalvarCartaoCreditoAsync(CartaoCreditoModel CartaoCreditoModel) => CartaoCreditoModel.Id > 0 ? _database.UpdateAsync(CartaoCreditoModel) : _database.InsertAsync(CartaoCreditoModel);
        public Task<int> ExcluirCartaoCreditoAsync(CartaoCreditoModel CartaoCreditoModel) => _database.DeleteAsync(CartaoCreditoModel);


        public Task<List<ContaBancariaModel>> ListarContasBancariaAsync() => _database.Table<ContaBancariaModel>().ToListAsync();
        public Task<ContaBancariaModel> ListarContaBancariaPorIdAsync(int id) => _database.Table<ContaBancariaModel>().Where(i => i.Id == id).FirstOrDefaultAsync();
        public Task<int> SalvarContaBancariaAsync(ContaBancariaModel ContaBancariaModel) => ContaBancariaModel.Id > 0 ? _database.UpdateAsync(ContaBancariaModel) : _database.InsertAsync(ContaBancariaModel);
        public Task<int> ExcluirContaBancariaAsync(ContaBancariaModel ContaBancariaModel) => _database.DeleteAsync(ContaBancariaModel);

        public Task<List<DistribuirRendaModel>> ListarDistribuirRendaAsync() => _database.Table<DistribuirRendaModel>().ToListAsync();
        public Task<DistribuirRendaModel> ListarDistribuirRendaPorIdAsync(int id) => _database.Table<DistribuirRendaModel>().Where(i => i.Id == id).FirstOrDefaultAsync();
        public Task<int> SalvarDistribuirRendaAsync(DistribuirRendaModel distribution) => distribution.Id > 0 ? _database.UpdateAsync(distribution) : _database.InsertAsync(distribution);
        public Task<int> ExcluiristribuirRendaAsync(DistribuirRendaModel distribution) => _database.DeleteAsync(distribution);
    }
