using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace PR10;

public partial class AddProductPage : UserControl
{
    private Database _db = new Database();
    private Product _product;

    public AddProductPage(Product product)
    {
        InitializeComponent();
        LoadDataCategoryCmb();
        LoadDataUnitCmb();
        LoadDataSupplierCmb();
        LoadDataProducerCmb();
        _product = product;
        
        if (_product != null)
        {
            NameTBox.Text = _product.ProductName;
            CategoryCmb.SelectedItem = _product.Category;
            AmountTBox.Text = _product.Amount.ToString();
            UnitCmb.SelectedItem = _product.Unit;
            SupplierCmb.SelectedItem = _product.Supplier;
            PriceTBox.Text = _product.Price.ToString();
            DescriptionTBox.Text = _product.Description;
            ProducerCmb.SelectedItem = _product.Producer;
            ArticleTBox.Text = _product.Article;
            MaxDiscountTBox.Text = _product.MaxDiscount.ToString();
            CurrentDiscountTBox.Text = _product.CurrentDiscount.ToString();
        }
        
    }

    private void BackBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Panel.Children.Clear();
        ProductsPage productsPage = new ProductsPage();
        Panel.Children.Add(productsPage);
    }

    private void SaveBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        _db.OpenConnection();
        if (_product == null)
        {
            string sql =
                "insert into product (product_name, category, amount, unit, supplier, price, description, producer, article, max_discount, current_discount) " +
                "values (@product, @category, @amount, @unit, @supplier, @price, @desc, @producer, @article, @max, @current)";
            MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
            command.Parameters.AddWithValue("@product", NameTBox.Text);
            int selectedCategoryId = GetSelectedCategoryId(CategoryCmb.SelectedItem.ToString());
            command.Parameters.AddWithValue("@category", selectedCategoryId);
            command.Parameters.AddWithValue("@amount", AmountTBox.Text);
            int selectedUnitId = GetSelectedUnitId(UnitCmb.SelectedItem.ToString());
            command.Parameters.AddWithValue("@unit", selectedUnitId);
            int selectedSupplierId = GetSelectedSupplierId(SupplierCmb.SelectedItem.ToString());
            command.Parameters.AddWithValue("@supplier", selectedSupplierId);
            command.Parameters.AddWithValue("@price", PriceTBox.Text);
            command.Parameters.AddWithValue("@desc", DescriptionTBox.Text);
            int selectedProducerId = GetSelectedProducerId(ProducerCmb.SelectedItem.ToString());
            command.Parameters.AddWithValue("@producer", selectedProducerId);
            command.Parameters.AddWithValue("@article", ArticleTBox.Text);
            command.Parameters.AddWithValue("@max", MaxDiscountTBox.Text);
            command.Parameters.AddWithValue("@current", CurrentDiscountTBox.Text);
            command.ExecuteNonQuery();
            var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Данные успешно добавлены", ButtonEnum.Ok,
                Icon.Success);
            var result = box.ShowAsync();
        }
        else
        {
            string sql =
                "update product set product_name = @product, category = @category, amount = @amount, unit = @unit, supplier = @supplier, " +
                "price = @price, description = @desc, producer = @producer, article = @article, max_discount = @max, current_discount = @current " +
                "where product_id = @id";
            MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
            command.Parameters.AddWithValue("@id", _product.ProductId);
            command.Parameters.AddWithValue("@product", NameTBox.Text);
            int selectedCategoryId = GetSelectedCategoryId(CategoryCmb.SelectedItem.ToString());
            command.Parameters.AddWithValue("@category", selectedCategoryId);
            command.Parameters.AddWithValue("@amount", AmountTBox.Text);
            int selectedUnitId = GetSelectedUnitId(UnitCmb.SelectedItem.ToString());
            command.Parameters.AddWithValue("@unit", selectedUnitId);
            int selectedSupplierId = GetSelectedSupplierId(SupplierCmb.SelectedItem.ToString());
            command.Parameters.AddWithValue("@supplier", selectedSupplierId);
            command.Parameters.AddWithValue("@price", PriceTBox.Text);
            command.Parameters.AddWithValue("@desc", DescriptionTBox.Text);
            int selectedProducerId = GetSelectedProducerId(ProducerCmb.SelectedItem.ToString());
            command.Parameters.AddWithValue("@producer", selectedProducerId);
            command.Parameters.AddWithValue("@article", ArticleTBox.Text);
            command.Parameters.AddWithValue("@max", MaxDiscountTBox.Text);
            command.Parameters.AddWithValue("@current", CurrentDiscountTBox.Text);
            command.ExecuteNonQuery();
            var box = MessageBoxManager.GetMessageBoxStandard("Успешно", "Данные успешно сохранены", ButtonEnum.Ok,
                Icon.Success);
            var result = box.ShowAsync();
        }
        _db.CloseConnection();
    }
    
    public void LoadDataCategoryCmb()
    {
        _db.OpenConnection();
        string sql = "select category_name from product_category;";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            CategoryCmb.Items.Add(reader["category_name"].ToString());
        }
        _db.CloseConnection();
    }
    
    public void LoadDataUnitCmb()
    {
        _db.OpenConnection();
        string sql = "select unit_name from unit;";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            UnitCmb.Items.Add(reader["unit_name"].ToString());
        }
        _db.CloseConnection();
    }
    
    public void LoadDataSupplierCmb()
    {
        _db.OpenConnection();
        string sql = "select supplier_name from supplier;";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            SupplierCmb.Items.Add(reader["supplier_name"].ToString());
        }
        _db.CloseConnection();
    }
    public void LoadDataProducerCmb()
    {
        _db.OpenConnection();
        string sql = "select producer_name from producer;";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            ProducerCmb.Items.Add(reader["producer_name"].ToString());
        }
        _db.CloseConnection();
    }

    public int GetSelectedCategoryId(string selectedCategory)
    {
        _db.OpenConnection();
        string sql = "select product_category_id from product_category where category_name = @selectedCategory;";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        command.Parameters.AddWithValue("@selectedCategory", selectedCategory);
        int selectedId = Convert.ToInt32(command.ExecuteScalar());
        return selectedId;
    }
    
    public int GetSelectedUnitId(string selectedUnit)
    {
        _db.OpenConnection();
        string sql = "select unit_id from unit where unit_name = @selectedUnit;";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        command.Parameters.AddWithValue("@selectedUnit", selectedUnit);
        int selectedId = Convert.ToInt32(command.ExecuteScalar());
        return selectedId;
    }
    public int GetSelectedSupplierId(string selectedSupplier)
    {
        _db.OpenConnection();
        string sql = "select supplier_id from supplier where supplier_name = @selectedSupplier;";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        command.Parameters.AddWithValue("@selectedSupplier", selectedSupplier);
        int selectedId = Convert.ToInt32(command.ExecuteScalar());
        return selectedId;
    }
    public int GetSelectedProducerId(string selectedProducer)
    {
        _db.OpenConnection();
        string sql = "select producer_id from producer where producer_name = @selectedProducer;";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        command.Parameters.AddWithValue("@selectedProducer", selectedProducer);
        int selectedId = Convert.ToInt32(command.ExecuteScalar());
        return selectedId;
    }

}