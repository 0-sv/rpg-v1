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
        [Fact]
        public void IfPlayerUsesHPpotionAndHPIsMaxThenHpIsTheSame() {
            Player p = new Player();
            int hp_before_hp_potion = p.GetHP();
            HealingPotion hp_potion = new HealingPotion("1");
            
            p.Use(hp_potion);

            Assert.Equal(p.GetHP(), hp_before_hp_potion);
        }
	}
}
