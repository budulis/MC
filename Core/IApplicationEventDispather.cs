using Core.Application.Messages;
using System;
using System.Threading.Tasks;

namespace Core {
	public interface IApplicationEventDispather : IDisposable {
		Task Dispatch(IApplicationEventNotificationMessage evt);
	}
}