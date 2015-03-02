using System;

namespace Infrastructure.Services.Logging {
	public class ConsoleLogger : Core.ILogger {

		private readonly ConsoleColor _color;

		internal ConsoleLogger() {
			_color = Console.ForegroundColor;
		}

		public void Audit(object data) {
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("[Audit event] {0}", data);
			Console.ForegroundColor = _color;
		}

		public void Warning(object data) {
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("[Warning event] {0}", data);
			Console.ForegroundColor = _color;
		}

		public void Error(object data) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("[Error event] {0}", data);
			Console.ForegroundColor = _color;
		}
	}
}
