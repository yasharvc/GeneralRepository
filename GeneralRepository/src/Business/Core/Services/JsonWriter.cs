using Core.Models;
using System;
using System.Collections.Generic;

namespace Core.Services
{
	public partial class JsonWriter
	{
		JsonValue Root = new JsonValue();

		public JsonValue GetRoot() => Root;

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

		public void SetValue(string path, object value)
		{
			if(value.GetType() == typeof(string))
				SetValue(path, (string)value);
			else if(value.GetType() == typeof(bool))
				SetValue(path, Convert.ToBoolean(value));
			else if (value.GetType() == typeof(TimeSpan))
				SetValue(path, (TimeSpan)value);
			else if (value.GetType() == typeof(int))
				SetValue(path, (int)value);
			else if (value.GetType() == typeof(double))
				SetValue(path, (double)value);
			else
				throw new InvalidCastException();
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
			return Root.ToJson();
		}
	}
}
