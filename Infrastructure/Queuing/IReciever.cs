using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Queuing {
	public interface IReciever<TMessage> : IDisposable {
		void Recieve(Action<Task<TMessage>> onMessageRecieved, CancellationToken cancellationToken);
	}
}