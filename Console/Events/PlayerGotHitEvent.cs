namespace Console.Events
{
    public class PlayerGotHitEvent
    {
        public PlayerGotHitEvent(string playerName, int damage)
        {
            PlayerName = playerName;
            Damage = damage;
        }

        public string PlayerName { get; }
        public int Damage { get; }
    }
}