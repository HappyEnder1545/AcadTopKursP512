using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using PersonalLibrary.Models;

namespace PersonalLibrary.Data
{
    public class GenreRepository
    {
        public List<Genre> GetAll()
        {
            var list = new List<Genre>();
            using var c = Database.GetConnection();
            using var cmd = new SqlCommand("SELECT Id, Name FROM Genre ORDER BY Name", c);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                list.Add(new Genre { Id = r.GetInt32(0), Name = r.GetString(1) });
            return list;
        }

        public void Add(Genre g)
        {
            using var c = Database.GetConnection();
            using var cmd = new SqlCommand("INSERT INTO Genre (Name) VALUES (@n)", c);
            cmd.Parameters.AddWithValue("@n", g.Name);
            cmd.ExecuteNonQuery();
        }

        public void Update(Genre g)
        {
            using var c = Database.GetConnection();
            using var cmd = new SqlCommand("UPDATE Genre SET Name=@n WHERE Id=@id", c);
            cmd.Parameters.AddWithValue("@n", g.Name);
            cmd.Parameters.AddWithValue("@id", g.Id);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var c = Database.GetConnection();
            using var cmd = new SqlCommand("DELETE FROM Genre WHERE Id=@id", c);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
        public bool IsUsed(int id)
        {
            using var c = Database.GetConnection();
            using var cmd = new SqlCommand("SELECT COUNT(*) FROM Book WHERE GenreId=@id", c);
            cmd.Parameters.AddWithValue("@id", id);
            return (int)cmd.ExecuteScalar() > 0;
        }
    }
}
