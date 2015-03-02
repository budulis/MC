using System.Threading.Tasks;
using Core.Domain;

namespace Core.Subscribers {
	public interface ISubscriber<in TEvent> {
		Task Notify(TEvent evt);
	}
}