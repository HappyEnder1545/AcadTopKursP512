using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using PersonalLibrary.Models;

namespace PersonalLibrary.Data
{
    public class AuthorRepository
    {
        public List<Author> GetAll()
        {
            var list = new List<Author>();
            using var c = Database.GetConnection();
            using var cmd = new SqlCommand("SELECT Id, FirstName, LastName FROM Author ORDER BY LastName, FirstName", c);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                list.Add(new Author
                {
                    Id = r.GetInt32(0),
                    FirstName = r.GetString(1),
                    LastName = r.GetString(2)
                });
            return list;
        }

        public void Add(Author a)
        {
            using var c = Database.GetConnection();
            using var cmd = new SqlCommand("INSERT INTO Author (FirstName, LastName) VALUES (@f, @l)", c);

            cmd.Parameters.AddWithValue("@f", a.FirstName);
            cmd.Parameters.AddWithValue("@l", a.LastName);
            
            cmd.ExecuteNonQuery();
        }

        public void Update(Author a)
        {
            using var c = Database.GetConnection();
            using var cmd = new SqlCommand("UPDATE Author SET FirstName=@f, LastName=@l WHERE Id=@id", c);

            cmd.Parameters.AddWithValue("@f", a.FirstName);
            cmd.Parameters.AddWithValue("@l", a.LastName);
            cmd.Parameters.AddWithValue("@id", a.Id);
            
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var c = Database.GetConnection();
            using var cmd = new SqlCommand("DELETE FROM Author WHERE Id=@id", c);

            cmd.Parameters.AddWithValue("@id", id);
            
            cmd.ExecuteNonQuery();
        }

        public bool IsUsed(int id)
        {
            using var c = Database.GetConnection();
            using var cmd = new SqlCommand("SELECT COUNT(*) FROM Book WHERE AuthorId=@id", c);
            
            cmd.Parameters.AddWithValue("@id", id);

            return (int)cmd.ExecuteScalar() > 0;
        }
    }
}