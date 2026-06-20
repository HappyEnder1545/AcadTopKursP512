using Microsoft.Data.SqlClient;
using System;

namespace Proj
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        static void CreateGenreTbl()
        {
            string ConnString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Kurs;Integrated Security = True";
            using var conn = new SqlConnection(ConnString);
            conn.Open();

            string sql = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Genre') 
            CREATE TABLE Genre (
            Id INT PRIMARY KEY IDENTITY(1,1),
            Name VARCHAR(100) NOT NULL )";

            using var comm = new SqlCommand(sql, conn);
            comm.ExecuteNonQuery();
            Console.WriteLine("Создана таблица жанров");
        }
        static void CreateAuthorTbl()
        {
            string ConnString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Kurs;Integrated Security = True";
            using var conn = new SqlConnection(ConnString);
            conn.Open();

            string sql = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Author') 
            CREATE TABLE Author (
            Id INT PRIMARY KEY IDENTITY(1,1),
            FirstName VARCHAR(100) NOT NULL,
            LastName VARCHAR(100) NOT NULL )";

            using var comm = new SqlCommand(sql, conn);
            comm.ExecuteNonQuery();
            Console.WriteLine("Создана таблица авторов");
        }
        static void CreateBookTbl()
        {
            string ConnString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Kurs;Integrated Security = True";
            using var conn = new SqlConnection(ConnString);
            conn.Open();

            string sql = @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Book') 
            CREATE TABLE Book (
            Id INT PRIMARY KEY IDENTITY(1,1),
            Title VARCHAR(100),
            AuthorId INTEGER NOT NULL REFERENCES Author(Id),
            GenreId INTEGER NOT NULL REFERENCES Genre(Id),
            Year INTEGER,
            Pages INTEGER,
            Status VARCHAR(50) NOT NULL DEFAULT 'Not Read',
            Notes VARCHAR(100)
            )";


            using var comm = new SqlCommand(sql, conn);
            comm.ExecuteNonQuery();
            Console.WriteLine("Создана таблица книг");
        }
        static async Task CreateBookAsync()
        {

        }
        static async Task ReadBookAsync()
        {
            string connectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = Kurs;Integrated Security = True";
            string sql = "SELECT Id, Title, AuthorId, GenreId, Year, Pages, Status, Notes";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int id = reader.GetInt32(0);
                            string title = reader.GetString(1);
                            string lastName = reader.GetString(2);
                            Console.WriteLine();
                        }
                    }
                }
            }
        }
        static async Task UpdateBookAsync()
        {

        }
    }


}