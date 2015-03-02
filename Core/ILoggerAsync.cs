namespace Core
{
	public interface ILoggerAsync : ILogger
	{
		void AuditAsync(object data);
		void WarningAsync(object data);
		void ErrorAsync(object data);
	}
}