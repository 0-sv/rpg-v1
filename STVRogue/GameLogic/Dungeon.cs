using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STVRogue.Utils;

namespace STVRogue.GameLogic
{
	public class Dungeon
	{
		public Node startNode;
		public Node exitNode;
		public uint difficultyLevel;
		/* a constant multiplier that determines the maximum number of monster-packs per node: */
		public uint M;
		Random randomnum = new Random();
		/* To create a new dungeon with the specified difficult level and capacity multiplier */
		public Dungeon(uint level, uint nodeCapacityMultiplier)
		{
			Logger.log("Creating a dungeon of difficulty level " + level + ", node capacity multiplier " + nodeCapacityMultiplier + ".");
			// array met indices van alle bridges
			int[] bridges = new int[level + 1];
			difficultyLevel = level;
			M = nodeCapacityMultiplier;
			List<Node> nodeList = InitializeNodeList(difficultyLevel, bridges);
			bridges[level] = nodeList.Count() - 1;
			// amount of bridges passed
			int bridgeloc = 0;

			for (int i = 0; i < nodeList.Count - 1; i++)
			{
				if (i == bridges[bridgeloc + 1])
				{
					bridgeloc++;
				}
				for (int j = i+1; j <= bridges[bridgeloc + 1]; j++)
				{
					// connect nodes
					if (randomnum.Next(1, 4) > nodeList[i].neighbors.Count())
					{ 
							nodeList[i].neighbors.Add(nodeList[j]);
							nodeList[j].neighbors.Add(nodeList[i]);
					}
				}
			}
			if(nodeList[nodeList.Count()-1].neighbors.Count() == 0)
			{
				nodeList[nodeList.Count() - 1].neighbors.Add(nodeList[nodeList.Count() - 2]);
				nodeList[nodeList.Count() - 2].neighbors.Add(nodeList[nodeList.Count() - 1]);
			}
			for (int i = 0; i < nodeList.Count(); i++)
			{
				Console.WriteLine("node" + nodeList[i].id);
				for (int j = 0; j < nodeList[i].neighbors.Count(); j++)
				{
					Console.WriteLine("neighbor" + nodeList[i].neighbors[j].id);
				}
			}

		}

		private static List<Node> InitializeNodeList(uint level, int[] bridges)
		{
			List<Node> nodeList = new List<Node>();
			Random rnd = new Random();
			int nodesonthislevel = rnd.Next(2, 6);
			int node_id = 0;
			for (int i = 1; i < level; i++)
			{ // between 2 and 4 nodes on each level excluding the bridges
				nodesonthislevel = rnd.Next(2, 5);
				for (int j = 0; j < nodesonthislevel; j++)
				{
					Node n = new Node(node_id++.ToString());
					nodeList.Add(n);
				}
				// add a bridge after each level
				Bridge b = new Bridge(node_id++.ToString());
				nodeList.Add(b);
				bridges[i] = bridges[i - 1] + nodesonthislevel + 1;

			}
			// some more nodes
			if (level > 0)
			{
				nodesonthislevel = rnd.Next(2, 6);
				for (int j = 0; j < nodesonthislevel; j++)
				{
					Node n = new Node(node_id++.ToString());
					nodeList.Add(n);
				}
			}
			return nodeList;

		}

		/* Return a shortest path between node u and node v */
		public List<Node> shortestpath(Node u, Node v)
		{
			if (u.id == v.id)
				return new List<Node>() { u };
			List<string> closedSet = new List<string>();
			Queue<Tuple<Node, List<Node>>> paths = new Queue<Tuple<Node, List<Node>>>();
			closedSet.Add(u.id);
			foreach (Node n in u.neighbors)
			{
				if (!closedSet.Contains(n.id))
				{
					paths.Enqueue(new Tuple<Node, List<Node>>(n, new List<Node>() { u }));
					closedSet.Add(n.id);
				}
			}
			while (paths.Count != 0)
			{
				Tuple<Node, List<Node>> next = paths.Dequeue();
				next.Item2.Add(next.Item1);
				if (next.Item1.id == v.id)
					return next.Item2;
				foreach (Node n in next.Item1.neighbors)
				{
					if (!closedSet.Contains(n.id))
					{
						paths.Enqueue(new Tuple<Node, List<Node>>(n, next.Item2));
						closedSet.Add(n.id);
					}
				}
			}
			return new List<Node>();
		}


		/* To disconnect a bridge from the rest of the zone the bridge is in. */
		public void disconnect(Bridge b)
		{
			Logger.log("Disconnecting the bridge " + b.id + " from its zone.");
			foreach (Node n in b.toNodes)
			{
				n.disconnect(b);
			}
			startNode = b;
		}

		/* To calculate the level of the given node. */
		public uint level(Node d)
		{
			List<Node> pathToNode_d = shortestpath(startNode, d);
			uint level = 0;

			for (int index = 0; index < pathToNode_d.Count(); index++)
			{
				if (pathToNode_d[index].isBridge)
				{
					level++;
				}
			}
			return level;
		}
	}

	public class Node
	{
		public String id;
		public List<Node> neighbors = new List<Node>();
		public List<Pack> packs = new List<Pack>();
		public List<Item> items = new List<Item>();
		public bool isBridge = false;

		public Node() { }
		public Node(String id) { this.id = id; }

		/* To connect this node to another node. */
		public void connect(Node nd)
		{
			neighbors.Add(nd); nd.neighbors.Add(this);
		}

		/* To disconnect this node from the given node. */
		public void disconnect(Node nd)
		{
			neighbors.Remove(nd); nd.neighbors.Remove(this);
		}

		/* Execute a fight between the player and the packs in this node.
         * Such a fight can take multiple rounds as describe in the Project Document.
         * A fight terminates when either the node has no more monster-pack, or when
         * the player's HP is reduced to 0. 
         */
		public void fight(Player player)
		{
			throw new NotImplementedException();
		}
	}

	public class Bridge : Node
	{
		List<Node> fromNodes = new List<Node>();
		public List<Node> toNodes = new List<Node>();
		public Bridge(String id) : base(id) { }
		new public bool isBridge = true;

		/* Use this to connect the bridge to a node from the same zone. */
		public void connectToNodeOfSameZone(Node nd)
		{
			base.connect(nd);
			fromNodes.Add(nd);
		}

		/* Use this to connect the bridge to a node from the next zone. */
		public void connectToNodeOfNextZone(Node nd)
		{
			base.connect(nd);
			toNodes.Add(nd);
		}
	}
}
