using System.Collections;
using Core.Domain;
using Core.Domain.Contexts.Ordering;

namespace Core {
	public interface ICashierRepository {
		Cashier GetById(Id id);
	}
}