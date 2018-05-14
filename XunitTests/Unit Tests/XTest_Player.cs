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
        int hp_before_hp_potion;

        Player p = new Player("1");
        Item i = new Item("2");
        HealingPotion hp_potion = new HealingPotion("3");
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
        [Fact]
        public void UseOnEmptyBag()
        {
            Assert.Throws<ArgumentException>(() => p.Use(new Item()));
        }

        [Fact]
        public void UseHealingPotionInBag_BagShouldBeEmpty()
        {
            p.SetBag(hp_potion);
            p.Use(hp_potion);

            Assert.DoesNotContain(hp_potion, p.GetBag());
        }

        [Fact]
        public void UseCrystalInBag_BagShouldBeEmpty()
        {
            p.SetBag(c);
            p.Use(c);

            Assert.DoesNotContain(c, p.GetBag());
        }

        [Fact]
        public void UseItemInBag_BagShouldBeEmpty()
        {
            p.SetBag(i);
            p.Use(i);

            Assert.DoesNotContain(i, p.GetBag());
        }

        [Fact]
        public void TwoItemsInBag_AndUseOneItem_Should_ContainOneItem()
        {
            p.SetBag(i);
            p.SetBag(i);

            p.Use(i);

            Assert.Contains(i, p.GetBag());
        }

        [Fact]
        public void UseCrystal_AcceleratedIsTrue()
        {
            p.SetBag(c);

            p.Use(c);

            Assert.True(p.GetAccelerated());
        }

        [Fact]
        public void IfPlayerUsesHPpotion_AndHPIsBase_ThenHpIsTheSame()
        {
            hp_before_hp_potion = p.GetHP();

            p.SetBag(hp_potion);
            p.Use(hp_potion);

            Assert.Equal(p.GetHP(), hp_before_hp_potion);
        }

        /* if the first test runs, then we can safely use an arbitrary value as HP as parameter */
        [Theory]
        [MemberData(nameof(HPData))]
        public void IfPlayerUsesHPpotion_AndHPIsLessThanBase_HPIsRestored(int value)
        {
            p.SetHP(value);
            p.SetBag(hp_potion);
            p.Use(hp_potion);

            if (value >= 7)
                Assert.Equal(10, p.GetHP());
            else
                Assert.NotEqual(10, p.GetHP());
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
