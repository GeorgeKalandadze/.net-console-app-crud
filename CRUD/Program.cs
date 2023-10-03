using System;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = @"Data Source=DESKTOP-URAA2EI\SQLEXPRESS;Initial Catalog=online-magazine;Integrated Security=True";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string sqlQuery = "SELECT p.Name AS ProductName, c.Name AS CategoryName, p.Price " +
                                  "FROM products p " +
                                  "INNER JOIN categories c ON p.category_ID = c.ID";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string productName = (string)reader["ProductName"];
                            string categoryName = (string)reader["CategoryName"];
                            decimal price = (decimal)reader["Price"];

                            string rowData = $"Product Name: {productName}, Category Name: {categoryName}, Price: {price:C}";
                            Console.WriteLine(rowData);
                        }
                    }
                }

                connection.Close();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadKey();
            }
        }
    }
}
