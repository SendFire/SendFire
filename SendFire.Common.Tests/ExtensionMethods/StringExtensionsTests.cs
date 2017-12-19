using System.Collections.Generic;
using System.Linq;
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

        [Theory,
            MemberData(nameof(SplitAtLinesTestData))]
        public void SplitAtLines(string paragraph, int lineLength, string[] expectedOutput)
        {
            var lines = paragraph.SplitAtLines(lineLength).ToArray();
            Assert.True(lines.Length == expectedOutput.Length, $"Output and expected output not the same number of lines {lines.Length} / {expectedOutput.Length}.");
            for (var inc = 0; inc < expectedOutput.Length; inc++)
            {
                Assert.Equal(lines[inc], expectedOutput[inc]);
            }
        }

        const string _sourceParagraph = @"""Now is the time for all good men to come to the aid of the party"" is a phrase first proposed as a typing drill by instructor Charles E.Weller; its use is recounted in his book The Early History of the Typewriter, p. 21 (1918).";

        public static IEnumerable<object[]> SplitAtLinesTestData => new[]
        {
            new object[] {
                _sourceParagraph,
                40, 
                new[] {
                     //----------------------------------------
                    @"""Now is the time for all good men to",
                     @"come to the aid of the party"" is a",
                      "phrase first proposed as a typing drill",
                      "by instructor Charles E.Weller; its use",
                      "is recounted in his book The Early",
                      "History of the Typewriter, p. 21 (1918)." 
                }
            }, new object[]  {
                _sourceParagraph,
                60,
                new[] {
                    //------------------------------------------------------------
                    @"""Now is the time for all good men to come to the aid of the",
                    @"party"" is a phrase first proposed as a typing drill by",
                     "instructor Charles E.Weller; its use is recounted in his",
                     "book The Early History of the Typewriter, p. 21 (1918)."
                }
            }
        };
    }
}
