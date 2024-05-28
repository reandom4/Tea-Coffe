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
        readonly DataBase dataBase = new DataBase();

        // Конструктор для инициализации окна хранилища
        public StorageWindow()
        {
            InitializeComponent();
            DataTable dataTable = dataBase.LoadProductsStorage();
            Showdata(dataTable);
        }

        private void Showdata(DataTable dataTable)
        {
            List<ProductItem> productList = new List<ProductItem>();


            foreach (DataRow row in dataTable.Rows)
            {
                // Создание нового объекта ProductItem
                ProductItem item = new ProductItem
                {
                    // Присвоение значения свойствам из данных строки таблицы
                    Id = Convert.ToInt32(row["idProducts"]),
                    Name = row["name"].ToString(),
                    ImageData = System.IO.Directory.GetCurrentDirectory() + "\\image\\" + row["photo"].ToString(),
                    Cost = Convert.ToInt32(row["cost"]),
                    DefaultCost = Convert.ToInt32(row["cost"]),
                    Unit = row["Products_unitname"].ToString(),
                    MinUnit = Convert.ToInt32(row["products_unitcol"]),
                    Quantity = Convert.ToInt32(row["products_unitcol"]),
                    QuantityInStock = Convert.ToInt32(row["quantity"])
                };
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

        private void Search(object sender, TextChangedEventArgs e)
        {
            string search = SearchTB.Text;


            try
            {
               
                DataTable dt = dataBase.SearchProducts(search, "quantity");
                Showdata(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void QuantityChange(object sender, RoutedEventArgs e)
        {
            try
            {
                
                ProductItem item = ((FrameworkElement)sender).DataContext as ProductItem;
                dataBase.ChangeQuantity(item);
                MessageBox.Show("Успешно изменено","Изменения",MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",MessageBoxButton.OK,MessageBoxImage.Error);
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
