using Microsoft.Win32;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Tea_Coffe.Window1;

namespace Tea_Coffe
{
    /// <summary>
    /// Логика взаимодействия для ChangeRemoveProduct.xaml
    /// </summary>
    public partial class ChangeRemoveProduct : Window
    {
        DataBase dataBase = new DataBase();
        ProductItem item = null;
        public ChangeRemoveProduct(ProductItem productItem)
        {
            InitializeComponent();
            try
            {
                Init(productItem);
            }
            catch
            { 
            
            }
            item = productItem;
        }

        private void Close(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void RemoveProduct(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить этот товар?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void ChangeProduct(object sender, RoutedEventArgs e)
        {

        }
        DataTable dt = new DataTable();
        private string imgname = "R.png";
        private void Init(ProductItem productItem)
        {
            name.Text = productItem.Name;
            cost.Text = productItem.Cost.ToString();

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(productItem.ImageData);
            bitmap.EndInit();

            image.Source = bitmap;
            imgname = System.IO.Path.GetFileName(productItem.ImageData);
            description.Text = productItem.Description;
            cooking_method.Text = productItem.Cooking_method;
            taste_and_aroma.Text = productItem.Taste_and_aroma;
            try
            {
                dt = dataBase.LoadData("products_unit");

                unitComboBox.ItemsSource = dt.DefaultView;
                unitComboBox.DisplayMemberPath = "Products_unitname";
                unitComboBox.SelectedValuePath = "Products_unitname";
                unitComboBox.SelectedValue = productItem.Unit;

                categoryComboBox.ItemsSource = dataBase.LoadData("product_category").DefaultView;
                categoryComboBox.DisplayMemberPath = "Product_categoryname";
                categoryComboBox.SelectedValuePath = "Product_categoryname";
                categoryComboBox.SelectedValue = productItem.Category;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AddImage(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Открываем диалоговое окно для выбора файла изображения
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    // Получаем путь к выбранному файлу
                    string imagePath = openFileDialog.FileName;

                    // Загружаем изображение
                    Bitmap bitmap = new Bitmap(imagePath);

                    // Устанавливаем изображение на элемент Image
                    image.Source = BitmapToImageSource(bitmap);

                    // Копируем изображение в нужную папку (здесь "C:\\Images" - пример пути)
                    string destinationFolder = Directory.GetCurrentDirectory() + "\\image\\";
                    string destinationPath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(imagePath));
                    if (!File.Exists(destinationPath))
                    {
                        File.Copy(imagePath, destinationPath);
                        imgname = System.IO.Path.GetFileName(imagePath);
                    }
                    else
                    {
                        imgname = System.IO.Path.GetFileName(imagePath);
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private ImageSource BitmapToImageSource(Bitmap bitmap)
        {
            MemoryStream memory = new MemoryStream();
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
            memory.Position = 0;

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            return bitmapImage;
        }
        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true; // Отменить ввод, если символ не является цифрой
                    break;
                }
            }
        }
        private void NumericTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Запрещаем использование пробела
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void ChangeUnit(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (unitComboBox.SelectedItem != null)
                {
                    DataRowView selectedRow = unitComboBox.SelectedItem as DataRowView;
                    if (selectedRow != null)
                    {
                        // Обновляем текст в TextBlock
                        quantityTextBlock.Text = selectedRow["products_unitcol"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
