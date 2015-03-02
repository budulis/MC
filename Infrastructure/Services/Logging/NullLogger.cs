namespace Infrastructure.Services.Logging
{
	public class NullLogger : Core.ILogger
	{
		internal NullLogger(){}
		public void Audit(object data){}
		public void Warning(object data){}
		public void Error(object data){}
	}
}