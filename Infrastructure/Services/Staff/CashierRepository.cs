using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Core.Domain;
using Core.Domain.Contexts.Ordering;

namespace Infrastructure.Services.Staff {
	public class CashierRepository : ICashierRepository
	{
		private static readonly IList<Cashier> Cashiers;

		static CashierRepository()
		{
			Cashiers = new List<Cashier>
				{
					new Cashier(new Id(Guid.Parse("CA000000-0000-0000-0000-000000000001")), "John"),
					new Cashier(new Id(Guid.Parse("CA000000-0000-0000-0000-000000000002")), "Marry"),
					new Cashier(new Id(Guid.Parse("CA000000-0000-0000-0000-000000000003")), "Travis"),
				};
		}
		public Cashier GetById(Id id) {
			return Cashiers.Single(x => Equals(x.Id, id));
		}
	}
}
