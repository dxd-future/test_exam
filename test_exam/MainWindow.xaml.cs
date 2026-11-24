using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace test_exam
{
    public partial class MainWindow : Window
    {
        catalogEntities db = new catalogEntities();

        public MainWindow()
        {
            InitializeComponent();
            LoadProducts();
            SetupUI();
        }

        private void SetupUI()
        {
            if (UserData.IsGuest)
            {
                TbUserInfo.Text = "Вы вошли как Гость";
            }
            else
            {
                TbUserInfo.Text = $"Добро пожаловать, {UserData.CurrentUser.surename} {UserData.CurrentUser.name} ({UserData.CurrentUser.role})";

                // Роль: Менеджер
                if (UserData.CurrentUser.role == "Менеджер" || UserData.CurrentUser.role == "Администратор")
                {
                    PanelFilters.Visibility = Visibility.Visible;
                    BtnOrders.Visibility = Visibility.Visible;
                }

                // Роль: Администратор
                if (UserData.CurrentUser.role == "Администратор")
                {
                    BtnAdd.Visibility = Visibility.Visible;
                    BtnEdit.Visibility = Visibility.Visible;
                    BtnDelete.Visibility = Visibility.Visible;
                }
            }

            // Заполняем комбобокс фильтра поставщиками
            var providers = db.tovar.Select(t => t.postav).Distinct().ToList();
            providers.Insert(0, "Все поставщики");
            CbFilter.ItemsSource = providers;
            CbFilter.SelectedIndex = 0;

            CbSort.SelectedIndex = 0;
        }

        private void LoadProducts()
        {
            var products = db.tovar.ToList();


            DgProducts.ItemsSource = products;
        }

        // Поиск
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        // Фильтрация
        private void CbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        // Сортировка
        private void CbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (DgProducts == null || CbFilter == null || CbSort == null) return;

            var query = db.tovar.AsQueryable();

            // Поиск по названию или артикулу
            if (!string.IsNullOrWhiteSpace(TbSearch.Text))
            {
                query = query.Where(t => t.name.Contains(TbSearch.Text) || t.article.ToString().Contains(TbSearch.Text));
            }

            // Фильтр по поставщику
            if (CbFilter.SelectedItem != null && CbFilter.SelectedItem.ToString() != "Все поставщики")
            {
                string provider = CbFilter.SelectedItem.ToString();
                query = query.Where(t => t.postav == provider);
            }

            // Сортировка по количеству на складе
            if (CbSort.SelectedIndex == 1)
            {
                query = query.OrderBy(t => t.count);
            }
            else if (CbSort.SelectedIndex == 2)
            {
                query = query.OrderByDescending(t => t.count);
            }

            DgProducts.ItemsSource = query.ToList();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ProductWindow window = new ProductWindow(null);
            if (window.ShowDialog() == true) LoadProducts();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgProducts.SelectedItem is tovar selectedProduct)
            {
                ProductWindow window = new ProductWindow(selectedProduct);
                if (window.ShowDialog() == true) LoadProducts();
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования!");
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DgProducts.SelectedItem is tovar selectedProduct)
            {
                if (MessageBox.Show("Удалить товар?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        db.tovar.Remove(selectedProduct);
                        db.SaveChanges();
                        LoadProducts();
                    }
                    catch
                    {
                        MessageBox.Show("Нельзя удалить товар, так как он есть в заказах!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления!");
            }
        }

        private void BtnOrders_Click(object sender, RoutedEventArgs e)
        {
            OrdersWindow window = new OrdersWindow();
            window.ShowDialog();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}