using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Domain;
using Core.Domain.Contexts.Production;

namespace Infrastructure.Services.Staff
{
	public class ChefRepository : IChefRepository
	{
		private static readonly IList<Chef> Chefs;

		static ChefRepository()
		{
			Chefs = new[]
			{
				new Chef(new Id(Guid.Parse("C0000000-0000-0000-0000-000000000001")), "Juan"),
				new Chef(new Id(Guid.Parse("C0000000-0000-0000-0000-000000000002")), "Jeremy"),
				new Chef(new Id(Guid.Parse("C0000000-0000-0000-0000-000000000003")), "Max"),
				new Chef(new Id(Guid.Parse("C0000000-0000-0000-0000-000000000004")), "Olivia")
			};
		}

		public Chef GetByName(string name)
		{
			return Chefs.Single(chef => chef.Name == name);
		}
	}
}