using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using static Tea_Coffe.Window1;

namespace Tea_Coffe
{
    /// <summary>
    /// Логика взаимодействия для ShowOrderItems.xaml
    /// </summary>
    public partial class ShowOrderItems : Window
    {
        readonly DataBase dataBase = new DataBase();
        // Конструктор для инициализации окна с подробной информацией о заказе
        public ShowOrderItems(int id)
        {
            InitializeComponent();
            try
            {
                DataTable dataTable = dataBase.GetOrderItem(id);

                List<ProductItem> productList = new List<ProductItem>();


                foreach (DataRow row in dataTable.Rows)
                {
                    // Создание нового объекта ProductItem
                    ProductItem item = new ProductItem
                    {
                        // Присвоение значения свойствам из данных строки таблицы
                        Id = Convert.ToInt32(row["product_id"]),
                        Name = row["name"].ToString(),
                        ImageData = System.IO.Directory.GetCurrentDirectory() + "\\image\\" + row["photo"].ToString(),
                        Unit = row["Products_unitname"].ToString(),
                        Quantity = Convert.ToInt32(row["quantity"]),



                    };

                    // Добавление объекта ProductItem в список
                    productList.Add(item);

                }
                ProductView.ItemsSource = productList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
