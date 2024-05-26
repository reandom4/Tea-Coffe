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
    /// Логика взаимодействия для CRUDProduct.xaml
    /// </summary>
    public partial class CRUDProduct : Window
    {
        public CRUDProduct()
        {
            InitializeComponent();
            DataBase dataBase = new DataBase();
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

            ProductItem productItem = productList[0];

            try
            {
                
                
                name.Text = productItem.Name;
                cost.Text = productItem.Cost.ToString();

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(productItem.ImageData);
                bitmap.EndInit();
                image.Source = bitmap;

                Quantity.Text = productItem.Quantity.ToString();
                unit.Text = productItem.Unit;

                descriptionHead.Visibility = Visibility.Collapsed;
                cooking_methodHead.Visibility = Visibility.Collapsed;
                taste_and_aromaHead.Visibility = Visibility.Collapsed;
                description.Visibility = Visibility.Collapsed;
                cooking_method.Visibility = Visibility.Collapsed;
                taste_and_aroma.Visibility = Visibility.Collapsed;
                if (productItem.Description != null && productItem.Description != "")
                {
                    descriptionHead.Visibility = Visibility.Visible;
                    description.Visibility = Visibility.Visible;
                    description.Text = productItem.Description;
                }
                if (productItem.Cooking_method != null && productItem.Cooking_method != "")
                {
                    cooking_methodHead.Visibility = Visibility.Visible;
                    cooking_method.Visibility = Visibility.Visible;
                    cooking_method.Text = productItem.Cooking_method;
                }
                if (productItem.Taste_and_aroma != null && productItem.Taste_and_aroma != "")
                {
                    taste_and_aromaHead.Visibility = Visibility.Visible;
                    taste_and_aroma.Visibility = Visibility.Visible;
                    taste_and_aroma.Text = productItem.Taste_and_aroma;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true; // Отменить ввод, если символ не является цифрой
                    break;
                }
            }
        }
        // Запрещает использование пробела в текстовом поле
        private void NumericTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Запрещаем использование пробела
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}
