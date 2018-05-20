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
		private Predicates p = new Predicates();
		public Node startNode;
		public Node exitNode;
		public List<Node> nodeList;
		public int[] bridges;
		public uint difficultyLevel;
		/* a constant multiplier that determines the maximum number of monster-packs per node: */
		public uint M;
		Random randomnum = new Random();
		/* To create a new dungeon with the specified difficult level and capacity multiplier */
		public Dungeon(uint level, uint nodeCapacityMultiplier)
		{
			Logger.log("Creating a dungeon of difficulty level " + level + ", node capacity multiplier " + nodeCapacityMultiplier + ".");
            nodeList = new List<Node>();
			difficultyLevel = level;
			M = nodeCapacityMultiplier;
			nodeList = new List<Node>();
			PopulateNodeList(level);
			startNode = nodeList[0];
			exitNode = nodeList[nodeList.Count - 1];
		}

		private void PopulateNodeList(uint level)
        {
            InitializeBridges(level);
            ConnectNodes(nodeList);
            ConnectBridges(nodeList);
            FinalizeConnectionofNonBridgeNodes(nodeList);
        }

        private void InitializeBridges(uint level)
        {
            bridges = new int[level + 2];
            InitializeNodeList(difficultyLevel, bridges);
            bridges[level + 1] = nodeList.Count() - 1;
        }

        private void FinalizeConnectionofNonBridgeNodes(List<Node> nodeList)
        {
            int amountOfPassedBridges = 0;
            for (int i = 0; i < nodeList.Count - 1; i++)
            {
                if (i == bridges[amountOfPassedBridges + 1])
                {
                    amountOfPassedBridges++;
                }
                for (int j = i + 2; j < bridges[amountOfPassedBridges + 1]; j++)
                { // max 4 neighbors per node
                    if (nodeList[i].neighbors.Count < 4 && nodeList[j].neighbors.Count < 4)
                    {
                        if (randomnum.Next(1, 4) == 1)
                        {
                            nodeList[i].Connect(nodeList[j]);
                        }
                    }
                }
            }
        }

        private void ConnectBridges(List<Node> nodeList)
        {
            for (int i = 0; i < bridges.Length - 1; i++)
            {
                nodeList[bridges[i + 1]].Connect(nodeList[bridges[i]]);
            }
        }

        private static void ConnectNodes(List<Node> nodeList)
        {
            for (int i = 0; i < nodeList.Count - 1; i++)
            {
                nodeList[i].Connect(nodeList[i + 1]);
            }
        }

        private void InitializeNodeList(uint level, int[] bridges)
		{
			Random rnd = new Random();
			int nodesonthislevel;
			int node_id = -1;

			for (int i = 1; i <= level; i++)
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
				bridges[i] = node_id;
			}

			// some more nodes
			if (level > 0)
			{
				nodesonthislevel = rnd.Next(3, 5);
				for (int j = 0; j < nodesonthislevel; j++)
				{
					Node n = new Node(node_id++.ToString());
					nodeList.Add(n);
				}
			}
		}

		/* Return a shortest path between node u and node v */
		public List<Node> Shortestpath(Node u, Node v)
		{
			if (!p.isReachable(u, v))
				return new List<Node>() { u };
			return ShortestpathAlgorithm(u, v);
		}

		private static List<Node> ShortestpathAlgorithm(Node u, Node v)
		{
			List<string> closedSet = new List<string>();
			Queue<Tuple<Node, List<Node>>> paths = new Queue<Tuple<Node, List<Node>>>();
			closedSet.Add(u.id);
			foreach (Node n in u.neighbors)
				if (!closedSet.Contains(n.id))
				{
					paths.Enqueue(new Tuple<Node, List<Node>>(n, new List<Node>() { u }));
					closedSet.Add(n.id);
				}

			while (paths.Count != 0)
			{
				Tuple<Node, List<Node>> next = paths.Dequeue();
				next.Item2.Add(next.Item1);
				if (next.Item1.id == v.id)
					return next.Item2;

				foreach (Node n in next.Item1.neighbors)
					if (!closedSet.Contains(n.id))
					{
						paths.Enqueue(new Tuple<Node, List<Node>>(n, next.Item2));
						closedSet.Add(n.id);
					}
			}
			return new List<Node>();
		}
		
		/* To disconnect a bridge from the rest of the zone the bridge is in. */
		/*
		public void Disconnect(Bridge b)
		{
			Logger.log("Disconnecting the bridge " + b.id + " from its zone.");
				for (int i = 0; i < b.neighbors.Count; i++)
				{
					if (Convert.ToInt32(b.neighbors[i].id) < Convert.ToInt32(b.id))
					{
						b.neighbors[i].Disconnect(b);
					}
				}
			startNode = b;
		}
		*/
		/* To calculate the level of the given node. */
		public uint Level(Node d)
		{
			return p.countNumberOfBridges(startNode, exitNode);
		}
	}

	public class Node
	{
		public String id;
		public List<Node> neighbors = new List<Node>();
		public List<Pack> packs = new List<Pack>();
		public List<Item> items = new List<Item>();

        public Node() { }
		public Node(String id) { this.id = id; }

		public void Connect(Node nd)
		{
			neighbors.Add(nd); 
            nd.neighbors.Add(this);
		}

		public void Disconnect(Node nd)
		{
			neighbors.Remove(nd); 
            nd.neighbors.Remove(this);
		}

		public void Combat(Player player)
        {
            Command c = player.GetNextCommand();
            c.ExecuteCommand();
            SelectMonsterAndAttack(player);
            MonsterTurn(player);
        }

        private void MonsterTurn(Player player)
        {
            foreach (Pack p in packs)
            {
                int fleePossibility = p.CalculateFleePossibility();
                if (fleePossibility <= 0.5)
                    p.members[RandomGenerator.rnd.Next(p.members.Count())].Attack(player);
            }
        }

        private void SelectMonsterAndAttack(Player player)
        {
            if (player.attacking)
            {
                ListPossiblePacks();
                int pack = ReadKey();
                ListPossibleMonsters(pack);
                int monster = ReadKey();
                player.Attack(packs[pack].members[monster]);
            }
        }

        private void ListPossibleMonsters(int pack)
        {
            Console.WriteLine("Choose which pack to attack: ");
            int index = 1;
            foreach (Monster m in packs[pack].members)
                Console.WriteLine(index.ToString() + ": " + m.id + " press key " + index++.ToString());
        }

        private void ListPossiblePacks()
        {
            Console.WriteLine("Choose which pack to attack: ");
            int index = 1;
            foreach (Pack p in packs)
                Console.WriteLine(index.ToString() + ": " + p.id + " press key " + index++.ToString());
        }

        private int ReadKey()
        {
            ConsoleKeyInfo cfi = Console.ReadKey();
            return Int32.Parse(cfi.Key.ToString());
        }
    }

	public class Bridge : Node
	{
		List<Node> fromNodes = new List<Node>();
		public List<Node> toNodes = new List<Node>();
		public Bridge(String id) : base(id) { }

		public void ConnectToNodeOfSameZone(Node nd) {
			base.Connect(nd);
			fromNodes.Add(nd);
		}

		public void ConnectToNodeOfNextZone(Node nd) {
			base.Connect(nd);
			toNodes.Add(nd);
		}
	}
}
