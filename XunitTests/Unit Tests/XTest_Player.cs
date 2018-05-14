using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace STVRogue.GameLogic
{
    public class XTest_Player
    {
        Player p = new Player("1");
        Item i = new Item("2");
        HealingPotion hp = new HealingPotion("3");
        Crystal c = new Crystal("4");

        [Fact]
        public void IfPlayerIsCreatedStatsAreOK()
        {
            Assert.Equal(10, p.GetHP());
            Assert.Equal(5, p.GetAttackRating());
        }

        [Fact]
        public void IfPlayerPicksUpItem_BagContainsItem()
        {
            p.PickUp(i);
            Assert.Equal(p.GetBag()[0], i);
        }
    }
}
