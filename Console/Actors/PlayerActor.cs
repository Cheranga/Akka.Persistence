using System;
using Akka.Persistence;
using Console.Commands;
using Console.Events;
using Console.Snapshots;

namespace Console.Actors
{
    public class PlayerActor : ReceivePersistentActor //ReceiveActor
    {
        private PlayerSnapShot _state;
        private int _eventCount = 0;

        public PlayerActor(string playerName, int health)
        {
            _state = new PlayerSnapShot(playerName, health);
            //
            // The commands will handle the message and is used to persist the message/event
            //
            Command<HitPlayerCommand>(command => HitPlayerCommandHandler(command));
            Command<CauseErrorCommand>(command => CauseErrorCommandHandler());
            Command<DisplayStatusCommand>(command => DisplayStatusCommandHandler());
            //
            // The "recover" methods will be called when the actor is restarted
            //
            Recover<PlayerGotHitEvent>(@event =>
            {
                DisplayHelper.ShowInfo("Recovering", @event.PlayerName);
                _state.Health -= @event.Damage;
            });

            Recover<SnapshotOffer>(offer =>
            {
                _state = offer.Snapshot as PlayerSnapShot;
            });
        }

        public string PlayerName => _state.PlayerName;
        public int Health => _state.Health;

        public override string PersistenceId => $"Player_{PlayerName}";

        private void DisplayStatusCommandHandler()
        {
            DisplayHelper.ShowInfo(PlayerName, "show health message received");
            DisplayHelper.ShowInfo(PlayerName, _state.Health);
        }

        private void CauseErrorCommandHandler()
        {
            DisplayHelper.ShowWarning(PlayerName, "received a message to cause error");
            throw new ApplicationException("CauseErrorMessage received...");
        }

        private void HitPlayerCommandHandler(HitPlayerCommand command)
        {
            DisplayHelper.ShowInfo(PlayerName, "got hit");

            Persist(new PlayerGotHitEvent(PlayerName, command.Damage), @event =>
            {
                DisplayHelper.ShowInfo("Saving", @event.PlayerName);
                _state.Health -= @event.Damage;
                //
                // Save snapshot after every 5 events
                //
                _eventCount++;

                if (_eventCount == 5)
                {
                    DisplayHelper.ShowInfo("Saving snapshot for", @event.PlayerName);
                    SaveSnapshot(_state);
                    _eventCount = 0;
                }
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
    }
}