
namespace POC.Repository.Interface
{
    /// <summary>
    /// Interface for logging
    /// </summary>
    public interface ILogger
    {
        void Information(string message);

        void Critical(string message);

        void Error(string message);
    }
}