namespace Console.Commands
{
    public class HitPlayerCommand
    {
        public HitPlayerCommand(int damage)
        {
            Damage = damage;
        }

        public int Damage { get; }
    }
}