using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Citations.Models
{
    public class CitationCRUD
    {
        //создание цитаты
        public static void Create(Citation citation)
        {
            String query = "INSERT INTO Citation(Text, Author, IdCategory, Date) VALUES(@text, @author, @idCategory, @date)";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("text", citation.Text),
                new SqlParameter("author", citation.Author),
                new SqlParameter("date", citation.Date)
            };

            if (citation.Category.Id == 0)
                parameters.Add(new SqlParameter("idCategory", DBNull.Value));
            else
                parameters.Add(new SqlParameter("idCategory", citation.Category.Id));
      
            CitationsDB.Execute(query, parameters);
            CitationsDB.CloseConnection();
        }

        //чтение всех цитат из БД
        public static List<Citation> Read()
        {
            String query = "SELECT * FROM Citation";
            List<Citation> citations = new List<Citation>();
            SqlDataReader reader = CitationsDB.Get(query);

            if (reader != null && reader.HasRows)
                while (reader.Read())
                {
                    Citation citation = new Citation(reader);
                    citations.Add(citation);
                }

            return citations;
        }

        //изменение цитаты
        public static void Update(Citation citation)
        {
            String query = "UPDATE Citation SET Text = @text, Author = @author, IdCategory = @idCategory, Date = @date WHERE Id = @id";
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter("id", citation.Id),
                new SqlParameter("text", citation.Text),
                new SqlParameter("author", citation.Author),
                new SqlParameter("date", citation.Date)
            };

            if (citation.Category.Id == 0)
                parameters.Add(new SqlParameter("idCategory", DBNull.Value));
            else
                parameters.Add(new SqlParameter("idCategory", citation.Category.Id));

            CitationsDB.Execute(query, parameters);
            CitationsDB.CloseConnection();
        }

        //удаление
        public static void Delete(int id)
        {
            String query = "DELETE FROM Citation WHERE Id = @id";
            List<SqlParameter> parameters = new List<SqlParameter>() { new SqlParameter("Id", id) };
            CitationsDB.Execute(query, parameters);
            CitationsDB.CloseConnection();
        }
    }
}
