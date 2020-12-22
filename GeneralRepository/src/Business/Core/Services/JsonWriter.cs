using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Core.Services
{
	public class JsonWriter
	{
		class Jsonvalue
		{
			public string Name { get; set; }
			public string SimpleValue { get; set; } = null;
			public object ObjectValue { get; set; } = null;
			public List<object> ArrayValue { get; set; } = null;
			public IList<Jsonvalue> SubItems { get; set; } = new List<Jsonvalue>();

			public Jsonvalue this[string name]
			{
				get
				{
					var pathList = name.Split('.');
					Jsonvalue field = null;
					if (pathList.Length > 0)
					{
						for (int i = 0; i < pathList.Length - 1; i++)
						{
							var item = pathList[i];
							if (i == 0)
								field = this;
							else
								field = field.SubItems.Single(m => m.Name == name);
						}
					}
					return field;
				}
				set
				{
					var pathList = name.Split('.');
					Jsonvalue field = null;
					if (pathList.Length > 1)
					{
						for (int i = 0; i < pathList.Length - 1; i++)
						{
							var item = pathList[i];
							if (i == 0)
								field = this;
							else
							{
								if (!field.SubItems.Any(m => m.Name == item))
									field.SubItems.Add(new Jsonvalue { Name = item });
								field = field.SubItems.Single(m => m.Name == item);
							}
						}
					}else if(pathList.Length == 1)
					{
						string temp = pathList[0];
						if (!SubItems.Any(m => m.Name == temp))
							SubItems.Add(new Jsonvalue { Name = temp });
						field = SubItems.Single(m => m.Name == temp);
					}
					field.SubItems = value.SubItems;
					field.ArrayValue = null;
					field.SimpleValue = null;
					field.ObjectValue = null;
					if(value.IsObject())
						field.ObjectValue = value.ObjectValue;
					else if(value.IsArray())
						field.ArrayValue = value.ArrayValue;
					else if (value.IsSimple())
						field.SimpleValue = value.SimpleValue;
				}
			}

			public bool IsSimple() => !string.IsNullOrEmpty(SimpleValue);
			public bool IsObject() => ObjectValue != null;
			public bool IsArray() => ArrayValue != null && ArrayValue.Count > 0;

			public string ToJson()
			{
				if (IsObject())
					return JsonSerializer.Serialize(ObjectValue);
				else if (IsArray())
					return JsonSerializer.Serialize(ArrayValue);
				else if (IsSimple())
					return SimpleValue;
				else
					return null;
			}
		}
		Jsonvalue Root = new Jsonvalue();

		public void SetValue(string path, string value)
		{
			CreatePath(path);
			Root[path] = new Jsonvalue { SimpleValue = $"\"{value}\"" };
		}
		public void SetValue(string path, DateTime value)
		{
			CreatePath(path);
			Root[path] = new Jsonvalue { SimpleValue = $"\"{value:O}\"" };
		}
		public void SetValue(string path, double value)
		{
			CreatePath(path);
			Root[path] = new Jsonvalue { SimpleValue = value.ToString() };
		}
		public void SetValue(string path, int value)
		{
			CreatePath(path);
			Root[path] = new Jsonvalue { SimpleValue = value.ToString() };
		}
		public void SetValue(string path, bool value)
		{
			CreatePath(path);
			Root[path] = new Jsonvalue { SimpleValue = value.ToString().ToLower() };
		}
		public void SetValue(string path, TimeSpan value)
		{
			CreatePath(path);
			Root[path] = new Jsonvalue { SimpleValue = value.ToString() };
		}
		public void SetValue(string path, byte[] value)
		{
			CreatePath(path);
			Root[path] = new Jsonvalue { SimpleValue = $"\"{Convert.ToBase64String(value)}\"" };
		}
		public void SetObjectValue(string path, object value)
		{
			CreatePath(path);
			Root[path] = new Jsonvalue { ObjectValue = value };
		}

		public void SetArrayValue(string path, List<object> value)
		{
			CreatePath(path);
			Root[path] = new Jsonvalue { ArrayValue = value };
		}
		private void CreatePath(string path)
		{
			
		}

		public string ToJson()
		{
			var res = "";
			foreach (var item in Root.SubItems)
			{
				res += MakeJson(item);
			}
			return $"{{{res}}}";
		}

		private string MakeJson(Jsonvalue item)
		{
			return $"\"{item.Name}\":{item.ToJson()}";
		}
	}
}
