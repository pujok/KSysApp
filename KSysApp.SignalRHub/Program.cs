using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Data.SqlClient;

namespace SignalRHub
{
    public class Program
    {
        private static SqlConnection con;
        public static void Main(string[] args)
        {
            var cs = @"Server=localhost\SQLEXPRESS;Database=signalrhubdb;Trusted_Connection=True;";
            var stm = "SELECT @@VERSION";

            con = new SqlConnection(cs);
            con.Open();

            using var cmd = new SqlCommand(stm, con);
            string version = cmd.ExecuteScalar().ToString();

            Console.WriteLine(version);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        public static SqlConnection GetConnection()
        {
            return con;
        }
    }
}
