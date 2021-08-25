using System;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KSysApp.WPFClient.SignalRChatService
{
    public class ChatService
    {
        private readonly HubConnection _connection;

        public event Action<string, string, string> MessageReceived;
        public event Action<List<string[]>> HistoryReceived;
        public event Action<bool> LoginReceived;

        public ChatService(HubConnection connection)
        {
            _connection = connection;

            _connection.On<string, string, string>("ReceiveMessage", (user, message, dateTime) => MessageReceived?.Invoke(user, message, dateTime));
            _connection.On<List<string[]>>("ReceiveHistory", (history) => HistoryReceived?.Invoke(history));
            _connection.On<bool>("ReceiveLogin", (valid) => LoginReceived?.Invoke(valid));
        }

        public async Task Connect(string username, string passwordHash)
        {
            await _connection.StartAsync();
            await _connection.SendAsync("AttemptLogin", username, passwordHash);
        }

        public async Task RequestHistory()
        {
            await _connection.SendAsync("RequestHistory");
        }

        public async Task SendMessage(string user, string message)
        {
            await _connection.SendAsync("SendMessage", user, message);
        }
    }
}
