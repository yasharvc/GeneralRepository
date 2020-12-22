using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Core.Models
{
	public class JsonValue
	{
		public string Name { get; set; }
		public string Value { get; set; } = null;
		public List<JsonValue> ArrayValue { get; set; } = null;
		public IList<JsonValue> SubItems { get; set; } = new List<JsonValue>();

		public JsonValue this[string name]
		{
			get
			{
				var pathList = name.Split('.');
				JsonValue field = this;
				for (int i = 0; i < pathList.Length && field != null; i++)
				{
					var item = pathList[i];
					field = field.SubItems.SingleOrDefault(m => m.Name == name);
				}
				return field;
			}
			set
			{
				var pathList = name.Split('.');
				JsonValue field = this;
				for (int i = 0; i < pathList.Length; i++)
				{
					var item = pathList[i];
					if (!field.SubItems.Any(m => m.Name == item))
						field.SubItems.Add(new JsonValue { Name = item });
					field = field.SubItems.Single(m => m.Name == item);
				}
				field.SubItems = value.SubItems;
				field.ArrayValue = null;
				field.Value = null;
				if (value.IsArray())
					field.ArrayValue = value.ArrayValue;
				else if (value.IsSimple())
					field.Value = value.Value;
			}
		}

		public bool IsSimple() => !string.IsNullOrEmpty(Value);
		public bool IsArray() => ArrayValue != null;

		public string ToJson()
		{
			if (SubItems.Count > 0)
			{
				var subItemsJson = "";
				foreach (var item in SubItems)
				{
					subItemsJson += $"{(subItemsJson.Length > 0 ? "," : "")}{item.ToJson()}";
				}
				return $"{(string.IsNullOrEmpty(Name) ? "" : $"\"{Name}\":")}{{{subItemsJson}}}";
			}
			else if (IsArray())
			{
				var arrJson = "";
				foreach (var item in ArrayValue)
				{
					var itemJson = item.ToJson();
					arrJson += $"{(arrJson.Length > 0 ? "," : "")}{(itemJson.StartsWith("{") && !itemJson.StartsWith("[") ? "" : "{")}{itemJson}{(itemJson.EndsWith("}") && !itemJson.EndsWith("]") ? "" : "}")}";
				}
				return $"\"{Name}\":[{arrJson}]";
			}
			else if (IsSimple())
				return $"\"{Name}\":{Value}";
			else
				return null;
		}
	}
}