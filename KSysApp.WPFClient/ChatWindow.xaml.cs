using KSysApp.WPFClient.SignalRChatService;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace KSysApp.WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private readonly ChatService _service;
        private readonly string _username;
        public ChatWindow(ChatService service, string username)
        {
            _service = service;
            _username = username;
            InitializeComponent();
            _service.MessageReceived += ChatService_MessageReceived;
            _service.HistoryReceived += ChatService_HistoryReceived;
            _service.RequestHistory();
        }

        private async void SendCommand(object sender, RoutedEventArgs e)
        {
            await _service.SendMessage(_username, MessageBox.Text);
            MessageBox.Clear();
        }

        private async void MessageKeyEventHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                await _service.SendMessage(_username, MessageBox.Text);
                MessageBox.Clear();
            }
        }

        private void ChatService_MessageReceived(string user, string message, string dateTime)
        {
            bool scroll = false;
            if (MessageScrollViewer.VerticalOffset == MessageScrollViewer.ScrollableHeight)
            {   
                scroll = true;
            }
            MessageBlock.Text += $"[{dateTime}] {user}:\n{message}\n";
            if (scroll)
            {
                MessageScrollViewer.ScrollToEnd();
            }
        }

        private void ChatService_HistoryReceived(List<string[]> history)
        {
            foreach (var message in history) {
                MessageBlock.Text += $"[{message[0]}] {message[1]}:\n{message[2]}\n";
            }
            MessageScrollViewer.ScrollToEnd();
        }
    }
}