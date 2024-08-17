using System.Net.Sockets;
using System.Text;

namespace OsuIRC
{
    public class IRCConnector
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _stream;
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;
        private readonly IRCMessageHandler _messageHandler;
        private readonly IRCCommands _commands;
        private bool _isConnected;

        public IRCConnector(string server, int port, string username, string password)
        {
            _client = new TcpClient();
            _client.Connect(server, port);
            Console.WriteLine($"Connected to Endpoint: {server}:{port}" );

            _stream = _client.GetStream();
            Console.WriteLine($"Got Stream");

            _reader = new StreamReader(_stream, Encoding.UTF8);
            Console.WriteLine($"Reader Created");
            _ = Task.Run(ReadAsync);

            _writer = new StreamWriter(_stream, Encoding.UTF8) { AutoFlush = true };
            Console.WriteLine($"Writer Created");

            _commands = new IRCCommands(this, username, password);
            _messageHandler = new IRCMessageHandler(this);
        }

        public IRCCommands GetIRCCommands() => _commands;

        public bool IsUserConnected() => _isConnected;

        public void UserConnected(bool connected) => _isConnected = connected;

        public async Task WriteLineAsync(string message)
        {
            if(_stream.CanWrite)
                await _writer.WriteLineAsync(message);
                
        }

        private async Task ReadAsync()
        {
            try
            {
                if (_stream.CanRead)
                {
                    await Console.Out.WriteLineAsync("Start Reading...");
                    string? line;
                    while (!string.IsNullOrEmpty(line = await _reader.ReadLineAsync()))
                    {
                        _ = Task.Run(() => _messageHandler.HandleMessages(line));
                        await Console.Out.WriteLineAsync($"Incoming Message: {line}");
                    }
                }
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.ToString());
            }
            
        }
    }
}
