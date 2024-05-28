using Microsoft.Win32;
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

        private void CreateBackup(object sender, RoutedEventArgs e)
        {
            try
            {
                dataBase.Backup();
                MessageBox.Show("Бэкап успешно создан","Backup",MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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
    }
}
