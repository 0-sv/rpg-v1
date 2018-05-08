using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STVRogue.Utils;

namespace STVRogue.GameLogic
{
    public class Game
    {
        public Player player;
        private Dungeon dungeon;
        private Predicates p;

        /* This creates a player and a random dungeon of the given difficulty level and node-capacity
         * The player is positioned at the dungeon's starting-node.
         * The constructor also randomly seeds monster-packs and items into the dungeon. The total
         * number of monsters are as specified. Monster-packs should be seeded as such that
         * the nodes' capacity are not violated. Furthermore the seeding of the monsters
         * and items should meet the balance requirements stated in the Project Document.
         */
        public Game(uint difficultyLevel, uint nodeCapcityMultiplier, uint numberOfMonsters)
        {
            Logger.log("Creating a game of difficulty level " + difficultyLevel + ", node capacity multiplier "
                       + nodeCapcityMultiplier + ", and " + numberOfMonsters + " monsters.");
            dungeon = new Dungeon(difficultyLevel, nodeCapcityMultiplier);
			p = new Predicates();
            if (!p.isValidDungeon(dungeon.startNode, dungeon.exitNode, difficultyLevel)) {
                throw new GameCreationException();
            }
                
            
			
            player = new Player();
        }

        /*
         * A single update turn to the game. 
         */
        public Boolean Update(Command userCommand)
        {
            Logger.log("Player does " + userCommand);
            return true;
        }
    }

    public class GameCreationException : Exception
    {
        public GameCreationException() { }
        public GameCreationException(String explanation) : base(explanation) { 
            explanation = "The dungeon is not a valid dungeon!";    
        }
    }
}
