using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using MySql.Data.MySqlClient;

namespace PR10;

public partial class ProductsPage : UserControl
{
    private Database _db = new Database();
    private ObservableCollection<Product> _products = new ObservableCollection<Product>();

    private string _sql =
        "select product_id, article, product_name, unit_name, price, max_discount, producer_name, supplier_name, category_name, current_discount, amount, description, image from product" +
        " join producer p on p.producer_id = product.producer" +
        " join supplier s on product.supplier = s.supplier_id" +
        " join unit u on product.unit = u.unit_id" +
        " join product_category pc on product.category = pc.product_category_id";
    public ProductsPage()
    {
        InitializeComponent();
        ShowTable(_sql);
    }

    public void ShowTable(string sql)
    {
        _db.OpenConnection();
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentProduct = new Product()
            {
                ProductId = reader.GetInt32("product_id"),
                Article = reader.GetString("article"),
                ProductName = reader.GetString("product_name"),
                Unit = reader.GetString("unit_name"),
                Price = reader.GetDecimal("price"),
                MaxDiscount = reader.GetInt32("max_discount"),
                Producer = reader.GetString("producer_name"),
                Supplier = reader.GetString("supplier_name"),
                Category = reader.GetString("category_name"),
                CurrentDiscount = reader.GetInt32("current_discount"),
                Amount = reader.GetInt32("amount"),
                Description = reader.GetString("description"),
                ImagePreview = reader["image"] as byte[]
            };
            _products.Add(currentProduct);
        }
        _db.CloseConnection();
        LBoxProducts.ItemsSource = _products;
    }
    

    private void SearchTBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        ObservableCollection<Product> search =
            new ObservableCollection<Product>(_products.Where(x =>
                x.ProductName.ToLower().Contains(SearchTBox.Text.ToLower())));
        LBoxProducts.ItemsSource = search;
    }

    private void OrderBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        ObservableCollection<Product> orderBy = new ObservableCollection<Product>(_products.OrderBy(x => x.Price));
        LBoxProducts.ItemsSource = orderBy;
    }

    private void OrderDescBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        ObservableCollection<Product> orderByDesc = new ObservableCollection<Product>(_products.OrderByDescending(x => x.Price));
        LBoxProducts.ItemsSource = orderByDesc;
    }
    
}