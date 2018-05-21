using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STVRogue.GameLogic;

namespace STVRogue {
    class Program {
        static void Main(string[] args) {
            Game game = new Game(5, 2, 10);
            Console.WriteLine("What is your player name?");
            Player player = new Player(Console.ReadLine());
            player.location = game.dungeon.startNode;

            while (true) {
                
                game.Update();
            }
        }
    }
}
