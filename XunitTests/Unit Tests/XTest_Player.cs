using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace STVRogue.GameLogic
{
    /* An example of a test class written using XUnit testing framework. 
     * This one is to unit-test the class Player. The test is incomplete though, 
     * as it only contains two test cases. 
     */
    public class XTest_Player
    {
        Player p = new Player();
        [Fact]
        public void UseOnEmptyBag()
        {
            Assert.Throws<ArgumentException>(() => p.Use(new Item()));
        }

        [Fact]
        public void UseItemInBag()
        {
            Item x = new HealingPotion("pot1");
            p.bag.Add(x);
            p.Use(x);
            Assert.DoesNotContain(x, p.bag);
        }
    }
}
