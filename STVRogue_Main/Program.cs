using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STVRogue.GameLogic;

namespace STVRogue
{
    /* A dummy top-level program to run the STVRogue game */
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(5, 2, 10);
            Player player = new Player("1");
            player.SetLocation(game.dungeon.startNode);

            while (true)
            {
                string command = Console.ReadLine();
                Command c = new Command(player);
                game.Update(c);
            }
        }
    }
}
