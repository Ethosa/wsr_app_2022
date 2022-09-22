using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace app
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int wrongCount = 0;
        private int timeout = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnLoad(object sender, RoutedEventArgs e)
        {
            timeout = Properties.Settings.Default.timeout;
            Console.WriteLine(timeout);
            if (timeout > 0)
                await AttemptsTimer(timeout);
        }

        private async void login_Click(object sender, RoutedEventArgs e)
        {
            var login = username.Text;
            var password = pswd.Password;
            if (login == password && login == "inspector")
            {
                error.Content = "";
            }
            else if (wrongCount < 3)
            {
                error.Content = $"неправильный логин или пароль. Осталось {3 - wrongCount} попытки";
                wrongCount++;
            }
            else
            {
                await AttemptsTimer(60);
            }
        }

        private async Task AttemptsTimer(int seconds)
        {
            error.Visibility = Visibility.Visible;
            timeout = seconds;
            Properties.Settings.Default["timeout"] = timeout;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Upgrade();
            for (int i = 0; i <= seconds; ++i)
            {
                error.Content = $"Вход заблокирован на {timeout} секунд.";
                await Task.Delay(1000);
                timeout--;
                Properties.Settings.Default["timeout"] = timeout;
                Properties.Settings.Default.Save();
                Properties.Settings.Default.Upgrade();
            }
            error.Content = "";
        }
    }
}
