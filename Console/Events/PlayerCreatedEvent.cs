namespace Console.Events
{
    public class PlayerCreatedEvent
    {
        public PlayerCreatedEvent(string playerName, int health)
        {
            PlayerName = playerName;
            Health = health;
        }

        public string PlayerName { get; }
        public int Health { get; }
    }
}