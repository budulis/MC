using System;
using System.Threading.Tasks;

namespace Infrastructure.Queuing {
	public interface ISender<in TMessage> : IDisposable {
		Task SendAsync(TMessage message);
	}
}