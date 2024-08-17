using Microsoft.Extensions.Configuration;
using OsuIRC;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace MikuWeb
{
    internal class Program
    {
        private static IConfiguration? _configuration;

        static async Task Main(string[] args)
        {
            Init();
            var server = _configuration.GetSection("IRC")["Server"];
            var port = int.Parse(_configuration.GetSection("IRC")["Port"]);
            var username = _configuration.GetSection("IRC")["Username"];
            var password = _configuration.GetSection("IRC")["Password"];

            var connector = new IRCConnector(server, port, username, password);
            var commands = connector.GetIRCCommands();

            await commands.Login();
            await Task.Delay(TimeSpan.FromSeconds(5));
            await commands.SendMessageToUser("HatsuneMiku01", $"Miku Miku Miiiiiii");
            await Task.Delay(TimeSpan.FromSeconds(10));
            await commands.Quit();

            Task.Delay(-1).Wait();
        }

        private static void Init()
        {
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        }
    }
}
