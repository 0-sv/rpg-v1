using STVRogue.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace STVRogue.GameLogic
{
   public class XTest_Monster
    {
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
