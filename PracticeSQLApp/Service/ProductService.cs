using PracticeSQLApp.Model;
using System.Data.SqlClient;

namespace PracticeSQLApp.Service
{
    public class ProductService : IProductService
    {
        //private static string db_source= "projectappserver.database.windows.net";
        //private static string db_user = "sqladmin";
        //private static string db_password = "Pa55word";
        //private static string db_database_name = "appdb";
        private readonly IConfiguration _configuration;

        public ProductService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("SQLConnection"));

            //var _builder = new SqlConnectionStringBuilder();
            //_builder.DataSource = db_source;
            //_builder.UserID = db_user;
            //_builder.Password = db_password;
            //_builder.InitialCatalog = db_database_name;
            //return new SqlConnection(_builder.ConnectionString);
        }

        public List<Product> GetProducts()
        {
            List<Product> _products_lst = new List<Product>();
            string _statement = "SELECT ProductID,ProductName,Quantity from Products";
            SqlConnection _sqlConnection = GetConnection();
            _sqlConnection.Open();

            SqlCommand _sqlCommand = new SqlCommand(_statement, _sqlConnection);

            using (SqlDataReader _reader = _sqlCommand.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Product _product = new Product()
                    {
                        ProductID = _reader.GetInt32(0),
                        ProductName = _reader.GetString(1),
                        Quantity = _reader.GetInt32(2)
                    };
                    _products_lst.Add(_product);
                }

            }
            return _products_lst;
        }

    }
}
