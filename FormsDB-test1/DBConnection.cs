using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

//namespace FormsDB_test1
//{
//    public class DBConnection
//    {
//        string conString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

//        public List<Category> GetCategories()
//        {
//            var returnList = new List<Category>();
//            using (var cn = new SqlConnection(conString))
//            {
//                cn.Open();
//                string sqlcmd = @"SELECT [CategoryID],[CategoryName] FROM [dbo].[Categories]";

//                using (var cmd = new SqlCommand(sqlcmd, cn))
//                {
//                    var rd = cmd.ExecuteReader();
//                    while (rd.Read())
//                    {
//                        returnList.Add(new Category(rd.GetInt32(0), rd.GetString(1)));
//                    }
//                }
//            }

//            return returnList;
//        }

//        public List<Products> GetProducts(int categoryId)
//        {
//            var returnList = new List<Products>();
//            using (var cn = new SqlConnection(conString))
//            {
//                cn.Open();
//                string sqlcmd = @"SELECT [ProductID],[ProductName],[UnitPrice] FROM [dbo].[Products] WHERE [CategoryID] = @categoryId";

//                using (var cmd = new SqlCommand(sqlcmd, cn))
//                {
//                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
//                    var rd = cmd.ExecuteReader();
//                    while (rd.Read())
//                    {
//                        returnList.Add(new Products(rd.GetInt32(0), rd.GetString(1), rd.GetDecimal(2)));
//                    }
//                }
//            }

//            return returnList;
//        }

//        public void UpdateProducts(int productID, string name, decimal price)
//        {
//            using (var cn = new SqlConnection(conString))
//            {
//                cn.Open();
//                string sqlcmd = @"UPDATE [dbo].[Products] SET [ProductName] = @nameChange, [UnitPrice] = @priceChange WHERE [ProductId] = @productID";

//                using (var cmd = new SqlCommand(sqlcmd, cn))
//                {
//                    cmd.Parameters.AddWithValue("@productID", productID);
//                    cmd.Parameters.AddWithValue("@priceChange", price);
//                    cmd.Parameters.AddWithValue("@nameChange", name);
//                    cmd.ExecuteNonQuery();
//                }
//            }
//        }

//        public void CreateProduct(string name, decimal price, int categoryId)
//        {
//            using (var cn = new SqlConnection(conString))
//            {
//                cn.Open();
//                string sqlcmd = @"INSERT INTO [dbo].[Products]([ProductName],[UnitPrice],[CategoryID]) VALUES(@name, @price, @categoryId)";

//                using (var cmd = new SqlCommand(sqlcmd, cn))
//                {
//                    cmd.Parameters.AddWithValue("@name", name);
//                    cmd.Parameters.AddWithValue("@price", price);
//                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
//                    cmd.ExecuteNonQuery();
//                }
//            }
//        }

//        public void DeleteProduct(int productID)
//        {
//            try
//            {
//                using (var cn = new SqlConnection(conString))
//                {
//                    cn.Open();
//                    string sqlcmd = @"DELETE FROM [dbo].[Products] WHERE [ProductID] = @productID";

//                    using (var cmd = new SqlCommand(sqlcmd, cn))
//                    {
//                        cmd.Parameters.AddWithValue("@productID", productID);
//                        cmd.ExecuteNonQuery();
//                    }
//                }
//            }
//            catch (System.Exception)
//            {
//                //TODO: do stuff
//            }
//        }
//    }
//}
