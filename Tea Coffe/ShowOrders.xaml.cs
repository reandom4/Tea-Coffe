using System;
using System.Collections.Generic;
using System.ComponentModel;
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
                        NPS = $"{row["name"]} {row["patronymic"]} {row["surname"].ToString().ToCharArray()[0]}." ,
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

        public class OrderItem
        {
            public int Id { get; set; }
            public int OrderProducts { get; set; }
            public string OrderPrice { get; set; }
            public string Date { get; set; }
            public string NPS { get; set; }

        }

        private void OpenFullOrder(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)sender).DataContext as OrderItem;
            ShowOrderItems showOrderItems = new ShowOrderItems(item.OrderProducts);
            showOrderItems.ShowDialog();
        }
    }
}
