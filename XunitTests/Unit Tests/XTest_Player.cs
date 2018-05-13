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
        Player p;
        [Fact]
        public void IfPlayerIsCreatedStatsAreOK()
        {
            p = new Player("Mark");
            Assert.Equal(100, p.GetHP());
            Assert.Equal(5, p.GetAttackRating());
        }
    }
}
