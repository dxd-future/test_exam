using System.Linq;
using System.Windows;

namespace test_exam
{
    public partial class LoginWindow : Window
    {
        catalogEntities db = new catalogEntities();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = TbLogin.Text.Trim();
            string password = PbPassword.Password.Trim();

            var user = db.users.FirstOrDefault(u => u.Логин == login && u.Пароль == password);

            if (user != null)
            {
                UserData.CurrentUser = user;
                UserData.IsGuest = false;
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            UserData.IsGuest = true;
            UserData.CurrentUser = null;
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}