namespace Infrastructure.Serialization
{
	public interface IDeserializer<out TData> {
		TData Deserialize(byte[] data);
	}
}