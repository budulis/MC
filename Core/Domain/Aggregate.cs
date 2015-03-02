using System.Collections.Generic;
using System.Linq;

namespace Core.Domain
{
	public abstract class Aggregate<TEntity,TDomainEvent> : IEntity<TEntity> where TDomainEvent : IDomainEvent
		where TEntity : IEntity<TEntity>
	{
		public Id Id { get; internal set; }
		private readonly List<TDomainEvent> _events;
		public IEnumerable<TDomainEvent> Events { get; private set; }

		public int CurrentSequenceNumber = 0;

		protected Aggregate(Id id)
		{
			Id = id;
			_events = new List<TDomainEvent>();
			Events = _events.AsEnumerable();
		}

		public bool Equals(TEntity other)
		{
			return Id.Equals(other.Id);
		}

		public virtual void ApplyEvent(TDomainEvent evt)
		{
			_events.Add(evt);
		}
		
		internal virtual void UpdateFromEvent(TDomainEvent evt)
		{
			CurrentSequenceNumber += 1;	
		}
	}
}