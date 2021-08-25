using System;
using Microsoft.AspNetCore.SignalR.Client;
using KSysApp.ConsoleClient.SignalRChatService;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace KSysApp.ConsoleClient
{
    class Program
    {
        private static ChatService chatService;
        private static string user;
        static void Main()
        { 
            Task t = MainAsync();
            t.Wait();
        }

        static async Task MainAsync()
        {
            bool success = false;
            string[] inputs;
            string url;
            string password;
            while (!success) {
                inputs = LoginLoop();
                user = inputs[1];
                url = inputs[0];
                password = inputs[2];
                string passwordHash = GetHash(password);
                try
                {
                    HubConnection connection = new HubConnectionBuilder()
                    .WithUrl(url, (opts) =>
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

                    Task connectTask = chatService.Connect(user, passwordHash);
                    connectTask.Wait();

                    chatService.MessageReceived += ChatService_MessageReceived;
                    chatService.HistoryReceived += ChatService_HistoryReceived;
                    chatService.LoginReceived += ChatService_LoginReceived;
                    success = true;
                }
                catch (System.AggregateException)
                {
                    Console.WriteLine("Invalid URL, try again.");
                }
                
            }
            while (true) ;
        }

        private static void ChatService_MessageReceived(string user, string message, string dateTime)
        {
            Console.WriteLine($"[{dateTime}] {user}: {message}");
        }

        private static void ChatService_HistoryReceived(List<string[]> history)
        {
            foreach (var message in history)
            {
                Console.WriteLine ($"[{message[0]}] {message[1]}: {message[2]}");
            }
        }

        private static async void ChatService_LoginReceived(bool valid)
        {
            if (valid)
            {
                await chatService.RequestHistory();
                
                while (true)
                {
                    string message = await GetInputAsync();
                    DeletePrevConsoleLine();
                    await chatService.SendMessage(user, message);
                }
            }
            else
            {
                Console.WriteLine("Invalid login credentials.");
                string[] inputs = LoginLoop();
                string user = inputs[1];
                string password = inputs[2];
                string passwordHash = GetHash(password);
                Task connectTask = chatService.Connect(user, passwordHash);
                connectTask.Wait();
            }
        }

        private static async Task<string> GetInputAsync()
        {
            return await Task.Run(() => Console.ReadLine());
        }

        private static void DeletePrevConsoleLine()
        {
            if (Console.CursorTop == 0) return;
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }

        private static string[] LoginLoop()
        {
            Console.WriteLine("URL: ");
            string urlinput = Console.ReadLine();
            DeletePrevConsoleLine();
            Console.WriteLine("Username: ");
            string nameinput = Console.ReadLine();
            DeletePrevConsoleLine();
            Console.WriteLine("Password: ");
            string pwinput = Console.ReadLine();
            DeletePrevConsoleLine();
            string[] returnstrings;
            returnstrings = new string[3] { urlinput, nameinput, pwinput };
            return returnstrings;
        }

        private static string GetHash(string password)
        {
            SHA256 sha256hash = SHA256.Create();
            byte[] data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        
    }
}