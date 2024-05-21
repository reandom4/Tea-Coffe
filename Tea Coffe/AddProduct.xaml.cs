using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
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
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        readonly DataBase dataBase = new DataBase();
        public AddProduct()
        {
            InitializeComponent();
            Init();
        }

        private void Close(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private DataTable dt = new DataTable();
        private void Init()
        {
            try
            {
                dt = dataBase.LoadData("products_unit");

                unitComboBox.ItemsSource = dt.DefaultView;
                unitComboBox.DisplayMemberPath = "Products_unitname";
                unitComboBox.SelectedValuePath = "Products_unitname";

                // Устанавливаем начальное значение для ComboBox
                unitComboBox.SelectedValue = "г";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                    string destinationFolder = "C:\\Images";
                    string destinationPath = System.IO.Path.Combine(destinationFolder, System.IO.Path.GetFileName(imagePath));
                    File.Copy(imagePath, destinationPath, true);

                    MessageBox.Show("Изображение успешно выбрано и скопировано в папку: " + destinationFolder);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message);
            }
        }

        private System.Windows.Media.ImageSource BitmapToImageSource(Bitmap bitmap)
        {
            MemoryStream memory = new MemoryStream();
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
            memory.Position = 0;

            System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memory;
            bitmapImage.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
}
