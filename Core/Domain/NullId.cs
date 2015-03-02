using System;

namespace Core.Domain
{
	class NullId : Id
	{
		public NullId() : base(Guid.Empty)
		{
		}
	}
}