using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
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

namespace Tea_Coffe
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        DataBase dataBase = new DataBase();

        public Window1()
        {
            InitializeComponent();

            showdata();
            
        }

        void showdata()
        {
            DataTable dataTable = new DataTable();
            dataTable = dataBase.LoadProducts();

            List<ProductItem> productList = new List<ProductItem>();


            foreach (DataRow row in dataTable.Rows)
            {
                // Создание нового объекта ProductItem
                ProductItem item = new ProductItem();

                // Присвоение значения свойствам из данных строки таблицы
                item.id = Convert.ToInt32(row["idProducts"]);
                item.Name = row["name"].ToString();
                item.ImageData = "E:/Diplom/Tea Coffe/Tea Coffe/image/" + row["photo"].ToString();
                item.Cost = Convert.ToInt32(row["cost"]);
                item.DefaultCost = Convert.ToInt32(row["cost"]);
                item.Unit = row["Products_unitname"].ToString();
                item.MinUnit = Convert.ToInt32(row["products_unitcol"]);
                item.Quantity = Convert.ToInt32(row["products_unitcol"]);
                item.QuantityInStock = Convert.ToInt32(row["quantity"]);
                if(item.QuantityInStock < item.MinUnit)
                {
                    item.Quantity = 0;
                }
                // Добавление объекта ProductItem в список
                productList.Add(item);

            }


            ProductView.ItemsSource = productList;
        }
        

        private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Предотвращаем прокрутку колесиком мыши для ListView
            e.Handled = false;
        }

        private void PlusButtonImage(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)sender).DataContext as ProductItem; // Замените YourItemType на ваш тип данных

            if(item.Quantity + item.MinUnit > item.QuantityInStock)
            {
                return;
            }
            // Увеличиваем вес на 50 г
            item.Quantity += item.MinUnit;
            item.Cost += item.DefaultCost;
        }
        private void MinusButtonImage(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)sender).DataContext as ProductItem; // Замените YourItemType на ваш тип данных

            if (item.Quantity == item.MinUnit)
            {
                return;
            }
            // Увеличиваем вес на 50 г
            item.Quantity -= item.MinUnit;
            item.Cost -= item.DefaultCost;
        }


        public class ProductItem : INotifyPropertyChanged
        {
            public int id { get; set; }
            public string Name { get; set; }
            public string ImageData { get; set; }

            public int DefaultCost { get; set; }
            public string Category { get; set; }
            public string Unit { get; set; }
            public int MinUnit { get; set; }
            public int QuantityInStock { get; set; }
            private int quantity;
            public int Quantity
            {
                get { return quantity; }
                set
                {
                    if (quantity != value)
                    {
                        quantity = value;
                        OnPropertyChanged(nameof(Quantity));
                    }
                }
            }
            private int cost;

            public int Cost
            {
                get { return cost; }
                set
                {
                    if (cost != value)
                    {
                        cost = value;
                        OnPropertyChanged(nameof(Cost));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        private void InBasketButton(object sender, RoutedEventArgs e)
        {

        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Перенаправляем событие колесика мыши из ScrollViewer в ListView
            e.Handled = true;
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
            {
                RoutedEvent = UIElement.MouseWheelEvent,
                Source = sender
            };
            ProductView.RaiseEvent(eventArg);
        }
        public class StringIsEmptyConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is string str)
                {
                    return string.IsNullOrWhiteSpace(str);
                }
                return false;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
