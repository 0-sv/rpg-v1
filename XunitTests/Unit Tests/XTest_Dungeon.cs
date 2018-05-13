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
					Assert.True(dungeon.nodeList[i].neighbors.Count == 2);
				}

			}
			
		}
	}
}
