using System;
using System.Data.SqlClient;
using System.Data;

class Program
{
    static string connectionString = @"Data Source=DESKTOP-URAA2EI\SQLEXPRESS;Initial Catalog=online-magazine;Integrated Security=True";

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Create Product");
            Console.WriteLine("2. Read Products");
            Console.WriteLine("3. Update Product");
            Console.WriteLine("4. Delete Product");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateProduct();
                    break;
                case "2":
                    ReadProducts();
                    break;
                case "3":
                    UpdateProduct();
                    break;
                case "4":
                    DeleteProduct();
                    break;
                case "5":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void CreateProduct()
    {
        Console.Write("Enter Product Name: ");
        string productName = Console.ReadLine();

        Console.Write("Enter Category ID: ");
        int categoryId = int.Parse(Console.ReadLine());

        Console.Write("Enter Price: ");
        decimal price = decimal.Parse(Console.ReadLine());

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string insertQuery = "INSERT INTO products (Name, category_ID, Price) " +
                                     "VALUES (@ProductName, @CategoryId, @Price)";

                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@ProductName", productName);
                    insertCommand.Parameters.AddWithValue("@CategoryId", categoryId);
                    insertCommand.Parameters.AddWithValue("@Price", price);

                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Product created successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to create the product.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static void ReadProducts()
    {
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
            }
        }
    }

    static void UpdateProduct()
    {
        Console.Write("Enter Product ID to update: ");
        int productId = int.Parse(Console.ReadLine());

        Console.Write("Enter New Product Name: ");
        string newProductName = Console.ReadLine();

        Console.Write("Enter New Category ID: ");
        int newCategoryId = int.Parse(Console.ReadLine());

        Console.Write("Enter New Price: ");
        decimal newPrice = decimal.Parse(Console.ReadLine());

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string updateQuery = "UPDATE products " +
                                     "SET Name = @NewProductName, category_ID = @NewCategoryId, Price = @NewPrice " +
                                     "WHERE ID = @ProductId";

                using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@NewProductName", newProductName);
                    updateCommand.Parameters.AddWithValue("@NewCategoryId", newCategoryId);
                    updateCommand.Parameters.AddWithValue("@NewPrice", newPrice);
                    updateCommand.Parameters.AddWithValue("@ProductId", productId);

                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Product updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No product with the given ID found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static void DeleteProduct()
    {
        Console.Write("Enter Product ID to delete: ");
        int productId = int.Parse(Console.ReadLine());

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string deleteQuery = "DELETE FROM products WHERE ID = @ProductId";

                using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@ProductId", productId);

                    int rowsAffected = deleteCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Product deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No product with the given ID found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
