using Microsoft.Win32;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Timers;
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
        readonly DataBase dataBase = new DataBase();
        readonly ProductItem item = null;
        public ChangeRemoveProduct(ProductItem productItem)
        {
            InitializeComponent();
            try
            {
                Init(productItem);
                item = productItem;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        // Обработчик для закрытия окна при нажатии на изображение
        private void Close(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        // Обработчик для удаления продукта из базы данных
        private void RemoveProduct(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить этот товар?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    dataBase.RemoveProduct(item.Id.ToString());
                    MessageBox.Show("Товар успешно удален", "Подтверждение удаления", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }
        // Обработчик для изменения данных продукта
        private void ChangeProduct(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите изменить этот товар?", "Подтверждение изменения", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {

                    ProductItem productItem = new ProductItem
                    {
                        Name = name.Text
                    };
                    if (productItem.Name == "")
                    {
                        MessageBox.Show("Наименование не заполненно");
                        return;
                    }
                    productItem.Description = description.Text;
                    if (productItem.Name == "")
                    {
                        MessageBox.Show("Описание не заполненно");
                        return;
                    }

                    if (cost.Text == "0" || cost.Text == "")
                    {
                        MessageBox.Show("Цена не заполненно");
                        return;
                    }
                    productItem.Cost = Convert.ToInt32(cost.Text);
                    productItem.Unit = unitComboBox.Text;
                    productItem.Category = categoryComboBox.Text;
                    productItem.Cooking_method = cooking_method.Text;
                    productItem.Taste_and_aroma = taste_and_aroma.Text;
                    productItem.ImageData = imgname;
                    productItem.Id = item.Id;
                    dataBase.ChangeProduct(productItem);
                    MessageBox.Show("Товар успешно изменен", "Подтверждение изменения", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        DataTable dt = new DataTable();
        private string imgname = "R.png";
        // Инициализирует элементы интерфейса данными продукта
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
        // Обработчик для добавления изображения продукта
        private void AddImage(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // Открываем диалоговое окно для выбора файла изображения
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All files (*.*)|*.*"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    // Получаем путь к выбранному файлу
                    string imagePath = openFileDialog.FileName;

                    FileInfo fileInfo = new FileInfo(imagePath);
                    long fileSizeInBytes = fileInfo.Length;
                    long fileSizeInKB = fileSizeInBytes / 1024; // переводим байты в килобайты

                    // Устанавливаем максимальный размер файла =
                    long maxSizeInKB = 3 * 1024;

                    if (fileSizeInKB > maxSizeInKB)
                    {
                        MessageBox.Show("Файл слишком большой. Максимальный размер файла: 3 МБ.");
                        return;
                    }
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
        // Преобразует Bitmap в ImageSource
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
        // Обработчик для ограничения ввода только числовых значений в текстовое поле
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
        // Обработчик для запрета ввода пробела в текстовое поле
        private void NumericTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Запрещаем использование пробела
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
        // Обработчик для изменения единицы измерения продукта
        private void ChangeUnit(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (unitComboBox.SelectedItem != null)
                {
                    if (unitComboBox.SelectedItem is DataRowView selectedRow)
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
