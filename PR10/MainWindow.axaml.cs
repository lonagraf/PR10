using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PR10;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ProductsBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.Children.Clear();
        ProductsPage productsPage = new ProductsPage();
        MainPanel.Children.Add(productsPage);
    }
}