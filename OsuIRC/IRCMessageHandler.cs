namespace OsuIRC
{
    internal class IRCMessageHandler
    {
        private readonly IRCConnector _connector;
        private readonly IRCCommands _commands;
        public IRCMessageHandler(IRCConnector connector)
        {
            _connector = connector;
            _commands = connector.GetIRCCommands();
        }

        public async Task HandleMessages(string message)
        {
            if (message is not null && message.Contains(":cho.ppy.sh"))
            {
                var split = message.Split(" ");
                var code = split[1];
                switch (code)
                {
                    case "001": // Welcome
                        await _commands.Userhost();
                        break;
                    case "376": // End of MOTD
                        await _commands.Mode();
                        {
                            _connector.UserConnected(true);
                            await Task.Delay(TimeSpan.FromSeconds(5));
                            await Console.Out.WriteLineAsync($"{split[2]} set as Connected");
                            break;
                        }
                }
            }
        }
    }
}
