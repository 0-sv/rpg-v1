using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STVRogue.Utils;

namespace STVRogue.GameLogic
{
    public class Creature
    {
        protected String id;
        protected String name;
        protected int HP;
        protected int AttackRating = 1;
        protected Node location;
        protected Creature() { }
        virtual public void Attack(Creature foe)
        {
            foe.HP = (int)Math.Max(0, foe.HP - AttackRating);
            String killMsg = foe.HP == 0 ? ", KILLING it" : "";
            Logger.log("Creature " + id + " attacks " + foe.id + killMsg + ".");
        }
    }

    public class Monster : Creature
    {
        public Pack pack;

        /* Create a monster with a random HP */
        public Monster(String id)
        {
            this.id = id; name = "Orc";
            HP = 1 + RandomGenerator.rnd.Next(6);
        }

        public Pack GetPack() => pack;
        public void SetHP(int HP) => this.HP = HP;
        public int GetHP() => HP;
    }

    public class Player : Creature
    {
        public Dungeon dungeon;
        public int HPbase = 10;
        private Boolean accelerated = false;
        private uint KillPoint = 0;
        private List<Item> bag = new List<Item>();

        public Player()
        {
            Console.WriteLine("What is your name?");
            this.id = Console.ReadLine();
            this.AttackRating = 5;
            this.HP = HPbase;
        }

        public Player(string id)
        {
            this.id = id;
            this.AttackRating = 5;
            this.HP = HPbase;
        }

        public bool GetAccelerated() => accelerated;
        public int GetAttackRating() => AttackRating;
        public int GetHPMax() => HPbase;
        public int GetHP() => HP;
        public Node GetLocation() => location;
        public string GetID() => id;
        public List<Item> GetBag() => bag;
        public void SetBag(Item i) => bag.Add(i);
        public void SetHP(int hp) => HP = hp;
        public void SetAccelerated(bool IsAccelerated) => accelerated = IsAccelerated;
        public void SetLocation(Node n) => location = n;

        public void PickUp(Item item) {
            bag.Add(item);
        }

        public void Use(Item item)
        {
            if (!bag.Contains(item) || item.used) throw new ArgumentException();
            item.Use(this);
            bag.Remove(item);
        }

        override public void Attack(Creature foe)
        {
            if (!(foe is Monster)) throw new ArgumentException();
            Monster foe_ = foe as Monster;
            if (!accelerated)
            {
                base.Attack(foe);
                if (foe_.GetHP() == 0)
                {
                    foe_.pack.members.Remove(foe_);
                    KillPoint++;
                }
            }
            else
            {
                foreach (Monster target in foe_.pack.members)
                {
                    base.Attack(target);
                }
                int packCount = foe_.pack.members.Count;
                foe_.pack.members.RemoveAll(target => target.GetHP() <= 0);
                KillPoint += (uint)(packCount - foe_.pack.members.Count);
                accelerated = false;
            }
        }
    }
}
