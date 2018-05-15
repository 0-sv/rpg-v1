using STVRogue.GameLogic;
using System;

namespace STVRogue
{
    public class Command
    {
        private Player player;
        private Node node;
        private ConsoleKeyInfo key;
        public bool attack;

        public Command (Player player) {
            this.key = Console.ReadKey();
            this.player = player;
            this.node = player.location;
        }

        public void ExecuteCommand() {
            switch (key.Key) {
                case ConsoleKey.LeftArrow: 
                    GoLeft();
                    break;
                case ConsoleKey.DownArrow:
                    GoDown();
                    break;
                case ConsoleKey.RightArrow:
                    GoRight();
                    break;
                case ConsoleKey.UpArrow:
                    GoUp();
                    break;
                case ConsoleKey.H:
                    UseHPPotion();
                    break;
                case ConsoleKey.C:
                    UseCrystal();
                    break;
                case ConsoleKey.F:
                    Flee();
                    break;
                case ConsoleKey.A:
                    Attack();
                    break;
                default:
                    break;
            }
        }

        private void GoUp() => throw new NotImplementedException();

        private void GoRight() => throw new NotImplementedException();

        private void GoDown() => throw new NotImplementedException();

        private void GoLeft() => throw new NotImplementedException();

        private void UseHPPotion() => player.Heal();

        private void UseCrystal() => player.Accelerate();

        private void Flee() => player.location = node.neighbors[0];

        public void Attack() => attack = true;
    }
}
