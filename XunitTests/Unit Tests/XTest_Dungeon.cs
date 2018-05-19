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

		[Property]
		public Property XTest_shortest_path(uint level, uint nodeCapacityMultiplier)
		{
			if (level > 0 && nodeCapacityMultiplier > 0)
			{
				Dungeon dungeon = new Dungeon(level, nodeCapacityMultiplier);
				return (dungeon.Shortestpath(dungeon.startNode,dungeon.exitNode).Count() == level+2).ToProperty();
			}
			else return true.ToProperty();
		}
		/*
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
		*/
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
