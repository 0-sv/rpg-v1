using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STVRogue.Utils;

namespace STVRogue.GameLogic
{
    public class Item
    {
        public String id;
        public Boolean used = false;
		public Node location;
        public bool IsHealingPotion => false;
        public bool IsCrystal => false;
        public Item() { }
        public Item(String id) { this.id = id; }

        virtual public void Use(Player player)
        {
            try {
                Logger.log("" + player.GetID() + " is trying to use an expired item: "
                              + this.GetType().Name + " " + id
                              + ". Rejected.");
                throw new Exception();
            }
            catch (Exception e) {
                Logger.log("" + player.GetID() + " uses " + this.GetType().Name + " " + id);
                Logger.log(e.ToString());
                used = true;
            }
        }
    }

    public class HealingPotion : Item
    {
        public int HPvalue;
        new public bool IsHealingPotion => true;

        /* Create a healing potion with random HP-value */
        public HealingPotion(String id)
            : base(id) {
            HPvalue = 3;
        }

        override public void Use(Player player)
        {
            base.Use(player);
            player.SetHP(Math.Min(player.HPbase, player.GetHP() + HPvalue));
        }
    }

    public class Crystal : Item
    {
        new public bool IsCrystal => true;
        public Crystal(String id) : base(id) { }
        override public void Use(Player player)
        {
            base.Use(player);
            player.SetAccelerated(true);
            if (player.GetLocation() is Bridge) 
                player.dungeon.Disconnect(player.GetLocation() as Bridge);
        }
    }
}
