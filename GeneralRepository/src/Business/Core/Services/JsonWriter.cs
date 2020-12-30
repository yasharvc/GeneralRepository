using Core.Models;
using System;
using System.Collections.Generic;

namespace Core.Services
{
	public partial class JsonWriter
	{
		JsonValue Root = new JsonValue();

		public void SetValue(string path, string value)
		{
			CreatePath(path);
			Root[path] = new JsonValue { Value = $"\"{value}\"" };
		}
		public void SetValue(string path, DateTime value)
		{
			CreatePath(path);
			Root[path] = new JsonValue { Value = $"\"{value:O}\"" };
		}
		public void SetValue(string path, double value)
		{
			CreatePath(path);
			Root[path] = new JsonValue { Value = value.ToString() };
		}
		public void SetValue(string path, int value)
		{
			CreatePath(path);
			Root[path] = new JsonValue { Value = value.ToString() };
		}
		public void SetValue(string path, bool value)
		{
			CreatePath(path);
			Root[path] = new JsonValue { Value = value.ToString().ToLower() };
		}
		public void SetValue(string path, TimeSpan value)
		{
			CreatePath(path);
			Root[path] = new JsonValue { Value = value.ToString() };
		}
		public void SetValue(string path, byte[] value)
		{
			CreatePath(path);
			Root[path] = new JsonValue { Value = $"\"{Convert.ToBase64String(value)}\"" };
		}

		public void AddItemToArray(string path,JsonValue value)
		{
			CreatePath(path);
			if(Root[path] == null)
				Root[path] = new JsonValue { ArrayValue = new List<JsonValue>() };
			Root[path].ArrayValue.Add(value);
		}
		
		private void CreatePath(string path)
		{
			
		}

		public string ToJson()
		{
			//var res = "";
			//foreach (var item in Root.SubItems)
			//{
			//	res += $"{(res.Length > 0 ? "," : "")}\"{item.Name}\":{MakeJson(item)}";
			//}
			//return $"{{{res}}}";
			return Root.ToJson();
		}

		//private string MakeJson(JsonValue item)
		//{
		//	if(item.SubItems.Count > 0)
		//	{
		//		var res = "";
		//		foreach (var subItem in item.SubItems)
		//		{
		//			res += $"{(res.Length > 0 ? "," : "")}\"{subItem.Name}\":{subItem.ToJson()}";
		//		}
		//		return $"{{{res}}}";
		//	}
		//	if (item.IsSimple())
		//		return $"{item.ToJson()}";
		//	else if (item.IsArray())
		//		return $"[{GetArrayItems(item.ArrayValue)}]";
		//	else
		//		return null;
		//}

		//private string GetArrayItems(List<JsonValue> arrayValue)
		//{
		//	var res = "";
		//	foreach (var item in arrayValue)
		//	{
		//		res += $"{(res.Length > 0 ? "," : "")}{item.ToJson()}";
		//	}
		//	return res;
		//}
	}
}
