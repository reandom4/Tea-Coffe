using System;

using System.Drawing;
using System.IO;
using System.Linq;

using System.Windows;

using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Point = System.Drawing.Point;



namespace Tea_Coffe
{
    /// <summary>
    /// Логика взаимодействия для authorization.xaml
    /// </summary>

    public partial class Authorization : Window
    {
        readonly DataBase database = new DataBase();
        private string capt;
        private bool firsterr = false;
        public Authorization()
        {
            InitializeComponent();
            //
            //Window1 window1 = new Window1("admin", "admin");
            //window1.Show();
            //this.Close();
            //
        }
        // Обработчик события для закрытия окна при нажатии на изображение
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        // Обработчик события для кнопки входа
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = textBoxLogin.Text;
                string password = passwordBox.Password;
                if (passwordBox.Visibility == Visibility.Collapsed)
                {
                    password = PasswordTextBox.Text;
                }
                string role = database.ValidateLogin(login, password);
                if (role != null)
                {
                    Window1 window1 = new Window1(login, role);
                    window1.Show();
                    this.Close();
                }
                else
                {
                    ErrorMessage.Visibility = Visibility.Visible;
                    if (firsterr)
                    {
                        CaptchaPanel.Visibility = Visibility.Visible;
                        Refreshimage();
                    }
                    firsterr = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        // Обработчик события для отображения пароля
        private void ShowPassword(object sender, MouseButtonEventArgs e)
        {

            Keyboard.ClearFocus();
            PasswordTextBox.Visibility = Visibility.Visible;
            PasswordTextBox.Text = passwordBox.Password;
            passwordBox.Visibility = Visibility.Collapsed;
            ShowPassowrdIcon.Visibility = Visibility.Collapsed;
            HidePassowrdIcon.Visibility = Visibility.Visible;

        }
        // Обработчик события для очистки поля ввода логина
        private void ClearLogin(object sender, MouseButtonEventArgs e)
        {
            textBoxLogin.Text = string.Empty;
        }
        // Обработчик события для скрытия пароля
        private void HidePassword(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
            passwordBox.Visibility = Visibility.Visible;
            ShowPassowrdIcon.Visibility = Visibility.Visible;
            HidePassowrdIcon.Visibility = Visibility.Collapsed;
            passwordBox.Password = PasswordTextBox.Text;

            PasswordTextBox.Visibility = Visibility.Collapsed;

        }
        // Генерирует случайный текст для капчи
        private string GenerateRandomCaptcha()
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        // Создает изображение с текстом капчи
        private Bitmap CreateImage(int Width, int Height, string text)
        {
            Random rnd = new Random();

            //Генерируем текст


            //Создадим изображение
            Bitmap result = new Bitmap(Width, Height);

            //Добавим различные цвета ддя текста
            Brush[] colors = {
            Brushes.Black,
            Brushes.Red,
            Brushes.RoyalBlue,
            Brushes.Green,
            Brushes.Yellow,
            Brushes.White,
            Brushes.Tomato,
            Brushes.Sienna,
            Brushes.Pink };

            //Добавим различные цвета линий
            Color[] colorp = {
            Color.Black,
            Color.Red,
            Color.RosyBrown,
            Color.RoyalBlue,
            Color.Yellow,
            Color.Pink,
            Color.Sienna,
            Color.Green};

            //Добавим различные шрифт текста
            string[] fonts = {
            "Comic Sans MS",
            "Courier New",
            "Arial",
            "Ink Free",
            "Segoe Print",
            "Impact"};

            //Делаем случайный стиль текста
            System.Drawing.FontStyle[] fontstyle = {
            System.Drawing.FontStyle.Bold,
            System.Drawing.FontStyle.Italic,
            System.Drawing.FontStyle.Regular,
            System.Drawing.FontStyle.Strikeout,
            System.Drawing.FontStyle.Underline};

            //Добавим различные углы поворота текста
            Int16[] rotate = { 1, -1, 2, -2, 3, -3, 4, -4, 5, -5, 6, -6 };

            //Укажем где рисовать
            Graphics g = Graphics.FromImage((System.Drawing.Image)result);

            //Пусть фон картинки будет серым
            g.Clear(Color.Gray);

            //Делаем случайный угол поворота текста
            g.RotateTransform(rnd.Next(rotate.Length));

            //Нарисуем сгенирируемый текст
            g.DrawString(text, new Font(fonts[rnd.Next(fonts.Length)], 25, fontstyle[rnd.Next(fontstyle.Length)]), colors[rnd.Next(colors.Length)], new PointF(rnd.Next(10, 120), rnd.Next(10, 100)));

            //Добавим немного помех
            //Линии из углов

            g.DrawLine(new Pen(colorp[rnd.Next(colorp.Length)], rnd.Next(3, 7)), new Point(0, rnd.Next(Height - 1)), new Point(Width - 1, rnd.Next(Height - 1)));
            g.DrawLine(new Pen(colorp[rnd.Next(colorp.Length)], rnd.Next(3, 7)), new Point(0, rnd.Next(Height - 1)), new Point(Width - 1, rnd.Next(Height - 1)));

            //Белые точки
            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);

            return result;
        }
        // Обновляет изображение капчи при нажатии на кнопку обновления
        private void RefreshCaptcha(object sender, MouseButtonEventArgs e)
        {
            Refreshimage();
        }
        // Проверяет введенный текст капчи и скрывает панель капчи при правильном вводе
        private void Entercaptcha(object sender, RoutedEventArgs e)
        {
            if (CaptchaTB.Text == capt)
            {
                CaptchaPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                CaptchaTB.IsEnabled = false;
                captchabutton.IsEnabled = false;
                CaptchaErr.Visibility = Visibility.Visible;
                // Создать таймер
                DispatcherTimer timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(10)
                };
                timer.Tick += Timer_Tick;
                timer.Start();
                Refreshimage();
            }
        }
        // Разблокирует поле ввода капчи после истечения времени таймера
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Разблокировать поле для ввода
            CaptchaTB.IsEnabled = true;
            captchabutton.IsEnabled = true;
            CaptchaErr.Visibility = Visibility.Collapsed;
            // Остановить таймер
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= Timer_Tick;
        }
        // Обновляет изображение капчи
        private void Refreshimage()
        {
            capt = GenerateRandomCaptcha();
            Bitmap bitmap = CreateImage(300, 245, capt);

            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }

            // Создание объекта Image и установка его источника

            //System.Windows.Controls.Image imageControl = new System.Windows.Controls.Image();
            imageCaptcha.Source = bitmapImage;
        }
    }
}
