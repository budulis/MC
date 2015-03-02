using System;

namespace Core.Domain
{
	public interface IEntity<T> : IEquatable<T>
	{
		Id Id { get; }	
	}
}