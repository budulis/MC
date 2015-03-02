using System;
using Core.Domain;

namespace Core.Application.Messages
{
	public interface IApplicationEventNotificationMessage : ISerializable {
		Id Id { get; set; }
		string Sender { get; set; }
		DateTime Date { get; set; }
		string ReplyTo { get; set; }
	}
}