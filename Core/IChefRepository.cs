using Core.Domain.Contexts.Production;

namespace Core {
	public interface IChefRepository {
		Chef GetByName(string name);
	}
}