using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Tea_Coffe.Window1;

namespace Tea_Coffe
{
    /// <summary>
    /// Логика взаимодействия для StorageWindow.xaml
    /// </summary>
    public partial class StorageWindow : Window
    {
        readonly DataBase dataBase = new DataBase();
        readonly WordHelper wordHelper = new WordHelper();

        // Конструктор для инициализации окна хранилища
        public StorageWindow()
        {
            InitializeComponent();
            try
            {
                DataTable dataTable = dataBase.LoadProductsStorage();
                Showdata(dataTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        // Метод для отображения данных в DataGrid
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
        // Обработчик события изменения текста в текстовом поле для поиска продуктов
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
        // Обработчик события изменения количества продукта
        private void QuantityChange(object sender, RoutedEventArgs e)
        {
            try
            {

                ProductItem item = ((FrameworkElement)sender).DataContext as ProductItem;
                dataBase.ChangeQuantity(item);
                MessageBox.Show("Успешно изменено", "Изменения", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Обработчик события ввода текста в текстовом поле для количества
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
        // Обработчик события нажатия клавиши в текстовом поле для количества
        private void NumericTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Запрещаем использование пробела
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        // Обработчик события нажатия кнопки для экспорта данных о продуктах в хранилище
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dataTable = dataBase.LoadProductsStorage();

                List<ProductItem> productList = new List<ProductItem>();


                foreach (DataRow row in dataTable.Rows)
                {
                    // Создание нового объекта ProductItem
                    ProductItem item = new ProductItem
                    {
                        // Присвоение значения свойствам из данных строки таблицы
                        Id = Convert.ToInt32(row["idProducts"]),
                        Name = row["name"].ToString(),
                        Unit = row["Products_unitname"].ToString(),
                        MinUnit = Convert.ToInt32(row["products_unitcol"]),
                        Quantity = Convert.ToInt32(row["quantity"]),
                        QuantityInStock = Convert.ToInt32(row["quantity"])
                    };

                    // Добавление объекта ProductItem в список
                    productList.Add(item);
                }
                wordHelper.CreateStorage(productList);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Обработчик события нажатия кнопки для выполнения поиска
        private void SearchButtonClick(object sender, RoutedEventArgs e)
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
    }
}
