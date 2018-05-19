using STVRogue.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FsCheck;
using FsCheck.Xunit;

namespace STVRogue.GameLogic
{
	public class XTest_Dungeon
	{
			[Property]
			public Property checkIfValidDungeonAutomated(uint level, uint nodeCapacityMultiplier)
			{
				if (level > 0 && nodeCapacityMultiplier > 0)
				{
					Dungeon dungeon = new Dungeon(level, nodeCapacityMultiplier);
					Predicates p = new Predicates();
					//	Assert.True(p.isValidDungeon(dungeon.startNode, dungeon.exitNode, dungeon.difficultyLevel));
					return p.isValidDungeon(dungeon.startNode, dungeon.exitNode, dungeon.difficultyLevel).ToProperty();
				}
				else return true.ToProperty();
			}
		[Property]
		public Property checkifLevelFunctionWorks(uint level, uint nodeCapacityMultiplier)
		{
			if (level > 0 && nodeCapacityMultiplier > 0)
			{
				Dungeon dungeon = new Dungeon(level, nodeCapacityMultiplier);
				return (dungeon.Level(dungeon.exitNode) == level).ToProperty();
			}
			else return true.ToProperty();
		}
		/*
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
		*/
       
	}
}
