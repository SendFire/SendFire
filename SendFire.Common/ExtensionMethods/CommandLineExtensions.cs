using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SendFire.Common.CommandLine;

namespace SendFire.Common.ExtensionMethods
{
    public static class CommandLineExtensions
    {
        public static string CommandCollectionsAsExecutionOptions(this CommandCollection[] collections)
        {
            return collections.Select(c => c.CollectionName).Aggregate("", (current, collectionName) => current + $" [{collectionName}]");
        }

        public static List<string> CommandCollectionsAsArgumentDescriptions(this CommandCollection[] collections, int lineLength, int minArgumentPadding = 25)
        {
            var lines = new List<string>();
            foreach (var commandCollection in collections)
            {
                var maxArgumentLength = commandCollection.AvailableArguments.Max(a =>
                    $"  --{a.Command}:{a.CommandValueName} | -{a.SwitchMapping}".Length) + 2;
                if (maxArgumentLength < minArgumentPadding) maxArgumentLength = minArgumentPadding;

                lines.Add($"{commandCollection.CollectionName}:");
                foreach (var arg in commandCollection.AvailableArguments)
                {
                    var descriptionLines = arg.Description.SplitAtLines(lineLength - maxArgumentLength);
                    lines.Add(arg.WriteArgumentInfo(maxArgumentLength, descriptionLines[0]));
                    for (var inc = 1; inc < descriptionLines.Length; inc++)
                    {
                        lines.Add(string.Format("{0}{1}", ' '.MakeLine(maxArgumentLength), descriptionLines[inc]));
                    }
                    lines.Add(string.Empty);
                }
            }
            return lines;
        }

        public static string WriteArgumentInfo(this CommandLineArgument arg, int argumentPaddingLength, string descriptionFirstLine)
        { 
            var sb = new StringBuilder($"  --{arg.Command}");
            if (!arg.CommandValueName.IsNullOrEmpty())
            {
                sb.Append($":{arg.CommandValueName}");
            }
            else if (!arg.SwitchMapping.IsNullOrEmpty())
            {
                sb.Append($" | -{arg.SwitchMapping}");
            }
            sb.Append(' '.MakeLine(argumentPaddingLength - sb.Length));
            sb.Append(descriptionFirstLine);
            return sb.ToString();
        }
    }
}
