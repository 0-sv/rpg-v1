using STVRogue.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace STVRogue.GameLogic
{
	public class XTest_Items
	{
        int hp_before_hp_potion;

        Player p = new Player("1");
        Crystal c = new Crystal("2");
        HealingPotion hp_potion = new HealingPotion("3");
        Item i = new Item("4");

        

        [Fact]
        public void IfHealingPotion_IsHealingPotionReturnTrue ()
        {
            Assert.True(hp_potion.IsHealingPotion);
        }

        [Fact]
        public void IfCrystal_IsCrystalReturnTrue()
        {
            Assert.True(c.IsCrystal);
        }

        [Fact]
        public void IfItem_IsHealingPotion_OrCrystal_ReturnFalse()
        {
            Assert.False(i.IsHealingPotion);
            Assert.False(i.IsCrystal);
        }

        [Fact]
        public void IfItemIsUsedByPlayer_UsedIsTrue() {
            Assert.False(hp_potion.used);

            p.SetBag(hp_potion);
            p.Use(hp_potion);

            Assert.True(hp_potion.used);
        }
        
        }
}
