using KSysApp.WPFClient.SignalRChatService;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net.Http;
using System.Windows;

namespace KSysApp.WPFClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Console.WriteLine("nic");
        }
    }
}
