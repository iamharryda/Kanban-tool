using MySql.Data.MySqlClient;

namespace AgileProjectManager.Model.DataAccess
{
    public class DbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory()
        {
            _connectionString = "server=localhost;port=3306;database=agile_project_manager;uid=root;pwd=;";
        }

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}