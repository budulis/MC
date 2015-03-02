using Core.Domain;

namespace Core {
	public interface IApplicationCommand {
		Id Id { get; }
	}
}
