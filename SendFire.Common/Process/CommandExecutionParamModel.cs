using System;
using System.Collections.Generic;
using System.Text;

namespace SendFire.Common.Process
{
    public class CommandExecutionParamModel
    {
        public string Command { get; set; }
        public string Arguments { get; set; }
        public bool IsSysCommand { get; set; }
    }
}
