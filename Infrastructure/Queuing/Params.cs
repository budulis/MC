using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queuing {
	internal class Params {
		public class Queueing {
			public class QueueName {
				public const string ForDomainCommand = "Domain_Commands";
				public const string ForDomainEvent = "Domain_Events";
				public const string ForApplicationEvent = "Application_Events";

				public const string ForDomainCommandDeadLetter = "Domain_Commands_Dead_Letter";
				public const string ForDomainEventDeadLetter = "Domain_Events_Dead_Letter";
				public const string ForApplicationEventDeadLetter = "Application_Events_Dead_Letter";
			}
		}
	}
}
