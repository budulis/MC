using System;
using Core;

namespace Infrastructure.Services.Logging
{
	public class LoggerFactory
	{
		private static readonly Lazy<ILogger> Console;
		private static readonly Lazy<ILogger> Null;

		static LoggerFactory()
		{
			Console = new Lazy<ILogger>(() => new ConsoleLogger());
			Null = new Lazy<ILogger>(()=>new NullLogger());
		}

		public static ILogger Default {
			get { return Null.Value; }
		}
		public static ILogger Get<TLogger>()
		{
			if (typeof (TLogger) == typeof (ConsoleLogger))
				return Console.Value;
			if (typeof (TLogger) == typeof (NullLogger))
				return Null.Value;

			throw new NotSupportedException();
		}
	}
}