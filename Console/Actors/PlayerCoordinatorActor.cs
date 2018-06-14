using Akka.Actor;
using Akka.Persistence;
using Console.Messages;

namespace Console.Actors
{
    public class PlayerCoordinatorActor : ReceivePersistentActor//ReceiveActor
    {
        private const int DefaultHealth = 100;

        public PlayerCoordinatorActor()
        {
            Command<CreatePlayerMessage>(message => CreatePlayerMessageHandler(message));
            Recover<CreatePlayerMessage>(message =>
            {
                DisplayHelper.ShowInfo("Recovering player coordinator");
                Context.ActorOf(Props.Create(() => new PlayerActor(message.PlayerName, DefaultHealth)), message.PlayerName);
            });
        }

        private void CreatePlayerMessageHandler(CreatePlayerMessage message)
        {
            DisplayHelper.ShowInfo("Received message to create", message.PlayerName);

            Persist(message, playerMessage =>
            {
                DisplayHelper.ShowInfo("Saving create player command for", message.PlayerName);
                Context.ActorOf(Props.Create(() => new PlayerActor(message.PlayerName, DefaultHealth)), message.PlayerName);
            });
        }

        public override string PersistenceId => "PlayerCoordinator";
    }
}