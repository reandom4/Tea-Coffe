using System;
using System.Collections.Generic;
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
using System.Xml.Linq;
using static Tea_Coffe.Window1;

namespace Tea_Coffe
{
    /// <summary>
    /// Логика взаимодействия для BasketWindow.xaml
    /// </summary>
    public partial class BasketWindow : Window
    {
        DataBase DataBase = new DataBase();
        WordHelper wordHelper = new WordHelper();
        Window1 Window1;
        public BasketWindow(List<ProductItem> items, Window1 main)
        {
            InitializeComponent();
            try
            {
                ProductView.ItemsSource = items;
                count_menu(items);
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
                count_menu(pr);
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
                count_menu(pr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        // Обновляет отображение общей стоимости и количества товаров в корзине
        private void count_menu(List<ProductItem> productItem)
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
                if(productItem.Count == 1) 
                {
                    FullBasketQuantity.Text = productItem.Count.ToString() + " товар";
                }
                else if(productItem.Count >= 2 && productItem.Count <= 4)
                {
                    FullBasketQuantity.Text = productItem.Count.ToString() + " товара";
                }
                else
                {
                    FullBasketQuantity.Text = productItem.Count.ToString() + " товаров";
                }
                if(productItem.Count == 0)
                {
                    emptycart.Visibility = Visibility.Visible;
                }
                else
                { 
                    emptycart.Visibility = Visibility.Collapsed;
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
                    count_menu(pr);
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
                bool result = DataBase.AddOrder(pr, 2, dateTime);
                if(result)
                {
                    MessageBox.Show("Заказ успешно оформлен");
                    wordHelper.Creatcheque(pr, dateTime);
                    pr.Clear();
                    Window1.Showdata();
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
    }
}
