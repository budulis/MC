using System;
using System.Collections;

namespace Core.Domain
{
	public interface IDomainCommand : ISerializable {
		Id Id { get; }
	}
}