using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Console.Actors;
using Console.Messages;

namespace Console
{
    class Program
    {
        private static ActorSystem _actorSystem;
        private static IActorRef _playerCoordinatorRef;

        static void Main(string[] args)
        {
            _actorSystem = ActorSystem.Create("GamePlan");
            _playerCoordinatorRef = _actorSystem.ActorOf(Props.Create(() => new PlayerCoordinatorActor()), "PlayerCoordinator");


            while (true)
            {
                var action = System.Console.ReadLine();

                var playerName = action.Split(' ')[1];

                if (action.Contains("create"))
                {
                    CreatePlayerCommand(playerName);
                }
                else if (action.Contains("hit"))
                {
                    var damage = int.Parse(action.Split(' ')[2]);

                    HitPlayerCommand(playerName, damage);
                }
                else if (action.Contains("display"))
                {
                    DisplayStatusCommand(playerName);
                }
                else if (action.Contains("error"))
                {
                    CauseErrorCommand(playerName);
                }
            }
        }

        private static void CreatePlayerCommand(string playerName)
        {
            _playerCoordinatorRef.Tell(new CreatePlayerMessage(playerName));
        }

        private static void HitPlayerCommand(string playerName, int damage)
        {
            _actorSystem.ActorSelection($"/user/PlayerCoordinator/{playerName}").Tell(new HitMessage(damage));
        }

        private static void DisplayStatusCommand(string playerName)
        {
            _actorSystem.ActorSelection($"/user/PlayerCoordinator/{playerName}").Tell(new DisplayStatusMessage());
        }

        private static void CauseErrorCommand(string playerName)
        {
            _actorSystem.ActorSelection($"/user/PlayerCoordinator/{playerName}").Tell(new CauseErrorMessage());
        }

    }
}
