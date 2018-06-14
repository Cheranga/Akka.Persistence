namespace Console.Messages
{
    public class CreatePlayerMessage
    {
        public CreatePlayerMessage(string playerName)
        {
            PlayerName = playerName;
        }

        public string PlayerName { get; }
    }
}