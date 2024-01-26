using System.Data;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace PR10;

public partial class AuthWindow : Window
{
    private Database _db = new Database();
    public AuthWindow()
    {
        InitializeComponent();
    }

    private void AuthBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        string login = LoginTBox.Text;
        string password = PasswordTBox.Text;

        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        string sql = "select * from user where login = @login and password = @password";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        command.Parameters.Add("@login", MySqlDbType.VarChar).Value = login;
        command.Parameters.Add("@password", MySqlDbType.VarChar).Value = password;
        adapter.SelectCommand = command;
        adapter.Fill(table);
        if (table.Rows.Count > 0)
        {
            MainWindow mainWindow = new MainWindow();
            this.Hide();
            mainWindow.Show();
        }
        else
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Неверный логин или пароль", ButtonEnum.Ok,
                MsBox.Avalonia.Enums.Icon.Error);
            var result = box.ShowAsync();
        }
    }
}