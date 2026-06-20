using Microsoft.Data.SqlClient;
using System;

namespace Proj
{
    class Program
    {
        static void Main(string[] args)
        {

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