using System;
using Akka.Actor;
using Akka.Persistence;
using Console.Messages;

namespace Console.Actors
{
    public class PlayerActor : ReceivePersistentActor//ReceiveActor
    {
        private int _health;

        public PlayerActor(string playerName, int health)
        {
            PlayerName = playerName;
            _health = health;

            //
            // The commands will handle the message and is used to persist the message/event
            //
            Command<HitMessage>(message => HitPlayerMessageHandler(message));
            Command<CauseErrorMessage>(message => CauseErrorMessageHandler());
            Command<DisplayStatusMessage>(message => DisplayStatusMessageHandler());
            //
            // The "recover" methods will be called when the actor is restarted
            //
            Recover<HitMessage>(message =>
            {
                DisplayHelper.ShowInfo("Recovering", PlayerName);
                _health -= message.Damage;
            });

        }

        public string PlayerName { get; }

        private void DisplayStatusMessageHandler()
        {
            DisplayHelper.ShowInfo(PlayerName, "show health message received");
            DisplayHelper.ShowInfo(PlayerName, _health);
        }

        private void CauseErrorMessageHandler()
        {
            DisplayHelper.ShowWarning(PlayerName, "received a message to cause error");
            throw new ApplicationException("CauseErrorMessage received...");
        }

        private void HitPlayerMessageHandler(HitMessage message)
        {
            DisplayHelper.ShowInfo(PlayerName, "got hit");

            Persist(message, hitMessage =>
            {
                DisplayHelper.ShowInfo("Saving", PlayerName);
                _health -= hitMessage.Damage;
            });
        }

        protected override void PreStart()
        {
            DisplayHelper.ShowInfo(PlayerName, "started...");
            base.PreStart();
        }

        protected override void PostStop()
        {
            DisplayHelper.ShowWarning(PlayerName, "is stopped...");
            base.PostStop();
        }

        public override string PersistenceId => $"Player_{PlayerName}";
    }
}