using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static Tea_Coffe.Window1;

namespace Tea_Coffe
{
    /// <summary>
    /// Логика взаимодействия для FullProductInfo.xaml
    /// </summary>
    public partial class FullProductInfo : Window
    {
        readonly ProductItem productItem = new ProductItem();

        readonly Window1 mainWindow;
        // Конструктор для инициализации окна с подробной информацией о продукте
        public FullProductInfo(ProductItem item, Window1 main, string role)
        {
            InitializeComponent();
            mainWindow = main;
            try
            {
                productItem = item;

                name.Text = productItem.Name;
                cost.Text = productItem.Cost.ToString();

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(productItem.ImageData);
                bitmap.EndInit();

                image.Source = bitmap;

                Quantity.Content = productItem.Quantity;
                unit.Content = productItem.Unit;

                descriptionHead.Visibility = Visibility.Collapsed;
                cooking_methodHead.Visibility = Visibility.Collapsed;
                taste_and_aromaHead.Visibility = Visibility.Collapsed;
                description.Visibility = Visibility.Collapsed;
                cooking_method.Visibility = Visibility.Collapsed;
                taste_and_aroma.Visibility = Visibility.Collapsed;
                if (productItem.Description != null && productItem.Description != "")
                {
                    descriptionHead.Visibility = Visibility.Visible;
                    description.Visibility = Visibility.Visible;
                    description.Text = productItem.Description;
                }
                if (productItem.Cooking_method != null && productItem.Cooking_method != "")
                {
                    cooking_methodHead.Visibility = Visibility.Visible;
                    cooking_method.Visibility = Visibility.Visible;
                    cooking_method.Text = productItem.Cooking_method;
                }
                if (productItem.Taste_and_aroma != null && productItem.Taste_and_aroma != "")
                {
                    taste_and_aromaHead.Visibility = Visibility.Visible;
                    taste_and_aroma.Visibility = Visibility.Visible;
                    taste_and_aroma.Text = productItem.Taste_and_aroma;
                }

                if (role != "admin")
                {
                    AddToBasketGrid.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Закрытие окна при нажатии на изображение
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        // Увеличение количества продукта при нажатии кнопки "+"
        private async void PlusButton(object sender, MouseButtonEventArgs e)
        {
            if (productItem.Quantity + productItem.MinUnit > productItem.QuantityInStock)
            {
                MaxQuantity.Visibility = Visibility.Visible;
                MaxQuantityText.Text = $"Сейчас в наличии только {productItem.Quantity}{productItem.Unit}";
                await Task.Delay(3000);

                MaxQuantity.Visibility = Visibility.Hidden;
                return;
            }
            // Увеличиваем вес на 50 г
            productItem.Quantity += productItem.MinUnit;
            productItem.Cost += productItem.DefaultCost;
            Quantity.Content = productItem.Quantity;
            cost.Text = productItem.Cost.ToString();



        }
        // Уменьшение количества продукта при нажатии кнопки "-"
        private void MinusButton(object sender, MouseButtonEventArgs e)
        {
            if (productItem.Quantity == productItem.MinUnit)
            {
                return;
            }
            // Увеличиваем вес на 50 г
            productItem.Quantity -= productItem.MinUnit;
            productItem.Cost -= productItem.DefaultCost;
            Quantity.Content = productItem.Quantity;
            cost.Text = productItem.Cost.ToString();
        }
        // Добавление продукта в корзину
        private void InBasket(object sender, RoutedEventArgs e)
        {
            try
            {

                var existingItem = mainWindow.Basket.FirstOrDefault(p => p.Id == productItem.Id);

                if (existingItem != null)
                {
                    // Увеличиваем количество товара в корзине
                    if (existingItem.BasketQuantity + productItem.Quantity > productItem.QuantityInStock)
                    {
                        productItem.BasketQuantity = productItem.QuantityInStock;
                    }
                    else
                    {
                        productItem.BasketQuantity += productItem.Quantity;
                    }

                }
                else
                {
                    // Добавляем новый товар в корзину
                    productItem.BasketQuantity = productItem.Quantity;
                    mainWindow.Basket.Add(productItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
