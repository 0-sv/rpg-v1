using STVRogue.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace STVRogue.GameLogic
{
	public class XTest_Dungeon
	{
		[Fact]
		public void checkifLevelFunctionWorks()
		{
			uint level = 5;
			Dungeon dungeon = new Dungeon(level, 2);
			Assert.Equal(dungeon.Level(dungeon.exitNode), level);
		}
		[Fact]
		public void checkIfDisconnectWorks()
		{
			uint level = 5;
			Dungeon dungeon = new Dungeon(level, 2);
			for(int i = 0; i < dungeon.nodeList.Count; i++)
			{
				if(dungeon.nodeList[i] is Bridge)
				{
					dungeon.Disconnect((Bridge)dungeon.nodeList[i]);
					Assert.True(dungeon.nodeList[i].neighbors.Count < 4);
				}

			}
			
		}

        [Fact]
        public void XTest_pack_move()
        {
            Dungeon dungeon = new Dungeon(5, 2);
            Node node = new Node("testnode");
            node.Connect(dungeon.nodeList[1]);
            dungeon.nodeList.Add(node);
            Pack pack = new Pack("testpack", 2);
            pack.location = dungeon.nodeList[1];
            pack.dungeon = dungeon;
            dungeon.nodeList[1].packs = new List<Pack>() { pack };
            pack.Move(node);
            Assert.Contains(pack, node.packs);
            Assert.DoesNotContain(pack, dungeon.nodeList[1].packs);
        }
	}
}
