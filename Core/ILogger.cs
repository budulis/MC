namespace Core
{
	public interface ILogger {
		void Audit(object data);
		void Warning(object data);
		void Error(object data);
	}
}