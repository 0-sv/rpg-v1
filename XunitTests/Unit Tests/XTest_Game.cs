using STVRogue.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace STVRogue.GameLogic
{
	public class XTest_Game
	{

		
		[Fact]
		public void checkIfValidDungeon()
		{
			Dungeon dungeon = new Dungeon(10, 2);
			Predicates p = new Predicates();
			Assert.True(p.isValidDungeon(dungeon.startNode, dungeon.exitNode, dungeon.difficultyLevel));
		}
		[Fact]
		public void checkIfTooManyMonstersThrowsException()
		{
            Assert.Throws<GameCreationException> (() => new Game(5, 2, 300));
		}
		[Fact]
		public void checkIfMonstersSpawn()
		{
			Game game = new Game(10, 5, 50);
			foreach (Pack pack in game.packs)
			{
				Assert.NotEmpty(pack.members);
			}
		}
			
		[Fact]
		public void CheckIfMonsterBalancingHolds()
		{
			uint difficultyLevel = 10;
			uint nodeCapacityMultiplier = 5;
			uint numberOfMonsters = 50;
			uint monstersInDungeon = 0;
			uint maxMonstersOnThisLevel, amountOfMonstersOnThisLevel;
			Game game = new Game(difficultyLevel, nodeCapacityMultiplier, numberOfMonsters);

			for (uint i = 0; i < game.dungeon.bridges.Length-1; i++)
			{
				amountOfMonstersOnThisLevel = game.amountOfMonstersPerLevel[i];
				if (i < game.dungeon.bridges.Length - 2)
				{
					maxMonstersOnThisLevel = (2 * (i + 1) * numberOfMonsters) / ((difficultyLevel + 2) * (difficultyLevel + 1));
					monstersInDungeon += maxMonstersOnThisLevel;
				}
				else
				{
					maxMonstersOnThisLevel = numberOfMonsters - monstersInDungeon;
				}
				Assert.True(amountOfMonstersOnThisLevel <= maxMonstersOnThisLevel);
			}
		}
		
		[Fact]
		public void CheckIfItemBalancingHolds()
		{
			Game game = new Game(10, 5, 50);
			int PlayerAndItemHP = game.player.HPbase;
			int MonsterHP = 0;
			for(int i = 0; i < game.items.Count; i++)
			{
				if(game.items[i] is HealingPotion)
				{
					HealingPotion potion = (HealingPotion)game.items[i];
					PlayerAndItemHP += potion.HPvalue;
				}


			}
			foreach (Pack p in game.packs)
			{
				foreach (Monster m in p.members)
				{
					MonsterHP += m.GetHP();
				}
			}
			MonsterHP = (int) (MonsterHP * 0.8);
			Assert.True(PlayerAndItemHP <= MonsterHP);

		}
		[Fact]

		public void CheckItemBalancingOnSmallDungeon()
		{
			Game game = new Game(2, 5, 1);
			for (int i = 0; i < game.items.Count; i++)
			{
				Assert.False(game.items[i] is HealingPotion);

			}
		}

		[Fact]
        public void XTest_disconnect_nodes()
        {
            Node node1 = new Node("1");
            Node node2 = new Node("2");
            Predicates p = new Predicates();
            node1.Connect(node2);
            Assert.True(p.isReachable(node1, node2));
            node1.Disconnect(node2);
            Assert.False(p.isReachable(node1, node2));
        }

        [Fact]
        public void XTest_shortest_path()
        {
            
            Node node1 = new Node("1");
            Node node2 = new Node("2");
            Node node3 = new Node("3");
            Node node4 = new Node("4");
            Node node5 = new Node("5");
            Node node6 = new Node("6");
            Node node7 = new Node("7");
            node1.Connect(node2);
            node2.Connect(node3);
            node3.Connect(node4);
            node4.Connect(node5);
            node1.Connect(node6);
            node6.Connect(node7);
            node7.Connect(node5);
            Dungeon d = new Dungeon(1, 2);
            d.nodeList = new List<Node>() { node1, node2, node3, node4, node5, node6, node7 };
            Assert.Equal(d.Shortestpath(node1, node5), new List<Node>() { node1, node6, node7, node5 });
        }

        [Fact]
        public void XTest_shortest_path_unreachable()
        {
            Node node1 = new Node("1");
            Node node2 = new Node("2");
            Node node3 = new Node("3");
            Node node4 = new Node("4");
            node1.Connect(node2);
            node3.Connect(node4);
            Dungeon d = new Dungeon(1, 2);
            d.nodeList = new List<Node>() { node1, node2, node3, node4 };
            Assert.Equal(d.Shortestpath(node1, node4), new List<Node>() { node1 });
        }
		[Fact]
		public void emptypath()
		{
			Dungeon d = new Dungeon(1, 2);
			Node node1 = new Node("1");
			d.nodeList = new List<Node>() { node1 };
			List<Node> l = d.Shortestpath(node1, node1);
			Assert.Empty(l);
		}
	}
}
