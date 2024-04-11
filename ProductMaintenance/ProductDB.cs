using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;


namespace ProductMaintenance
{
    public class ProductDB
    {
        public static Product GetProduct(string code)
        {
            SqlConnection connection = MMABooksDB.GetConnection();
            string selectStatement =
            "SELECT Code, Description, Price " +
            "FROM Products " +
            "WHERE Code = @Code";
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            selectCommand.Parameters.AddWithValue("@Code", code);
            try
            {
                connection.Open();
                SqlDataReader prodReader = selectCommand.ExecuteReader(CommandBehavior.SingleRow);
                if (prodReader.Read())
                {
                    Product p = new Product();
                    p.Code = (string)prodReader["Code"];
                    p.Description = prodReader["Description"].ToString();
                    p.Price = (decimal)prodReader["Price"];
                    return p;
                }
                else
                    return null;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

        }

        public static string AddProduct(Product product)
        {
            SqlConnection connection = MMABooksDB.GetConnection();
            string insertStatement = "INSERT Products " + "(Description, Price) " +
                                     "VALUES (@Description, @Price)";
            SqlCommand insertCommand = new SqlCommand(insertStatement, connection);
            insertCommand.Parameters.AddWithValue("@Description", product.Description);
            insertCommand.Parameters.AddWithValue("@Price", product.Price);
            try
            {
                connection.Open();
                insertCommand.ExecuteNonQuery();
                string selectStatement = "SELECT IDENT_CURRENT('Products') " +
                                         "FROM Products";
                SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
                string code = Convert.ToString(selectCommand.ExecuteScalar());
                return code;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool UpdateProduct(Product oldProduct, Product newProduct)
        {
            SqlConnection connection = MMABooksDB.GetConnection();
            string updateStatement =
                "UPDATE Products SET " +
                "Description  = @NewDescription, " +
                "Price = @NewPrice, " +
                "WHERE Code = @OldCode " +
                "AND Description = @OldDescription " +
                "AND Price = @OldPrice";
            SqlCommand updateCommand = new SqlCommand(updateStatement, connection);
            updateCommand.Parameters.AddWithValue("@NewDescription", newProduct.Description);
            updateCommand.Parameters.AddWithValue("@NewPrice", newProduct.Price);
            updateCommand.Parameters.AddWithValue("@OldCode", oldProduct.Code);
            updateCommand.Parameters.AddWithValue("@OldDescription", oldProduct.Description);
            updateCommand.Parameters.AddWithValue("@OldPrice", oldProduct.Price);
            try
            {
                connection.Open();
                int count = updateCommand.ExecuteNonQuery();
                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool DeleteProduct(Product product)
        {
            SqlConnection connection = MMABooksDB.GetConnection();
            string deleteStatement = "DELETE FROM Products " + "WHERE Code = @Code " + "AND Description = @Description " + "AND Price = @Price ";
            SqlCommand deleteCommand = new SqlCommand(deleteStatement, connection);
            deleteCommand.Parameters.AddWithValue("@Code", product.Code);
            deleteCommand.Parameters.AddWithValue("@Description", product.Description);
            deleteCommand.Parameters.AddWithValue("@Price", product.Price);
            try
            {
                connection.Open();
                int count = deleteCommand.ExecuteNonQuery();
                if (count > 0)
                    return true;
                else
                    return false;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
