using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Threading.Tasks;

namespace SignalRHub.Hubs
{
    public class ChatHub : Hub
    {   
        public async Task SendMessage(string user, string message)
        {
            DateTime dateTime = DateTime.Now;
            string sqlDateTime = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            await Clients.All.SendAsync("ReceiveMessage", user, message, sqlDateTime);
            
            using var cmd = new SqlCommand($"INSERT INTO messagelog(Timestamp, Username, Message) VALUES('{sqlDateTime}', '{user}', '{message}');", Program.GetConnection());
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task RequestHistory()
        {
            using var cmd = new SqlCommand("SELECT Timestamp, Username, Message FROM messagelog;", Program.GetConnection());
            using var reader = await cmd.ExecuteReaderAsync();

            List<string[]> history = new List<string[]>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string tempDateTime = reader.GetDateTime(0).ToString("yyyy-MM-dd HH:mm:ss");
                    string tempUser = reader.GetString(1);
                    string tempMsg = reader.GetString(2);
                    string[] tempStrArray = new string[3] { tempDateTime, tempUser, tempMsg };
                    history.Add(tempStrArray);
                }
            }
            await reader.CloseAsync();
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveHistory", history);
        }

        public async Task AttemptLogin(string username, string passwordHash)
        {
            using var cmd = new SqlCommand($"SELECT Username, PwdHash FROM usercredentials WHERE Username='{username}';", Program.GetConnection());
            using var reader = await cmd.ExecuteReaderAsync();
            bool valid = false;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    SqlBinary tempHash = reader.GetSqlBinary(1);
                    SqlBinary passwordHashSqlBinary = new SqlBinary(Encoding.UTF8.GetBytes(passwordHash));
                    if (Equals(tempHash, passwordHashSqlBinary)) { valid = true; }
                }
            }
            //Uncomment this to register new account
            //using var cmd = new SqlCommand($"INSERT INTO usercredentials(Username, PwdHash) VALUES('{username}',@hashbinary);", Program.GetConnection());
            //cmd.Parameters.AddWithValue("hashbinary", Encoding.UTF8.GetBytes(passwordHash));
            //await cmd.ExecuteNonQueryAsync();
            //bool valid = true;
            Clients.Client(Context.ConnectionId).SendAsync("ReceiveLogin", valid);
        }
    }
}