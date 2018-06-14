namespace Console.Commands
{
    public class CreatePlayerCommand
    {
        public CreatePlayerCommand(string playerName)
        {
            PlayerName = playerName;
        }

        public string PlayerName { get; }
    }
}