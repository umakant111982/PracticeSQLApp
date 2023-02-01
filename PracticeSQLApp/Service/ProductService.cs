using Microsoft.FeatureManagement;
using PracticeSQLApp.Model;
using System.Data.SqlClient;
using System.Text.Json;

namespace PracticeSQLApp.Service
{
    public class ProductService : IProductService
    {
        //private static string db_source= "projectappserver.database.windows.net";
        //private static string db_user = "sqladmin";
        //private static string db_password = "Pa55word";
        //private static string db_database_name = "appdb";
        private readonly IConfiguration _configuration;
        private readonly IFeatureManager _ifeatureManager;
        public ProductService(IConfiguration configuration, IFeatureManager featureManager)
        {
            _configuration = configuration;
            _ifeatureManager = featureManager;
        }

        public async Task<bool> IsBeta()
        {
            if (await _ifeatureManager.IsEnabledAsync("beta"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private SqlConnection GetConnection()
        {
            //return new SqlConnection(_configuration.GetConnectionString("SQLConnection"));
            return new SqlConnection(_configuration["SQLConnection"]);

            //var _builder = new SqlConnectionStringBuilder();
            //_builder.DataSource = db_source;
            //_builder.UserID = db_user;
            //_builder.Password = db_password;
            //_builder.InitialCatalog = db_database_name;
            //return new SqlConnection(_builder.ConnectionString);
        }

        //public List<Product> GetProducts()
        //{
        //    List<Product> _products_lst = new List<Product>();
        //    string _statement = "SELECT ProductID,ProductName,Quantity from Products";
        //    SqlConnection _sqlConnection = GetConnection();
        //    _sqlConnection.Open();

        //    SqlCommand _sqlCommand = new SqlCommand(_statement, _sqlConnection);

        //    using (SqlDataReader _reader = _sqlCommand.ExecuteReader())
        //    {
        //        while (_reader.Read())
        //        {
        //            Product _product = new Product()
        //            {
        //                ProductID = _reader.GetInt32(0),
        //                ProductName = _reader.GetString(1),
        //                Quantity = _reader.GetInt32(2)
        //            };
        //            _products_lst.Add(_product);
        //        }

        //    }
        //    return _products_lst;
        //}

        public async Task<List<Product>> GetProducts()
        {
            string functionURL = "https://azurefunctionapp33.azurewebsites.net/api/GetProducts?code=MY4iCVX_Me-DQa2tDdPJsBGziuDNEYWVt3myNIJfHNsuAzFurIdlJg==";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync(functionURL);

                string content = await responseMessage.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<List<Product>>(content);
            }

        }

    }
}
