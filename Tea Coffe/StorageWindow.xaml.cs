using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Tea_Coffe.Window1;

namespace Tea_Coffe
{
    /// <summary>
    /// Логика взаимодействия для StorageWindow.xaml
    /// </summary>
    public partial class StorageWindow : Window
    {
        DataBase dataBase = new DataBase();
        public StorageWindow()
        {
            InitializeComponent();
            DataTable dataTable = dataBase.SearchProducts("", "Популярные");
            List<ProductItem> productList = new List<ProductItem>();


            foreach (DataRow row in dataTable.Rows)
            {
                // Создание нового объекта ProductItem
                ProductItem item = new ProductItem();

                // Присвоение значения свойствам из данных строки таблицы
                item.Id = Convert.ToInt32(row["idProducts"]);
                item.Name = row["name"].ToString();
                item.ImageData = "E:/Diplom/Tea Coffe/Tea Coffe/image/" + row["photo"].ToString();
                item.Cost = Convert.ToInt32(row["cost"]);
                item.DefaultCost = Convert.ToInt32(row["cost"]);
                item.Unit = row["Products_unitname"].ToString();
                item.MinUnit = Convert.ToInt32(row["products_unitcol"]);
                item.Quantity = Convert.ToInt32(row["products_unitcol"]);
                item.QuantityInStock = Convert.ToInt32(row["quantity"]);
                if (item.QuantityInStock < item.MinUnit)
                {
                    item.Quantity = 0;
                }
                // Добавление объекта ProductItem в список
                productList.Add(item);
                item.Description = row["description"].ToString();
                item.Cooking_method = row["cooking_method"].ToString();
                item.Taste_and_aroma = row["taste_and_aroma"].ToString();
                item.MaxQuantity = "Collapsed";
            }

            ProductView.ItemsSource = productList;
        }
    }
}
