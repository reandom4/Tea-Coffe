using Microsoft.Win32;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace Tea_Coffe
{
    /// <summary>
    /// Логика взаимодействия для Backup.xaml
    /// </summary>
    public partial class Backup : Window
    {
        readonly DataBase dataBase = new DataBase();
        public Backup()
        {
            InitializeComponent();

        }
        // Обработчик события для кнопки создания бэкапа
        private void CreateBackup(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "SQL Files (*.sql)|*.sql|All files (*.*)|*.*",
                    Title = "Save a File",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    // Здесь можно сохранить файл по пути filePath
                    dataBase.Backup(filePath);
                    MessageBox.Show("Бэкап успешно создан", "Backup", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Обработчик события для кнопки загрузки бэкапа
        private void DownloadBackup(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "SQL Files (*.sql)|*.sql"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    // Получаем путь к выбранному файлу
                    string file = openFileDialog.FileName;
                    dataBase.Restore(file);
                    MessageBox.Show("Бэкап успешно загружен", "Backup", MessageBoxButton.OK, MessageBoxImage.Information);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateTable(object sender, RoutedEventArgs e)
        {
            string db = reportTypeComboBox.Text;
            string dbname = "";
            if (db == "Товар")
                dbname = "tea_coffe.product";
            if (db == "Пользователи")
                dbname = "tea_coffe.user";
            if (db == "Заказы")
                dbname = "tea_coffe.order";
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                    Title = "Save a File",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    // Здесь можно сохранить файл по пути filePath
                    dataBase.Outputcsv(dbname, filePath);
                    MessageBox.Show($"таблица {db} успешно выгружена");
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DownloadTable(object sender, RoutedEventArgs e)
        {
            try
            {
                string db = reportTypeComboBox.Text;
                string dbname = "";
                if (db == "Товар")
                    dbname = "tea_coffe.product";
                if (db == "Пользователи")
                    dbname = "tea_coffe.user";
                if (db == "Заказы")
                    dbname = "tea_coffe.order";
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "CSV files (*.csv)|*.csv"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    // Получаем путь к выбранному файлу
                    string file = openFileDialog.FileName;
                    dataBase.ImportCsvToDatabase(file, dbname);
                    MessageBox.Show($"таблица {db} успешно загружена");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
