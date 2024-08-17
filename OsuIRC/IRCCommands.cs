namespace OsuIRC
{
    public class IRCCommands
    {
        private readonly IRCConnector _connector;
        private readonly string _username;
        private readonly string _password;

        public IRCCommands(IRCConnector connector, string username, string password)
        {
            _connector = connector;
            _username = username;
            _password = password;
        }

        public async Task Login()
        {
            await Console.Out.WriteLineAsync($"{_username} added to Connected Dic");
            await _connector.WriteLineAsync("CAP LS 302");
            await _connector.WriteLineAsync($"PASS {_password}");
            await _connector.WriteLineAsync($"NICK {_username}");
            await _connector.WriteLineAsync($"USER {_username} 0 * :...");
            await Console.Out.WriteLineAsync("Send Login Message Done");
        }

        public async Task Userhost()
        {
            await _connector.WriteLineAsync($"USERHOST {_username}");
            await Console.Out.WriteLineAsync("Send Userhost Message Done");          
        }

        public async Task Mode()
        {
            await _connector.WriteLineAsync($"MODE {_username} +i");
            await Console.Out.WriteLineAsync("Send Mode Message Done");
        }

        public async Task SendMessageToUser(string username, string message)
        {
            if(_connector.IsUserConnected())
            {
                await _connector.WriteLineAsync($"PRIVMSG {username} :{message}");
                await Console.Out.WriteLineAsync("Send Message To User Done");
            }
        }

        public async Task Quit()
        {
            await _connector.WriteLineAsync("QUIT :quit");
            _connector.UserConnected(false);
            await Console.Out.WriteLineAsync("Send Quit Message Done");
        }
    }
}
