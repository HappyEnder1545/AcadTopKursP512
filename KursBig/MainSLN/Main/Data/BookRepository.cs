using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using PersonalLibrary.Models;

namespace PersonalLibrary.Data
{
    public class BookRepository
    {
        public List<Book> GetAll(string? titleSearch = null, int? genreId = null, BookStatus? status = null)
        {
            var list = new List<Book>();
            var sql = new StringBuilder(@"
            SELECT b.Id, b.Title, b.AuthorId, b.GenreId, b.Year, b.Pages, b.Status, b.Notes,
                   a.FirstName + ' ' + a.LastName AS AuthorName, g.Name AS GenreName
            FROM Book b
            JOIN Author a ON a.Id = b.AuthorId
            JOIN Genre  g ON g.Id = b.GenreId
            WHERE 1=1");

            if (!string.IsNullOrWhiteSpace(titleSearch))
                sql.Append(" AND b.Title LIKE @search");
            if (genreId.HasValue && genreId.Value > 0)
                sql.Append(" AND b.GenreId = @genre");
            if (status.HasValue)
                sql.Append(" AND b.Status = @status");

            sql.Append(" ORDER BY b.Title");

            using var c = Database.GetConnection();
            using var cmd = new SqlCommand(sql.ToString(), c);

            if (!string.IsNullOrWhiteSpace(titleSearch))
                cmd.Parameters.AddWithValue("@search", "%" + titleSearch + "%");
            if (genreId.HasValue && genreId.Value > 0)
                cmd.Parameters.AddWithValue("@genre", genreId.Value);
            if (status.HasValue)
                cmd.Parameters.AddWithValue("@status", status.Value.ToString());

            using var r = cmd.ExecuteReader();
            while (r.Read())
            {
                list.Add(new Book
                {
                    Id = r.GetInt32(0),
                    Title = r.GetString(1),
                    AuthorId = r.GetInt32(2),
                    GenreId = r.GetInt32(3),
                    Year = r.IsDBNull(4) ? (int?)null : r.GetInt32(4),
                    Pages = r.IsDBNull(5) ? (int?)null : r.GetInt32(5),
                    Status = BookStatusHelper.Parse(r.GetString(6)),
                    Notes = r.IsDBNull(7) ? null : r.GetString(7),
                    AuthorName = r.GetString(8),
                    GenreName = r.GetString(9)
                });
            }
            return list;
        }

        public void Add(Book b)
        {
            using var c = Database.GetConnection();
            using var cmd = new SqlCommand(@"
            INSERT INTO Book (Title, AuthorId, GenreId, Year, Pages, Status, Notes) VALUES (@t, @a, @g, @y, @p, @s, @n)", c);
            FillParams(cmd, b);
            cmd.ExecuteNonQuery();
        }

        public void Update(Book b)
        {
            using var c = Database.GetConnection();
            using var cmd = new SqlCommand(@"
            UPDATE Book SET Title=@t, AuthorId=@a, GenreId=@g, Year=@y, Pages=@p, Status=@s, Notes=@n WHERE Id=@id", c);
            FillParams(cmd, b);
            cmd.Parameters.AddWithValue("@id", b.Id);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var c = Database.GetConnection();
            using var cmd = new SqlCommand("DELETE FROM Book WHERE Id=@id", c);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        private static void FillParams(SqlCommand cmd, Book b)
        {
            cmd.Parameters.AddWithValue("@t", b.Title);
            cmd.Parameters.AddWithValue("@a", b.AuthorId);
            cmd.Parameters.AddWithValue("@g", b.GenreId);
            cmd.Parameters.AddWithValue("@y", (object?)b.Year ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@p", (object?)b.Pages ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@s", b.Status.ToString());
            cmd.Parameters.AddWithValue("@n",
                string.IsNullOrWhiteSpace(b.Notes) ? System.DBNull.Value : b.Notes);
        }

        // подписать что тут

        public int GetTotalCount()
        {
            using var c = Database.GetConnection();
            using var cmd = new SqlCommand("SELECT COUNT(*) FROM Book", c);
            return (int)cmd.ExecuteScalar();
        }

        public Dictionary<BookStatus, int> GetCountByStatus()
        {
            var dict = new Dictionary<BookStatus, int>
            {
                [BookStatus.NotRead] = 0,
                [BookStatus.Reading] = 0,
                [BookStatus.Read] = 0
            };
            using var c = Database.GetConnection();
            using var cmd = new SqlCommand(
                "SELECT Status, COUNT(*) FROM Book GROUP BY Status", c);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                dict[BookStatusHelper.Parse(r.GetString(0))] = r.GetInt32(1);
            return dict;
        }

        public List<(string Genre, int Count)> GetTopGenres(int top = 3)
        {
            var list = new List<(string, int)>();
            using var c = Database.GetConnection();
            using var cmd = new SqlCommand($@"
            SELECT TOP {top} g.Name, COUNT(*) AS Cnt FROM Book b JOIN Genre g ON g.Id = b.GenreId GROUP BY g.Name ORDER BY Cnt DESC", c);
            using var r = cmd.ExecuteReader();
            while (r.Read())
                list.Add((r.GetString(0), r.GetInt32(1)));
            return list;
        }
    }
}