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
		public List<Pack> packs;
		public List<Item> items;
		public Dungeon dungeon;
		public uint[] amountOfMonstersPerLevel;
		private Predicates p;
		Random randomnum = new Random();
		/* This creates a player and a random dungeon of the given difficulty level and node-capacity
         * The player is positioned at the dungeon's starting-node.
         * The constructor also randomly seeds monster-packs and items into the dungeon. The total
         * number of monsters are as specified. Monster-packs should be seeded as such that
         * the nodes' capacity are not violated. Furthermore the seeding of the monsters
         * and items should meet the balance requirements stated in the Project Document.
         */
		public Game(uint difficultyLevel, uint nodeCapacityMultiplier, uint numberOfMonsters)
		{
			Logger.log("Creating a game of difficulty level " + difficultyLevel + ", node capacity multiplier "
					   + nodeCapacityMultiplier + ", and " + numberOfMonsters + " monsters.");
			dungeon = new Dungeon(difficultyLevel, nodeCapacityMultiplier);
			amountOfMonstersPerLevel = new uint[difficultyLevel+1];
			packs = addpacks(difficultyLevel, nodeCapacityMultiplier,numberOfMonsters);
			player = new Player("1");
			int totalMonsterHP = calculateTotalMonsterHP();
			
			items = additems(totalMonsterHP, dungeon.bridges[dungeon.bridges.Length-1], player.HPbase);
			p = new Predicates();
		}
		public List<Pack> addpacks(uint difficultyLevel, uint nodeCapcityMultiplier, uint numberOfMonsters)
		{
			uint maxMonstersOnThisLevel, monstersOnThisLevel = 0, monstersInDungeon = 0;
			int pack_id = -1, count = 0, min, numbers;
			List<int> nodesOnThisLevelInRandomOrder;
			
			int amountOfMonsters = 0;
			List<Pack> packs = new List<Pack>();
			for(uint i = 0; i<dungeon.bridges.Length-1;i++)
			{
				min = dungeon.bridges[i] + 1;
				numbers = dungeon.bridges[i + 1] - min + 1;
				nodesOnThisLevelInRandomOrder = Enumerable.Range(min, numbers).OrderBy(x => randomnum.Next()).ToList();
				if(i < dungeon.bridges.Length - 2)
				{
					maxMonstersOnThisLevel = (2 * (i + 1) * numberOfMonsters) / ((difficultyLevel + 2) * (difficultyLevel + 1));
				}
				else
				{
					maxMonstersOnThisLevel = numberOfMonsters - monstersInDungeon;
				}
				count = 0;
				monstersOnThisLevel = 0;
					while (monstersOnThisLevel < maxMonstersOnThisLevel)
					{

					amountOfMonsters = (int)Math.Min(maxMonstersOnThisLevel - monstersOnThisLevel, (nodeCapcityMultiplier * (i+1)));
									
					monstersOnThisLevel += (uint)amountOfMonsters;

					Pack pack = new Pack(pack_id++.ToString(), (uint)amountOfMonsters);
					pack.location = dungeon.nodeList[nodesOnThisLevelInRandomOrder[count]];
					dungeon.nodeList[nodesOnThisLevelInRandomOrder[count++]].packs.Add(pack);
					packs.Add(pack);
					if(count > nodesOnThisLevelInRandomOrder.Count-1)
					{
						throw new GameCreationException("Amount of monsters and nodeCapacityMultiplier are not compatible");
					}
					}
				amountOfMonstersPerLevel[i] = monstersOnThisLevel;

				monstersInDungeon += monstersOnThisLevel;
			}
			return packs;
		}

		public int calculateTotalMonsterHP()
		{
			int totalHP = 0;
			foreach (Pack p in packs)
			{
				foreach(Monster m in p.members)
				{
					totalHP += m.GetHP();
				}
			}
			return totalHP;
		}

		public List<Item> additems(int totalMonsterHP, int nodeMax, int playerHP)
		{
			List<Item> items = new List<Item>();
			int HPlimit = (int)(totalMonsterHP * 0.8);
			int itemAndPlayerHP = playerHP;
			int item_id = -1;
			int count = 0;
			List<int> allNodesInRandomOrder = Enumerable.Range(1, nodeMax-1).OrderBy(x => randomnum.Next()).ToList();
			while ((itemAndPlayerHP+11) < HPlimit && count < allNodesInRandomOrder.Count )
			{
				HealingPotion item = new HealingPotion(item_id++.ToString());
				item.location = dungeon.nodeList[allNodesInRandomOrder[count]];
				dungeon.nodeList[allNodesInRandomOrder[count++]].items.Add(item);
				itemAndPlayerHP += item.HPvalue;
				items.Add(item);
			}
			while(count< allNodesInRandomOrder.Count-1)
			{
				if(randomnum.Next(1,31) == 5)
				{
					Crystal item = new Crystal(item_id++.ToString());
					item.location = item.location = dungeon.nodeList[allNodesInRandomOrder[count]];
					items.Add(item);
				}
				count++;
			}
			return items;
		}

        public void Update()
        {
            if (player.location.packs.Any())
                player.location.Combat(player);
            else
                new Command(player).ExecuteCommand();
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
