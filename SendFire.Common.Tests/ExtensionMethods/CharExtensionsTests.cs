using System;
using System.Collections.Generic;
using System.Text;
using SendFire.Common.ExtensionMethods;
using Xunit;

namespace SendFire.Common.Tests.ExtensionMethods
{
    public class CharExtensionsTests
    {
        [Theory,
            InlineData('-', 50, "--------------------------------------------------"),
            InlineData('*', 1, "*"),
            InlineData('$', 5, "$$$$$")]
        public void MakeLine(char c, int count, string expected)
        {
            Assert.Equal(c.MakeLine(count), expected);
        }

        [Theory,
         InlineData('-', "this a really long title that rocks (40)", "----------------------------------------"),
         InlineData('*', "A", "*"),
         InlineData('$', "FIVER", "$$$$$")]
        public void MakeTitleLine(char c, string title, string expected)
        {
            Assert.Equal(c.MakeTitleLine(title), expected);
        }
    }
}
