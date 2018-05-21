using STVRogue;
using STVRogue.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace STVRogue.GameLogic
{
    public class XTest_Command
    {
        Player p = new Player("Test");

        [Fact]
        public void WrongCommandThrowsException()
        {
            Command c = new Command(p, ConsoleKey.B);
            Assert.Throws<NotSupportedException>(() => c.Execute());
        }

        [Fact]
        public void LeftArrowExecutesGoLeft()
        {
            Command c = new Command(p, ConsoleKey.LeftArrow);
            Assert.Throws<NotImplementedException>(() => c.Execute());
        }

        [Fact]
        public void DownArrowExecutesGoLeft()
        {
            Command c = new Command(p, ConsoleKey.DownArrow);
            Assert.Throws<NotImplementedException>(() => c.Execute());
        }

        [Fact]
        public void RightArrowExecutesGoLeft()
        {
            Command c = new Command(p, ConsoleKey.RightArrow);
            Assert.Throws<NotImplementedException>(() => c.Execute());
        }

        [Fact]
        public void UpArrowExecutesGoLeft()
        {
            Command c = new Command(p, ConsoleKey.UpArrow);
            Assert.Throws<NotImplementedException>(() => c.Execute());
        }
    }
}
