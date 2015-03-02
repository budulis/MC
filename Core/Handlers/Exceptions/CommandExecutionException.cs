using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Handlers.Exceptions {
	[Serializable]
	public class CommandExecutionException : Exception {

		public CommandExecutionException(Exception ex)
			: base(null, ex) {
		}

		public CommandExecutionException(string message, Exception ex)
			: base(message, ex) {
		}
	}
}
