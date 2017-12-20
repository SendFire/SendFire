
namespace SendFire.Common.Process
{
    public class CommandExecutionParamModel
    {
        public string Command { get; set; }
        public string Arguments { get; set; }
       
    }

    public class CommandExecutionFileModel
    {
        public string FileName { get; set; }
        public bool IsUnixOS { get; set; }
    }
}
