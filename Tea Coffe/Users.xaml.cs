﻿using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для Users.xaml
    /// </summary>
    public partial class Users : Window
    {
        readonly DataBase dataBase = new DataBase();
        public Users()
        {
            InitializeComponent();
            try
            {
                DataTable dt = dataBase.LoadUsers();
                List<User> users = new List<User>();
                foreach (DataRow dr in dt.Rows)
                {
                    User item = new User
                    {
                        // Присвоение значения свойствам из данных строки таблицы
                        IdUser = Convert.ToInt32(dr["idUser"]),
                        Login = (string)dr["login"],
                        Password = dr["password"].ToString(),
                        Salt = dr["salt"].ToString(),
                        Surname = dr["surname"].ToString(),
                        Name = dr["name"].ToString(),
                        Patronymic = dr["patronymic"].ToString(),
                        Role = dr["User_roleName"].ToString(),
                        NPS = $"{dr["name"]} {dr["patronymic"]} {dr["surname"].ToString().ToCharArray()[0]}."

                    };
                    users.Add(item);
                }
                ProductView.ItemsSource = users;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Refresh()
        {
            SearchTB.Text = string.Empty;
            DataTable dt = dataBase.LoadUsers();
            List<User> users = new List<User>();
            foreach (DataRow dr in dt.Rows)
            {
                User item = new User
                {
                    // Присвоение значения свойствам из данных строки таблицы
                    IdUser = Convert.ToInt32(dr["idUser"]),
                    Login = (string)dr["login"],
                    Password = dr["password"].ToString(),
                    Salt = dr["salt"].ToString(),
                    Surname = dr["surname"].ToString(),
                    Name = dr["name"].ToString(),
                    Patronymic = dr["patronymic"].ToString(),
                    Role = dr["User_roleName"].ToString(),
                    NPS = $"{dr["name"]} {dr["patronymic"]} {dr["surname"].ToString().ToCharArray()[0]}."

                };
                users.Add(item);
            }
            ProductView.ItemsSource = null;
            ProductView.ItemsSource = users;
        }

        private void Search(object sender, TextChangedEventArgs e)
        {

        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = ((FrameworkElement)sender).DataContext as User;
            AddChangeUser addChangeUser = new AddChangeUser(item);
            addChangeUser.ShowDialog();
            Refresh();
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var item = ((FrameworkElement)sender).DataContext as User;
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить этого пользователя?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    dataBase.DeleteUser(item);
                    MessageBox.Show("Пользователь успешно удален", "Подтверждение удаления", MessageBoxButton.OK, MessageBoxImage.Information);
                    Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void AddUser(object sender, MouseButtonEventArgs e)
        {
            AddChangeUser addChangeUser = new AddChangeUser();
            addChangeUser.ShowDialog();
            Refresh();
        }
    }

    public class User
    {
        public int IdUser { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Role { get; set; }

        public string NPS { get; set; }
    }
}
