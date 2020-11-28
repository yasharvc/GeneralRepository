using Core.Exceptions.Application;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Extensions
{
	public static class ObjectExtensions
	{
		public static IDictionary<string, object> ToGeneralDictionary(this object obj)
		{
			try
			{
				return new ObjectToDictionaryConverter(obj).Translate();
			}
			catch (JsonException)
			{
				throw new InvalidJsonStringInputException();
			}
		}

		private class ObjectToDictionaryConverter
		{
			public IDictionary<string,JsonElement> Elements { get; set; }
			public ObjectToDictionaryConverter(object obj)
			{
				Elements = GetLowLevelDictionary(obj);
			}

			public IDictionary<string,object> Translate()
			{
				var result = new Dictionary<string, object>();
				foreach (var item in Elements)
					result[item.Key] = JsonElementToValue(item.Value);
				return result;
			}

			private static IDictionary<string, JsonElement> GetLowLevelDictionary(object obj)
			{
				if (obj.GetType() == typeof(string))
					return JsonSerializer.Deserialize<IDictionary<string, JsonElement>>(obj as string);
				else
					return JsonSerializer.Deserialize<IDictionary<string, JsonElement>>(JsonSerializer.Serialize(obj));
			}

			private static object JsonElementToValue(JsonElement value)
			{
				return value.ValueKind switch
				{
					JsonValueKind.String => value.GetString(),
					JsonValueKind.False or JsonValueKind.True => value.GetBoolean(),
					JsonValueKind.Number => value.GetDouble(),
					JsonValueKind.Null or JsonValueKind.Undefined => null,
					JsonValueKind.Object => new ObjectToDictionaryConverter(value.GetRawText().ToGeneralDictionary()).Translate(),
					JsonValueKind.Array => TranslateArray(value),
					_ => throw new NotImplementedException(),
				};
			}

			private static object TranslateArray(JsonElement value)
			{
				var res = new List<object>();
				foreach(var item in value.EnumerateArray())
					res.Add(JsonElementToValue(item));
				return res;
			}
		}
	}
}