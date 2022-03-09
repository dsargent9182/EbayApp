namespace DS.Lib.Logger
{
	public interface ILoggerManager
	{
		void LogInfo(string message);

		void LogError(string message);

		void LogError(string message, Exception ex);
	}
}