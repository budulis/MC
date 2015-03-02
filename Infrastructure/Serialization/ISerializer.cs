using System.Runtime.InteropServices;
using Core.Domain;

namespace Infrastructure.Serialization {
	public interface ISerializer<in TData> {
		byte[] Serialize(TData data);
	}
}