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
        public String id;
        public String name;
        public int HP;
        public int AttackRating = 1;
        public Node location;
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
        public int HPbase = 10;
        public Boolean accelerated;
        public uint KillPoint;
        public List<Item> bag;

        public Player(string id) {
            this.id = id;
            this.AttackRating = 5;
            this.HP = HPbase;
            this.KillPoint = 0;
            this.accelerated = false;
            this.bag = new List<Item>();
        }

        public void PickUp(Item item) => bag.Add(item);
   
        public Command GetNextCommand() => new Command(this);

        public void Heal() {
            foreach (HealingPotion p in bag)
                if (!p.IsUsed()) {
                    p.Use(this);
                    bag.Remove(p);
                }
        }

        public void Accelerate() {
            foreach (Crystal c in bag)
                if (!c.IsUsed()) {
                    c.Use(this);
                    bag.Remove(c);
                }
        }

        override public void Attack(Creature foe) {
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
