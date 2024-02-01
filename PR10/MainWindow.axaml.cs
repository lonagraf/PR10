using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PR10;

public partial class MainWindow : Window
{
    public MainWindow(string username)
    {
        InitializeComponent();
        Block.Text += $", {username}";
    }

    private void ProductsBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.Children.Clear();
        ProductsPage productsPage = new ProductsPage();
        MainPanel.Children.Add(productsPage);
    }

    private void ExitBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        AuthWindow authWindow = new AuthWindow();
        this.Hide();
        authWindow.Show();
    }
}