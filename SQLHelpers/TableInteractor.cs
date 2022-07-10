using System.Data;
using System.Data.SqlClient;

namespace SQLHelpers
{
    public class TableInteractor
    {
        public static DataTable Fetch(QueryBuilder query, string connectionString) => Fetch(query.Query, connectionString);

        public static DataTable Fetch(string query, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                var adapter = new SqlDataAdapter(command);
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }
        
        public static int Execute(QueryBuilder query, string connectionString) => Execute(query.Query, connectionString);
        public static int Execute(string query, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                return command.ExecuteNonQuery();
            }
        }
    }
}
