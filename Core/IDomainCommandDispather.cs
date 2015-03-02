using System;
using System.Threading.Tasks;
using Core.Domain;

namespace Core
{
	public interface IDomainCommandDispatcher {
		Task Dispatch(IDomainCommand command);
	}
}