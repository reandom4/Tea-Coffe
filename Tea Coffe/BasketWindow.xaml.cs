using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using static Tea_Coffe.Window1;

namespace Tea_Coffe
{
    /// <summary>
    /// Логика взаимодействия для BasketWindow.xaml
    /// </summary>
    public partial class BasketWindow : Window
    {
        readonly DataBase DataBase = new DataBase();
        readonly WordHelper wordHelper = new WordHelper();
        readonly Window1 Window1;
        readonly int userid = 0;
        public BasketWindow(List<ProductItem> items, Window1 main, int id)
        {
            InitializeComponent();


            this.MouseMove += new MouseEventHandler(main.OnUserActivity);
            this.KeyDown += new KeyEventHandler(main.OnUserActivity);
            try
            {
                userid = id;
                ProductView.ItemsSource = items;
                Count_menu(items);
                Window1 = main;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Обработчик события для закрытия окна при нажатии на изображение
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();

        }
        // Обработчик события для увеличения количества продукта в корзине
        private async void PlusButton(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var item = ((FrameworkElement)sender).DataContext as ProductItem;
                List<ProductItem> pr = ProductView.ItemsSource as List<ProductItem>;
                if (item.BasketQuantity + item.MinUnit > item.QuantityInStock)
                {
                    item.MaxBasketQuantity = "Visible";

                    await Task.Delay(3000);

                    item.MaxBasketQuantity = "Hidden";
                    return;
                }
                // Увеличиваем вес на 50 г
                item.BasketQuantity += item.MinUnit;
                item.BasketCost += item.DefaultCost;
                Count_menu(pr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        // Обработчик события для уменьшения количества продукта в корзине
        private void MinusButton(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var item = ((FrameworkElement)sender).DataContext as ProductItem;
                List<ProductItem> pr = ProductView.ItemsSource as List<ProductItem>;
                if (item.BasketQuantity == item.MinUnit)
                {
                    return;
                }
                // Увеличиваем вес на 50 г
                item.BasketQuantity -= item.MinUnit;
                item.BasketCost -= item.DefaultCost;
                Count_menu(pr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        // Обновляет отображение общей стоимости и количества товаров в корзине
        private void Count_menu(List<ProductItem> productItem)
        {
            int FullCost = 0;
            try
            {
                foreach (var item in productItem)
                {
                    FullCost += item.BasketCost;
                }

                FullBasketCost1.Text = FullCost.ToString() + "₽";
                FullBasketCost2.Text = FullCost.ToString() + "₽";
                if (productItem.Count == 1)
                {
                    FullBasketQuantity.Text = productItem.Count.ToString() + " товар";
                }
                else if (productItem.Count >= 2 && productItem.Count <= 4)
                {
                    FullBasketQuantity.Text = productItem.Count.ToString() + " товара";
                }
                else
                {
                    FullBasketQuantity.Text = productItem.Count.ToString() + " товаров";
                }
                if (productItem.Count == 0)
                {
                    emptycart.Visibility = Visibility.Visible;
                    OrderButton.IsEnabled = false;
                }
                else
                {
                    emptycart.Visibility = Visibility.Collapsed;
                    OrderButton.IsEnabled = true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        // Обработчик события для удаления продукта из корзины
        private void DeleteBasketItem(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var item = ((FrameworkElement)sender).DataContext as ProductItem;
                MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить {item.Name}?", "Подтверждение удаления", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    List<ProductItem> pr = ProductView.ItemsSource as List<ProductItem>;
                    pr.Remove(item);

                    ProductView.ItemsSource = null;
                    ProductView.ItemsSource = pr;
                    Count_menu(pr);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        // Обработчик события для оформления заказа
        private void AddOrderButton(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ProductItem> pr = ProductView.ItemsSource as List<ProductItem>;
                DateTime dateTime = DateTime.Now;
                bool result = DataBase.AddOrder(pr, userid, dateTime);
                if (result)
                {

                    wordHelper.Creatcheque(pr, dateTime, userid);
                    pr.Clear();
                    Window1.Showdata();
                    MessageBox.Show("Заказ успешно оформлен");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("При оформлении заказа произошла ошибка");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Обновление таймера при активности

    }
}
