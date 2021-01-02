using Core.Enums;
using Core.Models.DataStructure;
using System;
using System.Text.Json;

namespace Core.Extensions
{
	public static class JsonValueExtensions
	{
		public static bool IsSimpleType(this JsonValueKind jsonValueKind)
		{
			return !(jsonValueKind == JsonValueKind.Array ||
				jsonValueKind == JsonValueKind.Object);
		}
		public static bool IsSimpleType(this JsonElement element) => element.ValueKind.IsSimpleType();

		public static bool IsNullOrUndefined(this JsonValueKind jsonValueKind)
		{
			return jsonValueKind == JsonValueKind.Null
				|| jsonValueKind == JsonValueKind.Undefined;
		}

		public static bool IsNullOrUndefined(this JsonElement element) => element.ValueKind.IsNullOrUndefined();

		public static object Cast(this JsonElement fromValue, Field field)
		{
			switch (field.DataType)
			{
				case DataTypeEnum.Booelan:
					return fromValue.GetBoolean();
				case DataTypeEnum.Integer:
				case DataTypeEnum.Float:
					return fromValue.GetDouble();
				case DataTypeEnum.String:
				case DataTypeEnum.GUID:
					return fromValue.GetString();
				case DataTypeEnum.DateTime:
				case DataTypeEnum.Date:
					return fromValue.GetDateTime();
				case DataTypeEnum.Time:
					var val = fromValue.GetDateTime();
					return new TimeSpan(val.Hour, val.Minute, val.Second);
				case DataTypeEnum.Binary:
					return fromValue.GetBytesFromBase64();
				case DataTypeEnum.Void:
				case DataTypeEnum.Array:
					throw new NotImplementedException();
				case DataTypeEnum.Object:
					throw new NotImplementedException();
				default:
					throw new NotImplementedException();
			}
		}
	}
}