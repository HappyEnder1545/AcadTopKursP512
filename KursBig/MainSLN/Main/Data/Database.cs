using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace PersonalLibrary.Data
{
    public static class Database
    {
        public static string ConnectionString = @"Server=(localdb)\MSSQLLocalDB;Database=Kurs;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }
        public static void Initialize()
        {
            var masterCs = ConnectionString.Replace("Database=Kurs", "Database=master");
            using (var conn = new SqlConnection(masterCs))
            {
                conn.Open();
                using var cmd = new SqlCommand("IF DB_ID('PersonalLibrary') IS NULL CREATE DATABASE PersonalLibrary;", conn);
                cmd.ExecuteNonQuery();
            }

            
            using var c = GetConnection();
            string schema = @"
            IF NOT EXISTS (SELECT FROM * sys.tables WHERE name = 'Genre')
            CREATE TABLE Genre (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Name VARCHAR(100) NOT NULL );

            IF NOT EXISTS (SELECT FROM * sys.tables WHERE name = 'Author')
            CREATE TABLE Author (
                Id INT PRIMARY KEY IDENTITY(1,1),
                FirstName VARCHAR(100) NOT NULL,
                LastName  VARCHAR(100) NOT NULL
            );

            IF NOT EXISTS (SELECT FROM * sys.tables WHERE name = 'Book '
            CREATE TABLE Book (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Title VARCHAR(200),
                AuthorId INTEGER NOT NULL REFERENCES Author(Id),
                GenreId  INT NOT NULL REFERENCES Genre(Id),
                Year INTEGER,
                Pages INTEGER,
                Status VARCHAR(50) NOT NULL DEFAULT 'NotRead',
                Notes VARCHAR(100) NULL
            );";
            using (var cmd = new SqlCommand(schema, c))
            cmd.ExecuteNonQuery();

            Console.WriteLine("Созданы таблицы: Жанры, Авторы, Книги");

            SeedData(c);
        }

        private static void SeedData(SqlConnection c)
        {
                
            using (var check = new SqlCommand("SELECT COUNT(*) FROM Book", c))
            {
                int count = (int)check.ExecuteScalar();
                if (count > 0) return;
            }

            string seed = @"INSERT INTO Genre (Name) VALUES
            (N'Роман'), (N'Фантастика'), (N'Детектив'), (N'Научная литература'), (N'Поэзия');

            INSERT INTO Author (FirstName, LastName) VALUES
            (N'Лев', N'Толстой'),
            (N'Фёдор', N'Достоевский'),
            (N'Айзек', N'Азимов'),
            (N'Артур', N'Конан Дойл'),
            (N'Михаил', N'Булгаков');

            INSERT INTO Book (Title, AuthorId, GenreId, Year, Pages, Status, Notes) VALUES
            (N'Война и мир', 1, 1, 1869, 1300, 'Read', N'Классика'),
            (N'Анна Каренина', 1, 1, 1877, 864, 'Reading', NULL),
            (N'Преступление и наказание', 2, 3, 1866, 671, 'Read', NULL),
            (N'Идиот', 2, 1, 1869, 640, 'NotRead', NULL),
            (N'Я, робот', 3, 2, 1950, 253, 'Read', N'Сборник рассказов'),
            (N'Основание', 3, 2, 1951, 244, 'NotRead', NULL),
            (N'Этюд в багровых тонах', 4, 3, 1887, 200, 'Reading', NULL),
            (N'Мастер и Маргарита', 5, 1, 1967, 480, 'Read', N'Любимая книга');";

            using var cmd = new SqlCommand(seed, c);
            cmd.ExecuteNonQuery(); 
        }
    }
}