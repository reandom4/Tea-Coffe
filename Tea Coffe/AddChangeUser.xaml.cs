using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddChangeUser.xaml
    /// </summary>
    public partial class AddChangeUser : Window
    {
        readonly DataBase dataBase = new DataBase();
        User startuser = null;
        public AddChangeUser(User user = null)
        {
            InitializeComponent();
            
            startuser = user;
            if(user == null)
            {
                AddButton.Visibility = Visibility.Visible;
            }
            else
            {
                ChangeButton.Visibility = Visibility.Visible;
                Init(user);
            }
        }

        private void Init(User user)
        {
            name.Text = user.Name;
            surname.Text = user.Surname;
            patronymic.Text = user.Patronymic;
            login.Text = user.Login;
            role.Text = user.Role;
        }
        private void Change(object sender, RoutedEventArgs e)
        {
            try
            {
                User user = new User();
                user.IdUser = startuser.IdUser;
                user.Name = name.Text;
                user.Surname = surname.Text;
                user.Patronymic = patronymic.Text;
                user.Password = password.Text;
                user.Login = login.Text;
                user.Role = role.Text;
                if(user.Name .Length <=2  ) 
                {
                    MessageBox.Show("Имя не заполнено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (user.Surname.Length <= 2)
                {
                    MessageBox.Show("Фамилия не заполнен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (user.Login.Length <= 2)
                {
                    MessageBox.Show("Логин не заполнен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (user.Password.Length <= 2)
                {
                    MessageBox.Show("Пароль не заполнен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dataBase.ChangeUser(user);

                MessageBox.Show("Пользователь успешно изменен", "", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Add(object sender, RoutedEventArgs e)
        {
            try
            {
                User user = new User();
                user.Name = name.Text;
                user.Surname = surname.Text;
                user.Patronymic = patronymic.Text;
                user.Password = password.Text;
                user.Login = login.Text;
                user.Role = role.Text;
                if (user.Name.Length <= 2)
                {
                    MessageBox.Show("Имя не заполнено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (user.Surname.Length <= 2)
                {
                    MessageBox.Show("Фамилия не заполнен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (user.Login.Length <= 2)
                {
                    MessageBox.Show("Логин не заполнен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (user.Password.Length <= 2)
                {
                    MessageBox.Show("Пароль не заполнен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dataBase.CreateUser(user);

                MessageBox.Show("Пользователь успешно добавлен", "", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Регулярное выражение для проверки русских букв
            Regex regex = new Regex(@"^[А-Яа-яёЁ]+$");

            // Проверяем введенный символ
            if (!regex.IsMatch(e.Text))
            {
                // Если символ не русский, отменяем ввод
                e.Handled = true;
            }
        }

        
    }
}
