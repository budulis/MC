using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Serialization {
	class JsonDataSerializerNoTypeInfo<TData> : Serializer<TData> {

		public override byte[] Serialize(TData data) {
			var result = JsonConvert.SerializeObject(data);
#if DEBUG
			result = JToken.Parse(result).ToString(Formatting.Indented);
#endif
			return Encoding.UTF8.GetBytes(result);
		}

		public override TData Deserialize(byte[] data) {
			return JsonConvert.DeserializeObject<TData>(Encoding.UTF8.GetString(data));
		}
	}
}