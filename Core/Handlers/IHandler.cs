using System.Threading.Tasks;

namespace Core.Handlers
{
	public interface IHandler<in TDomainCommand>
	{
		Task Handle(TDomainCommand command);
	}
}