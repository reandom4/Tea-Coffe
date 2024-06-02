using System;
using System.Collections.Generic;
using System.Data;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace Tea_Coffe
{
    /// <summary>
    /// Логика взаимодействия для ShowOrders.xaml
    /// </summary>
    public partial class ShowOrders : Window
    {
        readonly DataBase dataBase = new DataBase();
        public ShowOrders()
        {
            InitializeComponent();
            Init();

        }
        // Метод инициализации, который загружает заказы и отображает их в интерфейсе
        private void Init()
        {
            try
            {
                DataTable dataTable = dataBase.GetOrders();

                List<OrderItem> OrderList = new List<OrderItem>();


                foreach (DataRow row in dataTable.Rows)
                {
                    // Создание нового объекта ProductItem
                    OrderItem item = new OrderItem
                    {
                        // Присвоение значения свойствам из данных строки таблицы
                        Id = Convert.ToInt32(row["idOrder"]),
                        OrderProducts = Convert.ToInt32(row["OrderProducts"]),
                        OrderPrice = row["OrderPrice"].ToString(),
                        Date = row["date"].ToString().ToString(),
                        NPS = $"{row["name"]} {row["patronymic"]} {row["surname"].ToString().ToCharArray()[0]}.",
                    };
                    OrderList.Add(item);
                }
                ProductView.ItemsSource = OrderList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Класс, представляющий элемент заказа
        public class OrderItem
        {
            public int Id { get; set; }
            public int OrderProducts { get; set; }
            public string OrderPrice { get; set; }
            public string Date { get; set; }
            public string NPS { get; set; }

        }
        // Метод, который открывает подробную информацию о заказе при двойном щелчке мыши
        private void OpenFullOrder(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)sender).DataContext as OrderItem;// Получение данных о заказе из контекста данных
            ShowOrderItems showOrderItems = new ShowOrderItems(item.OrderProducts);// Создание окна для отображения подробной информации о заказе
            showOrderItems.ShowDialog();// Открытие окна в виде диалогового окна
        }

        
    }
}
