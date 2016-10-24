using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;       // TILLAGT
using System.Data.SqlClient;      // TILLAGT

namespace FormsDB_test1
{
    public partial class Form1 : Form
    {
        private DBConnection connection = new DBConnection();

        public Form1()
        {
            InitializeComponent();
            var categories = connection.GetCategories();
            var products = connection.GetProducts(1);

            foreach (var item in categories)
            {
                dataView.Rows.Add(item.ID + "", item.CategoryName);
            }

            foreach (var item in products)
            {
                dataProducts.Rows.Add(item.ID + "", item.ProductName, item.UnitPrice);
            }
            dataView.Rows[0].Selected = true;
            dataProducts.Rows[0].Selected = true;
        }

        private void dataView_SelectionChanged(object sender, EventArgs e)
        {
            UpdateProductsDataGrid();
        }

        private void UpdateProductsDataGrid()
        {
            dataProducts.Rows.Clear();

            if (dataView.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataView.SelectedCells[0].RowIndex;

                var product = connection.GetProducts(selectedrowindex + 1);

                foreach (var item in product)
                {
                    dataProducts.Rows.Add(item.ID + "", item.ProductName, item.UnitPrice);
                }
            }
            dataProducts.Rows[0].Selected = true;
        }

        private void dataProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int selectedrowindex = dataProducts.SelectedCells[0].RowIndex;

            DataGridViewRow selectedRow = dataProducts.Rows[selectedrowindex];

            var selectedID = selectedRow.Cells[0].Value;
            var selectedName = selectedRow.Cells[1].Value;
            var selectedPrice = selectedRow.Cells[2].Value;

            var product = new Products(int.Parse(selectedID.ToString()), selectedName.ToString(), decimal.Parse(selectedPrice.ToString()));

            connection.UpdateProducts(product.ID, product.ProductName, product.UnitPrice);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            decimal price;
            if (!String.IsNullOrEmpty(txtName.Text.Trim()) && decimal.TryParse(txtPrice.Text, out price))
            {
                connection.CreateProduct(txtName.Text, price, dataView.SelectedCells[0].RowIndex + 1);

                UpdateProductsDataGrid();

                txtPrice.Text = "";
                txtName.Text = "";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (dataProducts.SelectedCells.Count > 0)
            {
                int selectedrowindex = dataProducts.SelectedCells[0].RowIndex;

                DataGridViewRow selectedRow = dataProducts.Rows[selectedrowindex];

                var selectedID = selectedRow.Cells[0].Value;

                connection.DeleteProduct(int.Parse(selectedID.ToString()));

                UpdateProductsDataGrid();

            }


        }
    }


    public class Category
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }

        public Category(int id, string name)
        {
            ID = id;
            CategoryName = name;
        }
    }
    public class Products
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }

        public Products(int id, string productName, decimal unitPrice)
        {
            ID = id;
            ProductName = productName;
            unitPrice = UnitPrice;
        }
    }
    public class DBConnection
    {
        string conString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

        public List<Category> GetCategories()
        {
            var returnList = new List<Category>();
            using (var cn = new SqlConnection(conString))
            {
                cn.Open();
                string sqlcmd = @"Select [CategoryID], [CategoryName] From [dbo].[Categories]";

                using (var cmd = new SqlCommand(sqlcmd, cn))
                {
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        returnList.Add(new Category(rd.GetInt32(0), rd.GetString(1)));
                    }
                }
            }
            return returnList;
        }

        public List<Products> GetProducts(int categoryID)
        {
            var returnList = new List<Products>();
            using (var cn = new SqlConnection(conString))
            {
                cn.Open();
                string sqlcmd = @"Select  [ProductID], [ProductName], [UnitPrice] From [dbo].[Products] Where [CategoryID] = @categoryID";

                using (var cmd = new SqlCommand(sqlcmd, cn))
                {
                    cmd.Parameters.AddWithValue("@categoryId", categoryID);
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        returnList.Add(new Products(rd.GetInt32(0), rd.GetString(1), rd.GetDecimal(2)));
                    }
                }
            }
            return returnList;
        }
        public void UpdateProducts(int productID, string name, decimal price)
        {
            using (var cn = new SqlConnection(conString))
            {
                cn.Open();
                string sqlcmd = @"Update [dbo].[Products] Set [ProductName] = @nameChange, [UnitPrice] = @priceChange Where [ProductId] = @productID";

                using (var cmd = new SqlCommand(sqlcmd, cn))
                {
                    cmd.Parameters.AddWithValue("@productID", productID);
                    cmd.Parameters.AddWithValue("@priceChange", price);
                    cmd.Parameters.AddWithValue("@nameChange", name);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CreateProduct(string name, decimal price, int categoryId)
        {
            using (var cn = new SqlConnection(conString))
            {
                cn.Open();
                string sqlcmd = @"Insert Into [dbo].[Products]([ProductName], UnitPrice], [CategoryID]) Values(@name, @price, @categoryId)";

                using (var cmd = new SqlCommand(sqlcmd, cn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@categoryId", categoryId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteProduct(int ProductID)
        {
            try
            {
                using (var cn = new SqlConnection(conString))
                {
                    cn.Open();
                    string sqlcmd = @"Delete from [dbo].[Products] Where [ProductID] = @productID";

                    using (var cmd = new SqlCommand(sqlcmd, cn))
                    {
                        cmd.Parameters.AddWithValue("@productID", ProductID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (System.Exception)
            {
            }
        }
    }
}
    


    

