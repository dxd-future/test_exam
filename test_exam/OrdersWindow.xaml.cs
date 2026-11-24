using System.Linq;
using System.Windows;

namespace test_exam
{
    public partial class OrdersWindow : Window
    {
        catalogEntities db = new catalogEntities();

        public OrdersWindow()
        {
            InitializeComponent();
            LoadOrders();

            if (UserData.CurrentUser.role != "Администратор")
            {
                BtnAddOrder.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadOrders()
        {
            DgOrders.ItemsSource = db.orders.ToList();
        }

        private void BtnAddOrder_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функционал добавления заказа (требует создания формы OrderWindow)");
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}