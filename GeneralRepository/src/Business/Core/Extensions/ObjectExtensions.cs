using Core.Exceptions.Application;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Core.Extensions
{
	public static class ObjectExtensions
	{
		public static JsonTranslation ToGeneralDictionary(this object obj)
		{
			try
			{
				var res = new ObjectToDictionaryConverter(obj);
				var translated = res.Translate();
				return new JsonTranslation { Elements = translated, ElementValueKind = res.ElementValueKind };
			}
			catch (JsonException)
			{
				throw new InvalidJsonStringInputException();
			}
		}

		private class ObjectToDictionaryConverter
		{
			IDictionary<string,JsonElement> Elements { get; set; }
			public IDictionary<string, JsonValueKind> ElementValueKind { get; set; } = new Dictionary<string, JsonValueKind>();
			public string ParentPath { get; set; } = "";
			public ObjectToDictionaryConverter(object obj, string parentPath = "")
			{
				Elements = GetLowLevelDictionary(obj);
				ParentPath = parentPath;
			}

			public IDictionary<string,object> Translate(string parent="")
			{
				var result = new Dictionary<string, object>();
				foreach (var item in Elements)
				{
					result[item.Key] = JsonElementToValue(item.Value, item.Key);
					AddElementValueKind(item.Key, item.Value.ValueKind);
				}
				return result;
			}

			private void AddElementValueKind(string key, JsonValueKind valueKind)
			{
				var path = $"{ParentPath}{(string.IsNullOrEmpty(ParentPath) ? "" : ".")}{key}";
				AddToElementValueKindDictionary(new KeyValuePair<string, JsonValueKind>(path, valueKind));
			}

			private static IDictionary<string, JsonElement> GetLowLevelDictionary(object obj)
			{
				if (obj.GetType() == typeof(string))
					return JsonSerializer.Deserialize<IDictionary<string, JsonElement>>(obj as string);
				else
					return JsonSerializer.Deserialize<IDictionary<string, JsonElement>>(JsonSerializer.Serialize(obj));
			}

			private object JsonElementToValue(JsonElement value,string parent)
			{
				switch(value.ValueKind)
				{
					case JsonValueKind.String: return value.GetString();
					case JsonValueKind.False:
					case JsonValueKind.True: 
						return value.GetBoolean();
					case JsonValueKind.Number: return value.GetDouble();
					case JsonValueKind.Null:
					case JsonValueKind.Undefined: return null;
					case JsonValueKind.Object:
						{
							var converter = new ObjectToDictionaryConverter(value.GetRawText().ToGeneralDictionary().Elements, parent);
							var res = converter.Translate();
							foreach (var item in converter.ElementValueKind)
							{
								AddToElementValueKindDictionary(item);
							}
							return res;
						};
					case JsonValueKind.Array: return TranslateArray(value,parent);
					default:
						throw new NotImplementedException();
				};
			}

			private void AddToElementValueKindDictionary(KeyValuePair<string, JsonValueKind> item)
			{
				var path = item.Key;
				var valueKind = item.Value;
				if (ElementValueKind.ContainsKey(path) && ElementValueKind[path] != valueKind)
					throw new InconsistanceJsonStructureException();
				ElementValueKind[path] = valueKind;
			}

			private object TranslateArray(JsonElement value, string parent)
			{
				var res = new List<object>();
				foreach(var item in value.EnumerateArray())
					res.Add(JsonElementToValue(item,parent));
				return res;
			}
		}
	}
}