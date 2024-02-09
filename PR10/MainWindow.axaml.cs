using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PR10;

public partial class MainWindow : Window
{
    private User _user = new User();
    private int userRole;
    private string username;
    public MainWindow(string username, int role)
    {
        InitializeComponent();
        Block.Text += $" {username}";
        userRole = role;
        this.username = username;
        BackBtn.IsVisible = false;
    }

    private void ProductsBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        switch (userRole)
        {
            case 1:
                MainPanel.Children.Clear();
                ProductsPage productsPageAdmin = new ProductsPage(1);
                MainPanel.Children.Add(productsPageAdmin);
                BackBtn.IsVisible = true;
                break;
            case 2:
                MainPanel.Children.Clear();
                ProductsPage productsPageManager = new ProductsPage(2);
                MainPanel.Children.Add(productsPageManager);
                BackBtn.IsVisible = true;
                break;
            case 3:
                MainPanel.Children.Clear();
                ProductsPage productsPageUser = new ProductsPage(3);
                MainPanel.Children.Add(productsPageUser);
                BackBtn.IsVisible = true;
                break;
            default:
                MainPanel.Children.Clear();
                ProductsPage productsPageGuest = new ProductsPage(0);
                MainPanel.Children.Add(productsPageGuest);
                BackBtn.IsVisible = true;
                break;
        }
        
    }

    private void ExitBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        AuthWindow authWindow = new AuthWindow();
        this.Hide();
        authWindow.Show();
    }

    private void BackBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow(username, userRole);
        this.Hide();
        mainWindow.Show();
        
    }
}