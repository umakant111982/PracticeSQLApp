using PracticeSQLApp.Model;

namespace PracticeSQLApp.Service
{
    public interface IProductService
    {
        Task<List<Product>> GetProducts();
        Task<bool> IsBeta();
    }
}