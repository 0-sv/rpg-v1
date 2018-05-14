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
        public void UseOnEmptyBag()
        {
            Assert.Throws<ArgumentException>(() => p.Use(new Item()));
        }

        [Fact]
        public void UseItemInBag()
        {
            p.SetBag(i);
            p.Use(i);

            Assert.DoesNotContain(i, p.GetBag());
        }

        [Fact]
        public void IfPlayerUsesHPpotion_AndHPIsBase_ThenHpIsTheSame() {
            hp_before_hp_potion = p.GetHP();

            p.SetBag(hp_potion);
            p.Use(hp_potion);

            Assert.Equal(p.GetHP(), hp_before_hp_potion);
        }

        /* if the first test runs, then we can safely use an arbitrary value as HP as parameter */
        [Theory]
        [MemberData(nameof(HPData))]
        public void IfPlayerUsesHPpotion_AndHPIsLessThanBase_HPIsRestored (int value) {
            p.SetHP(value);

            p.SetBag(hp_potion);
            p.Use(hp_potion);

            if (value >= 7)
                Assert.Equal(10, p.GetHP());
            else 
                Assert.NotEqual(10, p.GetHP());
        }

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
        public void IfItemIsUsedWithoutPlayerAndItemIsUsed_DoNothing ()
        {
            i.Use(p);
            Assert.Throws<Exception>(() => i.Use(p));
        }

        [Fact]
        public void IfItemIsUsedByPlayer_UsedIsTrue() {
            Assert.False(hp_potion.GetUsed());

            p.SetBag(hp_potion);
            p.Use(hp_potion);

            Assert.True(hp_potion.GetUsed());
        }
        public static IEnumerable<object[]> HPData =>
            new List<object[]> { 
                new object[] { 0 },
                new object[] { 1 },
                new object[] { 2 },
                new object[] { 3 },
                new object[] { 4 },
                new object[] { 5 },
                new object[] { 6 },
                new object[] { 7 },
                new object[] { 8 },
                new object[] { 9 },
            };
        }
}
