using System;
using System.Collections.Generic;

namespace Core.FclExtensions {
	public abstract class SemanticType<T> : IEquatable<SemanticType<T>> where T : IEquatable<T> {
		public T Value { get; private set; }

		protected SemanticType(Func<T, bool> isValid, T item) {
			if (isValid != null && !isValid(item))
				throw new Exception("Can not create object of: " + item);

			Value = item;
		}

		public bool Equals(SemanticType<T> other) {
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			if (other.GetType() != GetType()) return false;
			return Value.Equals(other.Value);
		}

		public static bool operator ==(SemanticType<T> t1, SemanticType<T> t2) {
			return !(t1 == null) && t1.Equals(t2);
		}

		public static bool operator !=(SemanticType<T> t1, SemanticType<T> t2) {
			return !(t1 == t2);
		}

		public override bool Equals(object obj) {
			return Equals((SemanticType<T>)obj);
		}

		public override int GetHashCode() {
			unchecked {
				return EqualityComparer<T>.Default.GetHashCode(Value);
			}
		}
	}
}
