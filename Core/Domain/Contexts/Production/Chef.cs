using System;
using System.Threading;

namespace Core.Domain.Contexts.Production {
	public class Chef : IEntity<Chef> {

		public Id Id { get; private set; }
		public string Name { get; private set; }
		public ILogger Logger { get; set; }

		public Chef(Id id, string name) {
			Id = id;
			Name = name;
		}

		public void Cook(ProductionOrder o) {
			foreach (var product in o.Products) {
				Logger.Audit(String.Format("Preparing [{0}]", product));
				//Simulate longrunning work	
				Thread.Sleep(1500);
			}
		}

		public bool Equals(Chef other) {
			return Id.Equals(other.Id);
		}

	}
}
