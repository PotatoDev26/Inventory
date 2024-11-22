using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Inventory
{
    public partial class frmAddProduct : Form
    {

        private string _ProductName, _Category, _MfgDate, _ExpDate,
            _Description, _Quantity, _SellPrice;

        private string[] ListOfProductCategory = { 
            "Beverages", "Bread/Bakery", "Canned/Jarred Goods", "Dairy"
            , "Frozen Goods", "Meat", "Personal Care", "Other" };

        private BindingSource showProductList;

        private bool productException = false, quantException = false, priceException = false;

        public frmAddProduct()
        {
            showProductList = new BindingSource();
            InitializeComponent();
            InitComboBox();
        }

        private void InitComboBox()
        {
            foreach (var goods in ListOfProductCategory)
            {
                cbCategory.Items.Add(goods);
            }
        }
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            _ProductName = Product_Name(txtProductName.Text);
            _Category = cbCategory.Text;
            _MfgDate = dtPickerMfgDate.Value.ToString("yyyy-MM-dd");
            _ExpDate = dtPickerExpDate.Value.ToString("yyyy-MM-dd");
            _Description = richTxtDescription.Text;
            _Quantity = Quantity(txtQuantity.Text).ToString();
            _SellPrice = SellingPrice(txtSellPrice.Text).ToString();

            if (productException || quantException || priceException) {
                MessageBox.Show("Inputs are not accepted nor added to the data grid....");
                productException = false; quantException = false; priceException = false;
            } else
            {
                MessageBox.Show("User product info has been added!");
                showProductList.Add(new ProductClass(_ProductName, _Category, _MfgDate,
                _ExpDate, Double.Parse(_SellPrice), Int32.Parse(_Quantity), _Description));
                gridViewProductList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                gridViewProductList.DataSource = showProductList;
            }
            
        }
        public string Product_Name(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name) || !Regex.IsMatch(name, @"^[a-zA-Z]+$"))
                {
                    throw new StringFormatException(StringFormatException.message);
                }
                return name;
            }
            catch (StringFormatException ex)
            {
                logBox.Items.Add(ex.Message());
                Console.WriteLine("An exception has been caught!");
                productException = true;
                return null;
            }
            finally
            {
                Console.WriteLine("Input accepted");
            }
        }
        public int Quantity(string qty)
        {
            try
            {
                if (string.IsNullOrEmpty(qty) || !Regex.IsMatch(qty, @"^[0-9]"))
                {
                    throw new NumberFormatException(NumberFormatException.message);
                }
                return Convert.ToInt32(qty);
            }
            catch (NumberFormatException ex)
            {
                logBox.Items.Add(ex.Message());
                Console.WriteLine("An exception has been caught!");
                quantException = true;
                return 0;
            }
            finally
            {
                Console.WriteLine("Input accepted");
            }
        }
        public double SellingPrice(string price)
        {
            try
            { 
                if (string.IsNullOrEmpty(price) || !Regex.IsMatch(price.ToString(), @"^(\d*\.)?\d+$"))
                {
                    throw new CurrencyFormatException(CurrencyFormatException.message);
                }
                return Convert.ToDouble(price);
            }
            catch (CurrencyFormatException ex)
            {
                logBox.Items.Add(ex.Message());
                Console.WriteLine("An exception has been caught!");
                priceException = true;
                return 0;
            }
            finally
            {
                Console.WriteLine("Input accepted");
            }
        }
    }




    /// EXCEPTION CLASSES ---------------------------------------------------------------------------------------------------->>>>
    public class StringFormatException : Exception
    {
        public static string message;
        public StringFormatException(string str) : base(str) {
            message = "Invalid product name format.";
        }
        public new string Message()
        {
            return message;
        }
    }
    public class NumberFormatException : Exception
    {
        public static string message;
        public NumberFormatException(string str) : base(str)
        {
            message = "Invalid quantity format.";
        }
        public new string Message()
        {
            return message;
        }
    }
    public class CurrencyFormatException : Exception
    {
        public static string message;
        public CurrencyFormatException(string str) : base(str) {
            message = "Invalid price format.";
        }
        public new string Message()
        {
            return message;
        }
    }
}
