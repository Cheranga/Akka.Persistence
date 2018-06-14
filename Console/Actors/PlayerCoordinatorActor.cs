using Akka.Actor;
using Akka.Persistence;
using Console.Commands;
using Console.Events;

namespace Console.Actors
{
    public class PlayerCoordinatorActor : ReceivePersistentActor //ReceiveActor
    {
        private const int DefaultHealth = 100;

        public PlayerCoordinatorActor()
        {
            Command<CreatePlayerCommand>(command => CreatePlayerMessageHandler(command));
            Recover<PlayerCreatedEvent>(@event =>
            {
                DisplayHelper.ShowInfo("Recovering player coordinator");
                Context.ActorOf(Props.Create(() => new PlayerActor(@event.PlayerName, DefaultHealth)), @event.PlayerName);
            });
        }

        public override string PersistenceId => "PlayerCoordinator";

        private void CreatePlayerMessageHandler(CreatePlayerCommand command)
        {
            DisplayHelper.ShowInfo("Received message to create", command.PlayerName);

            Persist(new PlayerCreatedEvent(command.PlayerName, DefaultHealth), @event =>
            {
                DisplayHelper.ShowInfo("Saving create player command for", @event.PlayerName);
                Context.ActorOf(Props.Create(() => new PlayerActor(@event.PlayerName, @event.Health)), @event.PlayerName);
            });
        }
    }
}