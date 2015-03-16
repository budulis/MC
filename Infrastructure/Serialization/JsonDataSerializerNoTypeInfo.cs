using System.Text;
using Newtonsoft.Json;

namespace Infrastructure.Serialization
{
	class JsonDataSerializerNoTypeInfo<TData> : Serializer<TData> {

		public override byte[] Serialize(TData data) {
			return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
		}

		public override TData Deserialize(byte[] data) {
			return JsonConvert.DeserializeObject<TData>(Encoding.UTF8.GetString(data));
		}
	}
}