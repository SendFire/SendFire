using System;
using System.Collections.Generic;
using System.Text;

namespace SendFire.Common.CommandLine
{
    public class CommandLineArgument
    {
        public string Command { get; set; }
        public string CommandValueName { get; set; }
        public string SwitchMapping { get; set; }
        public string Description { get; set; }
    }
}
