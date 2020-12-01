using Core.Enums;
using Core.Exceptions.Application;
using System.Text.Json;

namespace Core.Extensions
{
	public static class EnumExtensions
	{
		public static bool AreEqual(this BasicDataTypeEnum type,JsonValueKind kind)
		{
			if (type == BasicDataTypeEnum.Booelan)
				return (kind == JsonValueKind.False || kind == JsonValueKind.True);
			if (type == BasicDataTypeEnum.Float || type == BasicDataTypeEnum.Integer)
				return kind == JsonValueKind.Number;
			if (type == BasicDataTypeEnum.String)
				return kind == JsonValueKind.String;
			throw new InvalidComparisonException();
		}
	}
}