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
		public Node location;
        public bool IsHealingPotion => false;
        public bool IsCrystal => false;
        public Item() { }
        public Item(String id) { this.id = id; }
    }

    public class HealingPotion : Item
    {
        public int HPvalue;
        new public bool IsHealingPotion => true;

        /* Create a healing potion with 3 HP-value */
        public HealingPotion(String id)
            : base(id) {
            HPvalue = 3;
        }
    }

    public class Crystal : Item
    {
        new public bool IsCrystal => true;
        public Crystal(String id) : base(id) { }
    }
}
