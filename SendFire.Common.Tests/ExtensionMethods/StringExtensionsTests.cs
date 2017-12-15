using System.Net;
using SendFire.Common.ExtensionMethods;
using Xunit;

namespace SendFire.Common.Tests.ExtensionMethods.StringExtensions
{
    public class StringExtensionsTests
    {
        [Theory,
            InlineData("lowercase", "LOWERCASE", true),
            InlineData("LOwercasE", "lowerCasE", true),
            InlineData("lowercase", "lowercase", true),
            InlineData("lowercase", "uppercase", false),
            InlineData("lowercasE", "lowercase ", false),
            InlineData("lowercase1", "lowercase", false),
            InlineData(" lowercase", "lowercase", false)]
        public void CompareNoCase(string stringA, string stringB, bool expected)
        {
            Assert.Equal(expected, stringA.CompareNoCase(stringB));
        }

        [Theory,
            InlineData("tRue", true),
            InlineData("t", true),
            InlineData("1", true),
            InlineData("Y", true),
            InlineData("YeS", true),
            InlineData("FaLse", false),
            InlineData("F", false),
            InlineData("0", false),
            InlineData("n", false),
            InlineData("nO", false)]
        public void IsTrue(string arg, bool expected)
        {
            Assert.Equal(expected, arg.IsTrue());
        }

        [Theory,
         InlineData("tRue", false),
         InlineData("t", false),
         InlineData("1", false),
         InlineData("Y", false),
         InlineData("YeS", false),
         InlineData("FaLse", true),
         InlineData("F", true),
         InlineData("0", true),
         InlineData("n", true),
         InlineData("nO", true)]
        public void IsFalse(string arg, bool expected)
        {
            Assert.Equal(expected, arg.IsFalse());
        }

        [Theory,
         InlineData("tRue", false),
         InlineData("FaLse", false),
         InlineData("-1", true),
         InlineData("red", true),
         InlineData("ILikePuppies", true),
         InlineData("n0", true)]
        public void IsNotTrueAndNotFalse(string arg, bool expected)
        {
            Assert.Equal(expected, !arg.IsFalse() && !arg.IsTrue());
        }
    }
}
