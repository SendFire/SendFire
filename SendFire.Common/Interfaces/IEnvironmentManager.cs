namespace SendFire.Common.Interfaces
{
    public interface IEnvironmentManager
    {
        string GetEnvironmentVariable(string variable);
        void SetEnvironmentVariable(string variable, string value);
    }
}
