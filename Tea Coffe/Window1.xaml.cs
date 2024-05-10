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
using System.Windows.Media.Animation;
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
                item.description = row["description"].ToString();
                item.cooking_method = row["cooking_method"].ToString();
                item.taste_and_aroma = row["taste_and_aroma"].ToString();
                item.MaxQuantity = "Collapsed";
            }


            ProductView.ItemsSource = productList;
        }

        void sort(string sorntvalue)
        {

        }
        List<ProductItem> Basket = new List<ProductItem>();
        private void InBasketButton(object sender, RoutedEventArgs e)
        {
            try
            {
                ProductItem item = ((FrameworkElement)sender).DataContext as ProductItem;

                var existingItem = Basket.FirstOrDefault(p => p.id == item.id);

                if (existingItem != null)
                {
                    // Увеличиваем количество товара в корзине
                    if (existingItem.BasketQuantity + item.Quantity > item.QuantityInStock)
                    {
                        item.BasketQuantity = item.QuantityInStock;
                    }
                    else
                    {
                        item.BasketQuantity += item.Quantity;
                    }
                    
                }
                else
                {
                    // Добавляем новый товар в корзину
                    item.BasketQuantity = item.Quantity;
                    Basket.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       

        FullProductInfo fullProductInfo;
        private void OpenFullProductMenu(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)sender).DataContext as ProductItem;

            if (fullProductInfo != null)
            {
                fullProductInfo.Close();
            }

            fullProductInfo = new FullProductInfo(item);
            fullProductInfo.Show();
            fullProductInfo.Activate();
            fullProductInfo.Focus();
        }


        //Анимации
        private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Предотвращаем прокрутку колесиком мыши для ListView
            e.Handled = false;
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

            //Закрытие меню при пролистывании страницы
            if (leftPanel.Visibility == Visibility.Visible)
            {
                CloseLeftMenu();
                return;
            }
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
        
        private void ShowCatalogButton(object sender, RoutedEventArgs e)
        {
            if (leftPanel.Visibility == Visibility.Visible)
            {
                CloseLeftMenu();
                return;
            }
            OpenLeftMenu();
        }

        private void leftPanelFonClick(object sender, MouseButtonEventArgs e)
        {
            CloseLeftMenu();
        }

        private void OpenLeftMenu()
        {
            //Анимация выдвигание левого меню
            var PromotionLeftMenuanimation = new ThicknessAnimation
            {
                Duration = TimeSpan.FromSeconds(0.2), // Длительность анимации
                From = new Thickness(-381, 0, 0, 0),            // Начальное положение
                To = new Thickness(0, 0, 0, 0)     // Конечное положение (меню выдвигается)
            };
            //Анимация затемнения фона
            var DimmingMenuanimation = new DoubleAnimation
            {
                From = 0.0,                         // Начальное значение прозрачности
                To = 15.0,                           // Конечное значение прозрачности
                Duration = TimeSpan.FromSeconds(2)  // Длительность анимации
            };

            
            leftPanel.Visibility = Visibility.Visible;
            // Запускаем анимацию
            leftPanelFon.BeginAnimation(UIElement.OpacityProperty, DimmingMenuanimation);
            leftMenuPanel.BeginAnimation(MarginProperty, PromotionLeftMenuanimation);
        }
        private void CloseLeftMenu()
        {
            if (leftPanel.Visibility == Visibility.Collapsed)
            {               
                return;
            }
            //Анимация выдвигание левого меню
            var PromotionLeftMenuanimation = new ThicknessAnimation
            {
                Duration = TimeSpan.FromSeconds(0.2), // Длительность анимации
                From = new Thickness(0, 0, 0, 0),            // Начальное положение
                To = new Thickness(-381, 0, 0, 0)     // Конечное положение (меню выдвигается)
            };
            //Анимация затемнения фона
            var DimmingMenuanimation = new DoubleAnimation
            {
                From = 1.0,                         // Начальное значение прозрачности
                To = 0.0,                           // Конечное значение прозрачности
                Duration = TimeSpan.FromSeconds(0.2)  // Длительность анимации

            };
            RightMenuCocoaPanel.Visibility = Visibility.Collapsed;
            RightMenuTeaPanel.Visibility = Visibility.Collapsed;
            RightMenuCoffePanel.Visibility = Visibility.Collapsed;
            TeaTB.Foreground = Brushes.Black;
            CoffeTB.Foreground = Brushes.Black;
            CocoaTB.Foreground = Brushes.Black;
            DimmingMenuanimation.Completed += (sender, e) =>
            {
                // Скрываем элемент leftPanel после завершения анимации
                leftPanel.Visibility = Visibility.Collapsed;
            };
            // Запускаем анимацию
            leftPanelFon.BeginAnimation(UIElement.OpacityProperty, DimmingMenuanimation);
            leftMenuPanel.BeginAnimation(MarginProperty, PromotionLeftMenuanimation);
            
        }

        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseLeftMenu();
        }

        private void Hover_TeaTB(object sender, MouseEventArgs e)
        {
            TeaTB.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x2F, 0xAC, 0x66));
            RightMenuTeaPanel.Visibility = Visibility.Visible;

            RightMenuCoffePanel.Visibility = Visibility.Collapsed;
            RightMenuCocoaPanel.Visibility = Visibility.Collapsed;
            CoffeTB.Foreground = Brushes.Black;
            CocoaTB.Foreground = Brushes.Black;
        }
        private void Hover_CoffeTB(object sender, MouseEventArgs e)
        {
            CoffeTB.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x2F, 0xAC, 0x66));
            RightMenuCoffePanel.Visibility = Visibility.Visible;

            RightMenuTeaPanel.Visibility = Visibility.Collapsed;
            RightMenuCocoaPanel.Visibility = Visibility.Collapsed;
            TeaTB.Foreground = Brushes.Black;
            CocoaTB.Foreground = Brushes.Black;
        }
        private void Hover_CocoaTB(object sender, MouseEventArgs e)
        {
            CocoaTB.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x2F, 0xAC, 0x66));
            RightMenuCocoaPanel.Visibility = Visibility.Visible;

            RightMenuTeaPanel.Visibility = Visibility.Collapsed;
            RightMenuCoffePanel.Visibility = Visibility.Collapsed;
            TeaTB.Foreground = Brushes.Black;
            CoffeTB.Foreground = Brushes.Black;
        }

        private void Panel_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock)
            {
                TextBlock textBlock = sender as TextBlock;
                textBlock.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x2F, 0xAC, 0x66));
            }
        }
        private void Panel_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock)
            {
                TextBlock textBlock = sender as TextBlock;
                textBlock.Foreground = Brushes.Black;
            }

        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BottomSortPanel1.Visibility = Visibility.Visible;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            BottomSortPanel1.Visibility = Visibility.Collapsed;
        }

        private void sortTB_MouseEnter(object sender, MouseEventArgs e)
        {
            if(sender is Border)
            {
                Border border =  sender as Border;
                border.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xED, 0xED, 0xED));
                
            }
        }

        private void sortTB_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border)
            {
                
                Border border = sender as Border;
                TextBlock tb = border.Child as TextBlock;
                if(tb.Text == mainsortTB.Text)
                {
                    return;
                }
                border.Background = Brushes.White;
            }

        }

        private void sortTB_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border)
            {
                Border border = sender as Border;
                TextBlock tb = border.Child as TextBlock;
                if (tb.Text == mainsortTB.Text)
                {
                    return;
                }
                mainsortTB.Text = tb.Text;
                
                sort(tb.Text);
                if(border.Name == "popularTB")
                {
                    expensiveTB.Background = Brushes.White; ;
                    cheapTB.Background = Brushes.White;
                }
                if (border.Name == "expensiveTB")
                {
                    popularTB.Background = Brushes.White; ;
                    cheapTB.Background = Brushes.White;
                }
                if (border.Name == "cheapTB")
                {
                    expensiveTB.Background = Brushes.White; ;
                    popularTB.Background = Brushes.White;

                }
                BottomSortPanel1.Visibility = Visibility.Collapsed;
            }
        }

        private async void PlusButtonImageAsync(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)sender).DataContext as ProductItem; // Замените YourItemType на ваш тип данных

            if (item.Quantity + item.MinUnit > item.QuantityInStock)
            {
                item.MaxQuantity = "Visible";

                await Task.Delay(3000);

                item.MaxQuantity = "Hidden";
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




        public class BasketItem
        {
            public int id { get; set; }
            public string Name { get; set; }
            public string ImageData { get; set; }

            public int DefaultCost { get; set; }
            public string Category { get; set; }
            public string Unit { get; set; }
            public int MinUnit { get; set; }
            public int QuantityInStock { get; set; }
            private int Quantity { get; set; }
            public int Cost { get; set; }
        }

        //конструктор ProductItem
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
            public string description { get; set; }
            public string cooking_method { get; set; }
            public string taste_and_aroma { get; set; }
            private string maxQuantity;

            public string MaxQuantity
            {
                get { return maxQuantity; }
                set
                {
                    if (maxQuantity != value)
                    {
                        maxQuantity = value;
                        OnPropertyChanged(nameof(MaxQuantity));
                    }
                }
            }

            public int BasketQuantity { get; set; }
        }
    }
}
