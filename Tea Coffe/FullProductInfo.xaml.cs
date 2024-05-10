using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для FullProductInfo.xaml
    /// </summary>
    public partial class FullProductInfo : Window
    {
        ProductItem productItem = new ProductItem();
        public FullProductInfo(ProductItem item)
        {
            InitializeComponent();

            try
            {
                productItem = item;
                FullProductMenu.Visibility = Visibility.Visible;
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
                if (productItem.description != null && productItem.description != "")
                {
                    descriptionHead.Visibility = Visibility.Visible;
                    description.Visibility = Visibility.Visible;
                    description.Text = productItem.description;
                }
                if (productItem.cooking_method != null && productItem.cooking_method != "")
                {
                    cooking_methodHead.Visibility = Visibility.Visible;
                    cooking_method.Visibility = Visibility.Visible;
                    cooking_method.Text = productItem.cooking_method;
                }
                if (productItem.taste_and_aroma != null && productItem.taste_and_aroma != "")
                {
                    taste_and_aromaHead.Visibility = Visibility.Visible;
                    taste_and_aroma.Visibility = Visibility.Visible;
                    taste_and_aroma.Text = productItem.taste_and_aroma;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

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
    }
}
