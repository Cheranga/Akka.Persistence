namespace Console.Snapshots
{
    public class PlayerSnapShot
    {
        public PlayerSnapShot(string playerName, int health)
        {
            PlayerName = playerName;
            Health = health;
        }

        public string PlayerName { get; set; }
        public int Health { get; set; }
    }
}