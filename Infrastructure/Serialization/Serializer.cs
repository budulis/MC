
namespace Infrastructure.Serialization {
	public abstract class Serializer<TData> : ISerializer<TData>,IDeserializer<TData>
	{
		public abstract byte[] Serialize(TData data);
		public abstract TData Deserialize(byte[] data);
	}
}