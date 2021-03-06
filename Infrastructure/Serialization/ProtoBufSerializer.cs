using System;
using System.IO;
using System.Linq;
using System.Text;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Infrastructure.Serialization
{
	class ProtoBufSerializer<TData> : Serializer<TData>{
		public override byte[] Serialize(TData data) {
			var type = data.GetType();

			if (!RuntimeTypeModel.Default.IsDefined(type))
				throw new NotSupportedException("Can not serialize " + type + " type is unknown");

			using (var ms = new MemoryStream()) {
				Serializer.Serialize(ms, data);

				var serializedResult = ms.ToArray();
				var typeData = Encoding.UTF8.GetBytes(type.Name);
				var result = new byte[1 + typeData.Length + serializedResult.Length];

				result[0] = (byte)typeData.Length;

				Array.Copy(typeData, 0, result, 1, typeData.Length);
				Array.Copy(serializedResult, 0, result, typeData.Length + 1, serializedResult.Length);

				return result;
			}

		}
		public override TData Deserialize(byte[] data) {
			using (var ms = new MemoryStream(data)) {
				var typeLength = new byte[1];
				ms.Read(typeLength, 0, 1);
				var typeData = new byte[typeLength[0]];
				ms.Read(typeData, 0, typeLength[0]);

				var type = RuntimeTypeModel.Default.GetTypes().Cast<MetaType>().Single(t => t.Type.Name == Encoding.UTF8.GetString(typeData));

				return (TData)Serializer.NonGeneric.Deserialize(type.Type, ms);
			}
		}
	}
}