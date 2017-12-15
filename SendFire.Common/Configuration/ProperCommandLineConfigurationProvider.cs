using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace SendFire.Common.Configuration
{
    /// <summary>
    /// A command line based <see cref="ConfigurationProvider"/>.
    /// </summary>
    public class ProperCommandLineConfigurationProvider : ConfigurationProvider
    {
        private readonly Dictionary<string, string> _switchMappings;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="args">The command line args.</param>
        /// <param name="switchMappings">The switch mappings.</param>
        public ProperCommandLineConfigurationProvider(IEnumerable<string> args, IDictionary<string, string> switchMappings = null)
        {
            Args = args ?? throw new ArgumentNullException(nameof(args));

            if (switchMappings != null)
            {
                _switchMappings = GetValidatedSwitchMappingsCopy(switchMappings);
            }
        }

        /// <summary>
        /// The command line arguments.
        /// </summary>
        protected IEnumerable<string> Args { get; private set; }

        /// <summary>
        /// Loads the configuration data from the command line args.
        /// </summary>
        public override void Load()
        {
            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            using (var enumerator = Args.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var currentArg = enumerator.Current;
                    var keyStartIndex = 0;

                    if (currentArg.StartsWith("--"))
                    {
                        keyStartIndex = 2;
                    }
                    else if (currentArg.StartsWith("-"))
                    {
                        keyStartIndex = 1;
                    }
                    else if (currentArg.StartsWith("/"))
                    {
                        // "/SomeSwitch" is equivalent to "--SomeSwitch" when interpreting switch mappings
                        // So we do a conversion to simplify later processing
                        currentArg = string.Format("--{0}", currentArg.Substring(1));
                        keyStartIndex = 2;
                    }

                    var separator = currentArg.IndexOf('=');

                    string key;
                    string value;
                    if (separator < 0)
                    {
                        // If there is neither equal sign nor prefix in current arugment, it is an invalid format
                        if (keyStartIndex == 0)
                        {
                            throw new FormatException(Resources.FormatError_UnrecognizedArgumentFormat(currentArg));
                        }

                        // If the switch is a key in given switch mappings, interpret it
                        if (_switchMappings != null && _switchMappings.ContainsKey(currentArg))
                        {
                            key = _switchMappings[currentArg];
                        }
                        // If the switch starts with a single "-" and it isn't in given mappings , it is an invalid usage
                        else if (keyStartIndex == 1)
                        {
                            throw new FormatException(Resources.FormatError_ShortSwitchNotDefined(currentArg));
                        }
                        // Otherwise, use the switch name directly as a key
                        else
                        {
                            key = currentArg.Substring(keyStartIndex);
                        }

                        // A flag with no value parameter should default to being "true"
                        value = true.ToString();
                    }
                    else
                    {
                        var keySegment = currentArg.Substring(0, separator);

                        // If the switch is a key in given switch mappings, interpret it
                        if (_switchMappings != null && _switchMappings.ContainsKey(keySegment))
                        {
                            key = _switchMappings[keySegment];
                        }
                        // If the switch starts with a single "-" and it isn't in given mappings , it is an invalid usage
                        else if (keyStartIndex == 1)
                        {
                            throw new FormatException(Resources.FormatError_ShortSwitchNotDefined(currentArg));
                        }
                        // Otherwise, use the switch name directly as a key
                        else
                        {
                            key = currentArg.Substring(keyStartIndex, separator - keyStartIndex);
                        }

                        value = currentArg.Substring(separator + 1);

                        // Remove quotes and single quotes around escaped strings.
                        if(value.StartsWith('"') && value.EndsWith('"')) value = value.Substring(1, value.Length - 2);
                        if (value.StartsWith('\'') && value.EndsWith('\'')) value = value.Substring(1, value.Length - 2);
                    }

                    // Override value when key is duplicated. So we always have the last argument win.
                    data[key] = value;
                }
            }

            Data = data;
        }

        private Dictionary<string, string> GetValidatedSwitchMappingsCopy(IDictionary<string, string> switchMappings)
        {
            // The dictionary passed in might be constructed with a case-sensitive comparer
            // However, the keys in configuration providers are all case-insensitive
            // So we check whether the given switch mappings contain duplicated keys with case-insensitive comparer
            var switchMappingsCopy = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var mapping in switchMappings)
            {
                // Only keys start with "--" or "-" are acceptable
                if (!mapping.Key.StartsWith("-") && !mapping.Key.StartsWith("--"))
                {
                    throw new ArgumentException(
                        Resources.FormatError_InvalidSwitchMapping(mapping.Key),
                        nameof(switchMappings));
                }

                if (switchMappingsCopy.ContainsKey(mapping.Key))
                {
                    throw new ArgumentException(
                        Resources.FormatError_DuplicatedKeyInSwitchMappings(mapping.Key),
                        nameof(switchMappings));
                }

                switchMappingsCopy.Add(mapping.Key, mapping.Value);
            }

            return switchMappingsCopy;
        }
    }
}
