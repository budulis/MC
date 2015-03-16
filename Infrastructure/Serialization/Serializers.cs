using System;

namespace Infrastructure.Serialization
{
	public class Serializers{
		[Obsolete("Use Json instead. This one is only POC")]
		public static Serializer<TData> ProtoBuf<TData>() {
			return new ProtoBufSerializer<TData>(); 
		}

		public static Serializer<TData> Json<TData>() {
			return new JsonDataSerializer<TData>(); 
		}

		public static Serializer<TData> JsonNoTypeInfo<TData>() {
			return new JsonDataSerializerNoTypeInfo<TData>();
		}
	}
}