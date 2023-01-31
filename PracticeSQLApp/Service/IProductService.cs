using PracticeSQLApp.Model;

namespace PracticeSQLApp.Service
{
    public interface IProductService
    {
        List<Product> GetProducts();
        Task<bool> IsBeta();
    }
}