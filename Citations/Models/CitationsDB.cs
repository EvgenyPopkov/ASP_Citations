using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Citations.Models
{
    public class CitationsDB
    {
        private static String connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Citations;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";
        private static SqlConnection connection;
        private static SqlCommand command;

        //получение соединения с БД
        public static SqlConnection GetConnection()
        {
            connection = new SqlConnection(connectionString);
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();
            return connection;
        }

        //закрытие соединения с БД
        public static void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }

        //извлечение объекта чтения данных, просмотр результатов оператора SELECT
        public static SqlDataReader Get(String query, List<SqlParameter> parameters = null)
        {
            command = new SqlCommand(query, GetConnection());

            if (parameters != null)
                foreach (SqlParameter p in parameters)
                    command.Parameters.Add(p);

            return command.ExecuteReader();
        }

        //модификация данных (INSERT, UPDATE, DELETE)
        public static int Execute(String query, List<SqlParameter> parameters = null)
        {
            command = new SqlCommand(query, GetConnection());

            if (parameters != null)
                foreach (SqlParameter p in parameters)
                    command.Parameters.Add(p);

            return command.ExecuteNonQuery();
        }
    }
}
