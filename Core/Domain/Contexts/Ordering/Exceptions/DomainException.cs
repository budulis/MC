using System;
using System.Runtime;
using System.Runtime.Serialization;

namespace Core.Domain.Contexts.Ordering.Exceptions {
	[Serializable]
	internal class DomainException : Exception {
		public object Context { get; set; }
		public DomainException() { }
		public DomainException(string msg) : base(msg) { }
		public DomainException(string msg, DomainException ex) : base(msg, ex) { }
		public DomainException(SerializationInfo si, StreamingContext context) : base(si, context) { }
	}
}
