using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace PR10;

public partial class ProductsPage : UserControl
{
    private Database _db = new Database();
    private ObservableCollection<Product> _products = new ObservableCollection<Product>();
    private ObservableCollection<Producer> _producers = new ObservableCollection<Producer>();

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
        LoadDataFilterCBox();
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

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Panel.Children.Clear();
        AddProductPage addProductPage = new AddProductPage(null); 
        Panel.Children.Add(addProductPage);
    }

    private void LBoxProducts_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        Product selectedProduct = LBoxProducts.SelectedItem as Product;
        if (selectedProduct != null)
        {
            Panel.Children.Clear();
            AddProductPage addProductPage = new AddProductPage(selectedProduct);
            Panel.Children.Add(addProductPage);
        }
    }
    private async void DeleteBtn_OnClick(object? sender, RoutedEventArgs e)
    {
            Product selectedProduct= LBoxProducts.SelectedItem as Product;

            if (selectedProduct != null)
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Предупреждение", "Вы уверены что хотите удалить?", ButtonEnum.YesNo);
                var result = await box.ShowAsync();
                if (result == ButtonResult.Yes)
                {
                    _db.OpenConnection();
                    string sql = "delete from product where product_id = " + selectedProduct.ProductId;
                    MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
                    command.ExecuteNonQuery();
                    _db.CloseConnection();
                    _products.Remove(selectedProduct);
                    var success = MessageBoxManager.GetMessageBoxStandard("Успешно", "Данные успешно удалены!", ButtonEnum.Ok);
                    var result1 = success.ShowAsync();
                }
                else
                {
                    var error = MessageBoxManager.GetMessageBoxStandard("Отмена", "Операция удаления отменена!", ButtonEnum.Ok);
                    var result1 = error.ShowAsync();
                }
            }
            else
            {
                var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите проект для удаления!", ButtonEnum.Ok);
                var result = box.ShowAsync();
            }
    }

    private void LoadDataFilterCBox()
    {
        _db.OpenConnection();
        string sql = "select producer_name from producer";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentProducer = new Producer()
            {
                ProducerName = reader.GetString("producer_name")
            };
            _producers.Add(currentProducer);
        }
        _db.CloseConnection();
        _producers.Insert(0, new Producer{ProducerName = "Все производители"});
        FilterCBox.ItemsSource = _producers;
        
    }

    private void FilterCBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {

        var selectedProducer = FilterCBox.SelectedItem as Producer;
        if (selectedProducer.ProducerName == "Все производители")
        {
            LBoxProducts.ItemsSource = _products;
        }
        else
        {
            var filter = _products.Where(x => x.Producer == selectedProducer.ProducerName).ToList();
            LBoxProducts.ItemsSource = filter;
        }
    }

    private void OrderByBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        if (OrderByCheckBox.IsChecked == true)
        {
            ObservableCollection<Product> sort = new ObservableCollection<Product>(_products.OrderBy(x => x.Price).ToList());
            LBoxProducts.ItemsSource = sort;
        }
        else
        {
            ObservableCollection<Product> sort = new ObservableCollection<Product>(_products.OrderByDescending(x => x.Price).ToList());
            LBoxProducts.ItemsSource = sort;
        }
    }
}