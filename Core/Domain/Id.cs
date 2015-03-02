using System;

namespace Core.Domain
{
	public class Id : IEquatable<Id>,ISerializable
	{
		public Guid Value { get; private set; }

		public Id(Guid value)
		{
			Value = value;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((Id) obj);
		}
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
		public bool Equals(Id other)
		{
			return Value.Equals(other.Value);
		}
		public override string ToString()
		{
			return Value.ToString("N");
		}
		public static Id New()
		{
			return new Id(Guid.NewGuid());
		}
		public static Id Null() {
			return new NullId();
		}
	}
}