using System;
using System.Windows;

namespace test_exam
{
    public partial class ProductWindow : Window
    {
        catalogEntities db = new catalogEntities();
        private tovar _currentProduct;

        public ProductWindow(tovar product)
        {
            InitializeComponent();
            if (product != null)
            {
                _currentProduct = product;
                TbArticle.Text = product.article.ToString();
                TbName.Text = product.name;
                TbPrice.Text = product.price.ToString();
                TbPostav.Text = product.postav;
                TbSale.Text = product.sale.ToString();
                TbCount.Text = product.count.ToString();
                TbInfo.Text = product.info;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbName.Text) || string.IsNullOrWhiteSpace(TbPrice.Text))
            {
                MessageBox.Show("Заполните обязательные поля!");
                return;
            }

            try
            {
                if (_currentProduct == null)
                {
                    // Добавление
                    tovar newProduct = new tovar
                    {
                        article = TbArticle.Text,
                        name = TbName.Text,
                        price = short.Parse(TbPrice.Text),
                        postav = TbPostav.Text,
                        sale = byte.Parse(TbSale.Text),   
                        count = byte.Parse(TbCount.Text),  
                        info = TbInfo.Text,
                        photo = "1.jpg"
                    };
                    db.tovar.Add(newProduct);
                }
                else
                {
                    // Редактирование
                    _currentProduct.article = TbArticle.Text;
                    _currentProduct.name = TbName.Text;
                    _currentProduct.price = short.Parse(TbPrice.Text); 
                    _currentProduct.postav = TbPostav.Text;
                    _currentProduct.sale = byte.Parse(TbSale.Text);    
                    _currentProduct.count = byte.Parse(TbCount.Text);  
                    _currentProduct.info = TbInfo.Text;
                }

                db.SaveChanges();
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка ввода или сохранения: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}