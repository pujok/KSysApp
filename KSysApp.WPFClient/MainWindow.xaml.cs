using KSysApp.WPFClient.SignalRChatService;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Net.Http;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace KSysApp.WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChatService chatService;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void LoginClick(object sender, RoutedEventArgs e)
        {
            LoginCommand();
        }

        public void LoginCommand()
        {
            bool error = false;
            if (string.IsNullOrEmpty(NameBox.Text))
            {
                ErrorBlock.Text = "You forgot to enter a username.";
                error = true;
            }
            else if (string.IsNullOrEmpty(PwdBox.Password))
            {
                ErrorBlock.Text = "You forgot to enter a password.";
                error = true;
            }
            else if (string.IsNullOrEmpty(UrlBox.Text))
            {
                ErrorBlock.Text = "You forgot to enter a URL.";
                error = true;
            }
            else
            {                
                SHA256 sha256hash = SHA256.Create();
                byte[] data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(PwdBox.Password));
                var sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                string passwordHash = sBuilder.ToString();                
                try
                {
                    HubConnection connection = new HubConnectionBuilder()
                    .WithUrl(UrlBox.Text, (opts) =>
                        {
                            opts.HttpMessageHandlerFactory = (message) =>
                                {
                                    if (message is HttpClientHandler clientHandler)
                                        clientHandler.ServerCertificateCustomValidationCallback +=
                                            (sender, certificate, chain, sslPolicyErrors) => { return true; };
                                    return message;
                                };
                        }).Build();
                    
                    chatService = new ChatService(connection);
                    chatService.Connect(NameBox.Text, passwordHash);

                    chatService.LoginReceived += ChatService_LoginReceived;
                }
                catch (System.Net.Http.HttpRequestException)
                {
                    ErrorBlock.Text = "Invalid URL.";
                    error = true;
                }
            }
            if (error) { ErrorPopup.IsOpen = true; }
        }

        public void PopupClick(object sender, RoutedEventArgs e)
        {
            ErrorPopup.IsOpen = false;
        }

        private void KeyEventHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                LoginCommand();
            }
        }

        private void ChatService_LoginReceived(bool valid)
        {
            if (valid)
            {
                ChatWindow chatWindow = new ChatWindow(chatService, NameBox.Text);
                chatWindow.Show();
                this.Close();
            }
            else
            {
                ErrorBlock.Text = "Invalid login credentials.";
                ErrorPopup.IsOpen = true;
            }
        }
    }
}