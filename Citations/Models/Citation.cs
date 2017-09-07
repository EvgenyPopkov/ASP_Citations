using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Citations.Models
{
    public class Citation
    {
        public int Id { get; set; }
        public String Text { get; set; }
        public String Author { get; set; }
        public Category Category { get; set; }
        public DateTime Date { get; set; }

        public Citation()
        {

        }

        public Citation(SqlDataReader reader)
        {
            Id = (int)reader["Id"];
            Text = (String)reader["Text"];
            Author = (String)reader["Author"];
            if (reader["IdCategory"] != DBNull.Value)
                Category = Category.GetCategory((int)reader["IdCategory"]);
            Date = (DateTime)reader["Date"];
        }

        //получение одной цитаты
        public static Citation GetCitation(int id)
        {
            String query = "SELECT * FROM Citation WHERE Id = @id";
            SqlDataReader reader = CitationsDB.Get(query, new List<SqlParameter>() { new SqlParameter("id", id) });
            Citation citation = new Citation();

            if (reader != null && reader.HasRows)
                while (reader.Read())
                {
                    citation.Id = (int)reader["Id"];
                    citation.Text = (String)reader["Text"];
                    citation.Author = (String)reader["Author"];
                    citation.Date = (DateTime)reader["Date"];
                    if (reader["IdCategory"] != DBNull.Value)
                        citation.Category = Category.GetCategory((int)reader["IdCategory"]);
                }

            CitationsDB.CloseConnection();
            return citation;
        }

        //поиск по текту цитаты
        public static List<Citation> SearchText(String text)
        {
            String query = @"SELECT * FROM Citation WHERE Text LIKE N'%" + text + "%'";
            SqlDataReader reader = CitationsDB.Get(query);
            List<Citation> data = new List<Citation>();

            if (reader != null && reader.HasRows)
                while (reader.Read())
                {
                    Citation citation = new Citation(reader);
                    data.Add(citation);
                }

            CitationsDB.CloseConnection();
            return data;
        }

        //поиск по автору
        public static List<Citation> SearchAuthor(String author)
        {
            String query = @"SELECT * FROM Citation WHERE Author LIKE N'%" + author + "%'";
            SqlDataReader reader = CitationsDB.Get(query);
            List<Citation> data = new List<Citation>();

            if (reader != null && reader.HasRows)
                while (reader.Read())
                {
                    Citation citation = new Citation(reader);
                    data.Add(citation);
                }

            CitationsDB.CloseConnection();
            return data;
        }

        //поиск по текту и по автору
        public static List<Citation> SearchAll(String text, String author)
        {
            String query = @"SELECT * FROM Citation WHERE Text LIKE N'%" + text + "%' AND Author LIKE N'%" + author + "%'";
            SqlDataReader reader = CitationsDB.Get(query);
            List<Citation> data = new List<Citation>();
            
            if (reader != null && reader.HasRows)
                while (reader.Read())
                {
                    Citation citation = new Citation(reader);
                    data.Add(citation);
                }

            CitationsDB.CloseConnection();
            return data;
        }

        //поиск
        public static List<Citation> Search(String text, String author)
        {
            if (text == null && author == null)
                return CitationCRUD.Read();
            else if (text == null && author != null)
                return SearchAuthor(author);
            else if (text != null && author == null)
                return SearchText(text);
            else return SearchAll(text, author);
        }
    }
}
