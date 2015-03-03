using System.Text;
using Newtonsoft.Json;

namespace Infrastructure.Serialization
{
	class JsonDataSerializer<TData> : Serializer<TData>{
		public override byte[] Serialize(TData data) {
			return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data, new JsonSerializerSettings {
				TypeNameHandling = TypeNameHandling.Objects
			}));
		}

		public override TData Deserialize(byte[] data) {
			return JsonConvert.DeserializeObject<TData>(Encoding.UTF8.GetString(data),
				new JsonSerializerSettings {
					TypeNameHandling = TypeNameHandling.Objects
				});
		}
	}
}