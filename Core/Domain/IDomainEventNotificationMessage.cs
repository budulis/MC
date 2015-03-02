using System;
using Core.Application;

namespace Core.Domain
{
	public interface IDomainEventNotificationMessage : ISerializable {
		Id Id { get; set; }
		DateTime Date { get; set; }
		string ReplyTo { get; set; }
	}
}