using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Tea_Coffe
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        int currenttab = 0;
        int Alltab = 1;
        readonly int itemontab = 20;
        readonly int userid = 0;


        readonly DataBase dataBase = new DataBase();
        private readonly string curRole = null;
        public Window1(string login, string role)
        {
            InitializeComponent();
            curRole = role;

            Showdata();
            if (role == "storekeeper")
            {
                Storage.Visibility = Visibility.Visible;
            }
            if (role == "admin")
            {
                Users.Visibility = Visibility.Visible;
                Reports.Visibility = Visibility.Visible;
                AddProductImage.Visibility = Visibility.Visible;
                BD.Visibility = Visibility.Visible;
            }
            if (role == "cashier")
            {
                BasketImage.Visibility = Visibility.Visible;
                Orders.Visibility = Visibility.Visible;
            }

            try
            {
                userid = dataBase.GetUserId(login);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            InitializeInactivityTimer();
            this.MouseMove += new MouseEventHandler(OnUserActivity);
            this.KeyDown += new KeyEventHandler(OnUserActivity);
        }
        // Метод для отображения данных продуктов
        public void Showdata()
        {
            try
            {
                //DataTable dataTable = new DataTable();
                //dataTable = dataBase.SearchProducts("", "Популярные");

                //List<ProductItem> productList = new List<ProductItem>();


                //foreach (DataRow row in dataTable.Rows)
                //{
                //    // Создание нового объекта ProductItem
                //    ProductItem item = new ProductItem
                //    {
                //        // Присвоение значения свойствам из данных строки таблицы
                //        Id = Convert.ToInt32(row["idProducts"]),
                //        Name = row["name"].ToString(),
                //        ImageData = System.IO.Directory.GetCurrentDirectory() + "\\image\\" + row["photo"].ToString(),
                //        Cost = Convert.ToInt32(row["cost"]),
                //        DefaultCost = Convert.ToInt32(row["cost"]),
                //        Unit = row["Products_unitname"].ToString(),
                //        MinUnit = Convert.ToInt32(row["products_unitcol"]),
                //        Quantity = Convert.ToInt32(row["products_unitcol"]),
                //        QuantityInStock = Convert.ToInt32(row["quantity"]),
                //        Category = row["Product_categoryname"].ToString(),
                //        AllowChange = "Collapsed",
                //        AllowBasket = "Collapsed"

                //    };
                //    if (item.QuantityInStock < item.MinUnit)
                //    {
                //        item.Quantity = 0;
                //        if (curRole != "admin")
                //        {
                //            continue;
                //        }
                //    }
                //    if (curRole == "admin")
                //    {
                //        item.AllowChange = "Visible";

                //    }
                //    if (curRole == "cashier")
                //    {
                //        item.AllowBasket = "Visible";
                //    }
                //    // Добавление объекта ProductItem в список
                //    productList.Add(item);
                //    item.Description = row["description"].ToString();
                //    item.Cooking_method = row["cooking_method"].ToString();
                //    item.Taste_and_aroma = row["taste_and_aroma"].ToString();
                //    item.MaxQuantity = "Collapsed";

                //}


                //ProductView.ItemsSource = null;
                //ProductView.ItemsSource = productList;
                //Productcount.Text = $"найдено {productList.Count}";

                CurrentTabTB.Text = $"Все товары";
                mainsortTB.Text = "Популярные";
                expensiveTB.Background = Brushes.White; ;
                cheapTB.Background = Brushes.White;
                popularTB.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xED, 0xED, 0xED));

                Paggination();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        // Метод для отображения данных продуктов
        void Showdata(DataTable dataTable)
        {

            try
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
                        QuantityInStock = Convert.ToInt32(row["quantity"]),
                        Category = row["Product_categoryname"].ToString(),
                        AllowChange = "Collapsed",
                        AllowBasket = "Collapsed"
                    };
                    if (item.QuantityInStock < item.MinUnit)
                    {
                        item.Quantity = 0;
                        if (curRole != "admin")
                        {
                            continue;
                        }
                    }
                    if (curRole == "admin")
                    {
                        item.AllowChange = "Visible";

                    }
                    if (curRole == "cashier")
                    {
                        item.AllowBasket = "Visible";
                    }
                    // Добавление объекта ProductItem в список
                    productList.Add(item);
                    item.Description = row["description"].ToString();
                    item.Cooking_method = row["cooking_method"].ToString();
                    item.Taste_and_aroma = row["taste_and_aroma"].ToString();
                    item.MaxQuantity = "Collapsed";
                }


                ProductView.ItemsSource = productList;
                //Productcount.Text = $"найдено {productList.Count}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public List<ProductItem> Basket = new List<ProductItem>();
        // Метод для добавления продукта в корзину
        private void InBasketButton(object sender, RoutedEventArgs e)
        {
            try
            {
                ProductItem item = ((FrameworkElement)sender).DataContext as ProductItem;

                var existingItem = Basket.FirstOrDefault(p => p.Id == item.Id);

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
                    item.BasketCost = item.BasketQuantity * item.DefaultCost / item.MinUnit;
                }
                else
                {
                    // Добавляем новый товар в корзину
                    item.BasketQuantity = item.Quantity;
                    item.BasketCost = item.BasketQuantity * item.DefaultCost / item.MinUnit;
                    Basket.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        FullProductInfo fullProductInfo;
        // Метод для открытия полного описания продукта
        private void OpenFullProductMenu(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)sender).DataContext as ProductItem;

            fullProductInfo?.Close();

            fullProductInfo = new FullProductInfo(item, this, curRole);            
            fullProductInfo.Show();
            fullProductInfo.Activate();
            fullProductInfo.Focus();
        }
        // Метод для поиска и сортировки продуктов
        private void Search_Sort(object sender, TextChangedEventArgs e)
        {
            string search = SearchTB.Text;
            string filter = CurrentTabTB.Text;

            try
            {
                if (search == "" && filter.Split(' ')[0] == "Результаты")
                {
                    CurrentTabTB.Text = "Все товары";
                }
                else if (CurrentTabTB.Text == "Все товары")
                {
                    CurrentTabTB.Text = $"Результаты по запросу «{search}»";
                }
                currenttab = 0;
                Paggination();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Метод для фильтрации продуктов
        private void Filtr_Sort(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            try
            {
                string Filtr = textBlock.Text;
                string sort = mainsortTB.Text;
                SearchTB.Text = "";
                try
                {
                    CurrentTabTB.Text = $"{Filtr}";
                    currenttab = 0;
                    Paggination();
                    CloseLeftMenu();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Метод для фильтрации больших категорий продуктов
        private void BigFiltr_Sort(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            try
            {
                string Filtr = textBlock.Text;
                string sort = mainsortTB.Text;
                SearchTB.Text = "";
                try
                {
                    CurrentTabTB.Text = $"{Filtr}";
                    currenttab = 0;
                    Paggination();
                    CloseLeftMenu();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Метод для отображения всех продуктов
        private void ShowallFiltr(object sender, MouseButtonEventArgs e)
        {

            SearchTB.Text = "";
            try
            {
                CurrentTabTB.Text = $"Все товары";
                currenttab = 0;
                Paggination();
                CloseLeftMenu();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        // Метод для фильтрации какао продуктов
        private void CacaoFiltr_Sort(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            try
            {
                string Filtr = textBlock.Text;

                try
                {
                    CurrentTabTB.Text = $"{Filtr}";
                    currenttab = 0;
                    Paggination();
                    CloseLeftMenu();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Метод для обработки прокрутки мыши на ListView
        private void ListView_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Предотвращаем прокрутку колесиком мыши для ListView
            e.Handled = false;
        }
        // Метод для установки стилей кнопок сортировки
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
        // Метод для закрытия левого меню
        private void ShowCatalogButton(object sender, RoutedEventArgs e)
        {
            if (leftPanel.Visibility == Visibility.Visible)
            {
                CloseLeftMenu();
                return;
            }
            OpenLeftMenu();
        }
        // Закрывает левое меню при клике на фон левой панели.
        private void LeftPanelFonClick(object sender, MouseButtonEventArgs e)
        {
            CloseLeftMenu();
        }
        // Анимирует открытие левого меню.
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
        // Анимирует закрытие левого меню.
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
        // Закрывает левое меню при нажатии на текстовое поле.
        private void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseLeftMenu();
        }
        // Изменяет цвет текстового блока при наведении мыши.
        private void Hover_AllTB(object sender, MouseEventArgs e)
        {
            TextBlock tb = sender as TextBlock;

            tb.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x2F, 0xAC, 0x66));
            RightMenuTeaPanel.Visibility = Visibility.Collapsed;

            RightMenuCoffePanel.Visibility = Visibility.Collapsed;
            RightMenuCocoaPanel.Visibility = Visibility.Collapsed;
            TeaTB.Foreground = Brushes.Black;
            CoffeTB.Foreground = Brushes.Black;
            CocoaTB.Foreground = Brushes.Black;
        }
        // Возвращает цвет текстового блока к изначальному после ухода курсора с него.
        private void AllTb_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            tb.Foreground = Brushes.Black;
        }
        // Изменяет цвет текстового блока при наведении мыши на "Чай".
        private void Hover_TeaTB(object sender, MouseEventArgs e)
        {
            TeaTB.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x2F, 0xAC, 0x66));
            RightMenuTeaPanel.Visibility = Visibility.Visible;

            RightMenuCoffePanel.Visibility = Visibility.Collapsed;
            RightMenuCocoaPanel.Visibility = Visibility.Collapsed;
            CoffeTB.Foreground = Brushes.Black;
            CocoaTB.Foreground = Brushes.Black;
        }
        // Изменяет цвет текстового блока при наведении мыши на "Кофе".
        private void Hover_CoffeTB(object sender, MouseEventArgs e)
        {
            CoffeTB.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x2F, 0xAC, 0x66));
            RightMenuCoffePanel.Visibility = Visibility.Visible;

            RightMenuTeaPanel.Visibility = Visibility.Collapsed;
            RightMenuCocoaPanel.Visibility = Visibility.Collapsed;
            TeaTB.Foreground = Brushes.Black;
            CocoaTB.Foreground = Brushes.Black;
        }
        // Изменяет цвет текстового блока при наведении мыши на "Какао".
        private void Hover_CocoaTB(object sender, MouseEventArgs e)
        {
            CocoaTB.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x2F, 0xAC, 0x66));
            RightMenuCocoaPanel.Visibility = Visibility.Visible;

            RightMenuTeaPanel.Visibility = Visibility.Collapsed;
            RightMenuCoffePanel.Visibility = Visibility.Collapsed;
            TeaTB.Foreground = Brushes.Black;
            CoffeTB.Foreground = Brushes.Black;
        }
        // Изменяет цвет текстового блока при наведении мыши.
        private void Panel_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock)
            {
                TextBlock textBlock = sender as TextBlock;
                textBlock.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x2F, 0xAC, 0x66));
            }
        }
        // Возвращает цвет текстового блока к изначальному после ухода курсора с него.
        private void Panel_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is TextBlock)
            {
                TextBlock textBlock = sender as TextBlock;
                textBlock.Foreground = Brushes.Black;
            }

        }
        // Отображает панель сортировки при нажатии на сетку.
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BottomSortPanel1.Visibility = Visibility.Visible;
        }
        // Скрывает панель сортировки при уходе курсора с сетки.
        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            BottomSortPanel1.Visibility = Visibility.Collapsed;
        }
        // Изменяет цвет границы текстового блока при наведении мыши.
        private void SortTB_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border)
            {
                Border border = sender as Border;
                border.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xED, 0xED, 0xED));

            }
        }
        // Возвращает цвет границы текстового блока к изначальному после ухода курсора с него.
        private void SortTB_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border)
            {

                Border border = sender as Border;
                TextBlock tb = border.Child as TextBlock;
                if (tb.Text == mainsortTB.Text)
                {
                    return;
                }
                border.Background = Brushes.White;
            }

        }
        // Обрабатывает клик по элементу сортировки.
        private void SortTB_Click(object sender, MouseButtonEventArgs e)
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


                if (border.Name == "popularTB")
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
            currenttab = 0;
            Paggination();
        }
        // Асинхронно обрабатывает увеличение количества продукта.
        private async void PlusButtonImageAsync(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)sender).DataContext as ProductItem;

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
        // Обрабатывает уменьшение количества продукта.
        private void MinusButtonImage(object sender, MouseButtonEventArgs e)
        {
            var item = ((FrameworkElement)sender).DataContext as ProductItem;

            if (item.Quantity == item.MinUnit)
            {
                return;
            }
            // Увеличиваем вес на 50 г
            item.Quantity -= item.MinUnit;
            item.Cost -= item.DefaultCost;
        }

        //конструктор ProductItem
        public class ProductItem : INotifyPropertyChanged
        {
            public int Id { get; set; }
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
            public string Description { get; set; }
            public string Cooking_method { get; set; }
            public string Taste_and_aroma { get; set; }
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

            private string MaxbasketQuantity = "Collapsed";
            public string MaxBasketQuantity
            {
                get { return MaxbasketQuantity; }
                set
                {
                    if (MaxbasketQuantity != value)
                    {
                        MaxbasketQuantity = value;
                        OnPropertyChanged(nameof(MaxbasketQuantity));
                    }
                }
            }
            private int Basketquantity;
            public int BasketQuantity
            {
                get { return Basketquantity; }
                set
                {
                    if (Basketquantity != value)
                    {
                        Basketquantity = value;
                        OnPropertyChanged(nameof(Basketquantity));
                    }
                }
            }
            private int Basketcost;
            public int BasketCost
            {
                get { return Basketcost; }
                set
                {
                    if (Basketcost != value)
                    {
                        Basketcost = value;
                        OnPropertyChanged(nameof(Basketcost));
                    }
                }
            }
            public string AllowChange { get; set; }
            public string AllowBasket { get; set; }
            public int Total_quantity { get; set; }
        }


        BasketWindow basketWindow;
        // Отображает окно корзины с полным списком продуктов.
        private void ShowFullBasket(object sender, MouseButtonEventArgs e)
        {
            basketWindow?.Close();

            basketWindow = new BasketWindow(Basket, this, userid);
            
            basketWindow.Show();
            basketWindow.Activate();
            basketWindow.Focus();

        }
        // Закрывает окно корзины и другие окна при закрытии главного окна.
        private void Window_Closed(object sender, EventArgs e)
        {

            basketWindow?.Close();
            fullProductInfo?.Close();
        }
        // Открывает окно добавления продукта.
        private void AddProductButton(object sender, MouseButtonEventArgs e)
        {
            AddProduct addProduct = new AddProduct();
            addProduct.ShowDialog();
            Paggination();
        }
        // Открывает окно изменения продукта.
        private void ChangeProduct(object sender, RoutedEventArgs e)
        {
            var item = ((FrameworkElement)sender).DataContext as ProductItem;
            ChangeRemoveProduct changeRemoveProduct = new ChangeRemoveProduct(item);
            changeRemoveProduct.ShowDialog();
            Paggination();
        }
        // Открывает окно хранилища.
        private void OpenStorage(object sender, MouseButtonEventArgs e)
        {
            CloseLeftMenu();
            StorageWindow storageWindow = new StorageWindow();
            storageWindow.ShowDialog();

        }
        // Открывает окно пользователей.
        private void ShowUsers(object sender, MouseButtonEventArgs e)
        {
            CloseLeftMenu();
            Users users = new Users();
            users.ShowDialog();
        }
        // Открывает окно отчетов.
        private void ShowReports(object sender, MouseButtonEventArgs e)
        {
            ReportGenerator reportGenerator = new ReportGenerator();
            reportGenerator.ShowDialog();
        }
        // Открывает окно резервного копирования.
        private void ShowBD(object sender, MouseButtonEventArgs e)
        {
            Backup backup = new Backup();
            backup.ShowDialog();
        }
        // Открывает окно заказов.
        private void OpenOrder(object sender, MouseButtonEventArgs e)
        {
            ShowOrders showOrders = new ShowOrders();
            showOrders.ShowDialog();
        }
        // Пагинация - переходит к предыдущей странице.
        private void PagLeft(object sender, RoutedEventArgs e)
        {
            currenttab--;
            Paggination();
            scrollViewer.ScrollToTop();
        }
        // Пагинация - переходит к следующей странице.
        private void PagRight(object sender, RoutedEventArgs e)
        {
            currenttab++;
            Paggination();
            scrollViewer.ScrollToTop();
        }
        // Выполняет пагинацию
        private void Paggination()
        {
            string filter = CurrentTabTB.Text;
            string search = SearchTB.Text;
            string sort = mainsortTB.Text;
            try
            {
                DataTable dt = new DataTable();
                dt = dataBase.BigFiltr(filter, sort, search, itemontab, currenttab);
                Showdata(dt);

                int allitems = dataBase.GetCount(filter, sort, search);
                Alltab = (int)Math.Ceiling((double)allitems / itemontab);
                Productcount.Text = $"найдено {allitems}";
                PagTB.Text = $"{currenttab + 1}/{Alltab}";
                if (Alltab == 0)
                {
                    pagGrid.Visibility = Visibility.Collapsed;
                }
                else
                {
                    pagGrid.Visibility = Visibility.Visible;
                }
                if (currenttab == 0)
                {
                    PagLeftButton.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xC7, 0xC7, 0xC7));
                    PagLeftButton.IsEnabled = false;
                }
                else
                {
                    PagLeftButton.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x2F, 0xAC, 0x66));
                    PagLeftButton.IsEnabled = true;
                }
                if (currenttab + 1 == Alltab)
                {
                    PagRightButton.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xC7, 0xC7, 0xC7));
                    PagRightButton.IsEnabled = false;
                }
                else
                {
                    PagRightButton.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x2F, 0xAC, 0x66));
                    PagRightButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Выполняет поиск по нажатию на кнопку
        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {
            Paggination();
            scrollViewer.ScrollToTop();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (MainWindow.Width < 1900)
            {
                if (this.FindName("ProductView") is ListView listView)
                {
                    // Создайте новый FrameworkElementFactory для UniformGrid
                    FrameworkElementFactory uniformGridFactory = new FrameworkElementFactory(typeof(UniformGrid));
                    uniformGridFactory.SetValue(UniformGrid.ColumnsProperty, 4);

                    // Создайте новый ItemsPanelTemplate, используя созданный FrameworkElementFactory
                    ItemsPanelTemplate newTemplate = new ItemsPanelTemplate(uniformGridFactory);

                    // Примените новый ItemsPanelTemplate к ListView
                    listView.ItemsPanel = newTemplate;
                }
            }
            if (MainWindow.Width > 1920)
            {
                if (this.FindName("ProductView") is ListView listView)
                {
                    // Создайте новый FrameworkElementFactory для UniformGrid
                    FrameworkElementFactory uniformGridFactory = new FrameworkElementFactory(typeof(UniformGrid));
                    uniformGridFactory.SetValue(UniformGrid.ColumnsProperty, 5);

                    // Создайте новый ItemsPanelTemplate, используя созданный FrameworkElementFactory
                    ItemsPanelTemplate newTemplate = new ItemsPanelTemplate(uniformGridFactory);

                    // Примените новый ItemsPanelTemplate к ListView
                    listView.ItemsPanel = newTemplate;
                }
            }
        }

        private void ChangeAccount(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите выйти?", "Подтверждение выхода", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    CloseAllWindowsAndOpenLogin();
                });
            }
        }

        private Timer _inactivityTimer;
        private const int InactivityLimit = 120000;

        private void InitializeInactivityTimer()
        {
            _inactivityTimer = new Timer(InactivityLimit);
            _inactivityTimer.Elapsed += OnInactivityTimerElapsed;
            _inactivityTimer.Start();
        }

        public void OnUserActivity(object sender, EventArgs e)
        {
            _inactivityTimer.Stop();
            _inactivityTimer.Start();
        }

        private void OnInactivityTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CloseAllWindowsAndOpenLogin();
            });
        }

        private void CloseAllWindowsAndOpenLogin()
        {
            _inactivityTimer.Stop();
            _inactivityTimer.Dispose();
            foreach (Window window in Application.Current.Windows)
            {
                if (window != this)
                {
                    window.Close();
                }
            }
            Authorization loginWindow = new Authorization();
            loginWindow.Show();
            this.Close();

            // Открываем окно авторизации
            
        }
    }
}
