using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Tea_Coffe
{
    /// <summary>
    /// Логика взаимодействия для AddChangeUser.xaml
    /// </summary>
    public partial class AddChangeUser : Window
    {
        readonly DataBase dataBase = new DataBase();// Создание экземпляра базы данных
        readonly User startuser = null;// Исходный пользователь
        public AddChangeUser(User user = null)
        {
            InitializeComponent();
            startuser = user;// Сохранение переданного пользователя
            if (user == null)
            {
                AddButton.Visibility = Visibility.Visible;// Если пользователь не передан, показать кнопку добавления
            }
            else
            {
                ChangeButton.Visibility = Visibility.Visible;// Если пользователь передан, показать кнопку изменения
                Init(user);// Инициализация полей формы данными пользователя
            }
            
        }
        // Инициализация полей формы данными пользователя
        private void Init(User user)
        {
            name.Text = user.Name;
            surname.Text = user.Surname;
            patronymic.Text = user.Patronymic;
            login.Text = user.Login;
            role.Text = user.Role;
        }
        // Обработка события изменения пользователя
        private void Change(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создание нового объекта пользователя с обновленными данными
                User user = new User
                {
                    IdUser = startuser.IdUser,
                    Name = name.Text,
                    Surname = surname.Text,
                    Patronymic = patronymic.Text,
                    Password = password.Text,
                    Login = login.Text,
                    Role = role.Text
                };
                // Проверка заполненности обязательных полей
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
                if (user.Password.Length == 1)
                {
                    MessageBox.Show("Пароль не заполнен", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dataBase.ChangeUser(user);

                MessageBox.Show("Пользователь успешно изменен", "", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        // Обработка события добавления пользователя
        private void Add(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создание нового объекта пользователя с обновленными данными
                User user = new User
                {
                    Name = name.Text,
                    Surname = surname.Text,
                    Patronymic = patronymic.Text,
                    Password = password.Text,
                    Login = login.Text,
                    Role = role.Text
                };
                // Проверка заполненности обязательных полей
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
        // Обработка события предварительного ввода текста в TextBox для проверки русских букв
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
        // Генерация случайного пароля
        private void GeneratePass(object sender, RoutedEventArgs e)
        {
            password.Text = GeneratePassword(8);
        }
        // Метод генерации случайного пароля заданной длиной
        private string GeneratePassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%&";
            StringBuilder password = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(validChars.Length);
                password.Append(validChars[index]);
            }

            return password.ToString();
        }
        // Обработка события изменения текста в TextBox для автоматического преобразования первой буквы в верхний регистр
        private void Name_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!(sender is TextBox textBox) || string.IsNullOrEmpty(textBox.Text))
                return;

            int caretIndex = textBox.CaretIndex;

            string text = textBox.Text;
            if (text.Length > 0)
            {
                text = char.ToUpper(text[0]) + text.Substring(1);
                textBox.Text = text;

                textBox.CaretIndex = caretIndex;
            }
        }


    }
}
