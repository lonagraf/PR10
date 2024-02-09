using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySql.Data.MySqlClient;

namespace PR10;

public partial class AuthWindow : Window
{
    private Database _db = new Database();
    private ObservableCollection<Role> _roles = new ObservableCollection<Role>();
    private int attemptsCount = 0;
    private bool captchaNeed = false;
    public AuthWindow()
    {
        InitializeComponent();
        CaptchaTBlock.IsVisible = false;
        CaptchaTBox.IsVisible = false;
    }

    private void AuthBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            string login = LoginTBox.Text;
            string password = PasswordTBox.Text;
        
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            string sql = "select * from user where login = @login and password = @password";
            MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@password", password);
            adapter.SelectCommand = command;  
            adapter.Fill(table);
            if (table.Rows.Count > 0)
            {
                string username = table.Rows[0]["full_name"].ToString();
                int role = GetRole(username);
                MainWindow mainWindow = new MainWindow(username, role);
                this.Hide();
                mainWindow.Show();
            }
            else
            {
                attemptsCount++;
                if (attemptsCount > 0)
                {
                    captchaNeed = true;
                    var box = MessageBoxManager.GetMessageBoxStandard("Ошибка","Неверный логин или пароль.", ButtonEnum.Ok);
                    var result = box.ShowAsync();
                }

                if (captchaNeed)
                {
                    CaptchaTBlock.Text = CreateCaptcha();
                    CaptchaTBlock.IsVisible = true;
                    CaptchaTBox.IsVisible = true;
                }
                else
                {
                    CaptchaTBlock.IsVisible = false;
                    CaptchaTBox.IsVisible = false;
                }
            }
        }
        catch (Exception ex)
        {
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Ошибка " + ex, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            var result = error.ShowAsync();
        }
    }

    private int GetRole(string name)
    {
        _db.OpenConnection();
        string sql = "select role from user where full_name = @name";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        command.Parameters.AddWithValue("@name", name);
        int userRole = Convert.ToInt32(command.ExecuteScalar());
        return userRole;
    }

    private string CreateCaptcha()
    {
        string allowChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        char[] chars = allowChar.ToCharArray();
        string captcha = "";

        Random random = new Random();
        for (int i = 0; i < 4; i++)
        {
            captcha += chars[random.Next(chars.Length)];
        }

        return captcha;
    }


    private void GuestTBlock_OnTapped(object? sender, TappedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow(null, 0);
        mainWindow.Show();
    }
}