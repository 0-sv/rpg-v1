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
			Game game = new Game(5, 2, 300);
		}
	}
}
