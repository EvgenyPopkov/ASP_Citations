using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Citations.Models
{
    public class Category
    {
        public int Id { get; set; }
        public String Name { get; set; }
    
        public Category()
        {
           
        }

        public Category(SqlDataReader reader)
        {
            Id = (int)reader["Id"];
            Name = (String)reader["Name"];
        }

        //получение все категорий
        public static List<Category> Read()
        {
            String query = "SELECT * FROM Category";
            List<Category> categories = new List<Category>();
            SqlDataReader reader = CitationsDB.Get(query);

            if (reader != null && reader.HasRows)
                //чтение категорий из БД по записям
                while (reader.Read())
                {
                    Category category = new Category(reader);
                    categories.Add(category);
                }

            CitationsDB.CloseConnection();
            return categories;
        }

        //получение одной кактегории
        public static Category GetCategory(int id)
        {
            String query = "SELECT Name FROM Category WHERE Id = @id";
            SqlDataReader reader = CitationsDB.Get(query, new List<SqlParameter>() { new SqlParameter("id", id) });
            Category category = new Category();

            if (reader != null && reader.HasRows)
                while (reader.Read())
                {
                    category.Id = id;
                    category.Name = (String)reader["Name"];
                }
                    

            CitationsDB.CloseConnection();
            return category;
        }

        //получение id категории по названию
        public static int GetIdByName(string name)
        {
            String query = "SELECT Id FROM Category WHERE Name = @name";
            SqlDataReader reader = CitationsDB.Get(query, new List<SqlParameter>() { new SqlParameter("name", name) });
            int id = 0;

            if (reader != null && reader.HasRows)
                while (reader.Read())
                {
                    id = (int)reader["Id"];
                }


            CitationsDB.CloseConnection();
            return id;
        }
    }

}
