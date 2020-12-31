using Core.Enums;
using Core.Exceptions.Application;
using Core.Extensions;
using Core.Models.DataStructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Services
{
	public class StructureMapper
	{
		public StructureDefinition SourceStructure { get; set; }
		public StructureDefinition DestinationStructure { get; set; }
		public StructureMapping Mapping { get; set; }

		public async Task<string> Map(string sourceJson)
		{
			try
			{
				if (await SourceStructure.ValidateJsonStructure(sourceJson))
				{
					JsonWriter jsonWriter = new JsonWriter();
					var mem = new MemoryStream(Encoding.UTF8.GetBytes(sourceJson));
					foreach (var mapItem in Mapping.Mappings)
					{
						await WriteToJson(mem,jsonWriter, mapItem);
					}
					return jsonWriter.ToJson();
				}
				throw new InvalidStructureException();
			}
			catch {
				throw new InvalidStructureException();
			}
		}

		private async Task WriteToJson(MemoryStream jsonStream, JsonWriter jsonWriter, FieldMapping mapItem)
		{
			var fromValue = await GetValue(jsonStream, SourceStructure, mapItem.FromField);
			var fromField = SourceStructure.GetFieldByPath(mapItem.FromField);
			var toField = DestinationStructure.GetFieldByPath(mapItem.ToField);
			if (fromField.IsDataTypeSimple() && toField.IsDataTypeSimple())
				Write(jsonWriter, fromValue, toField, mapItem.ToField);
		}

		private void Write(JsonWriter jsonWriter, JsonElement fromValue, Field field, string path)
		{
			switch (field.DataType)
			{
				case DataTypeEnum.Booelan:
					jsonWriter.SetValue(path, fromValue.GetBoolean());
					break;
				case DataTypeEnum.Integer:
				case DataTypeEnum.Float:
					jsonWriter.SetValue(path, fromValue.GetDouble());
					break;
				case DataTypeEnum.String:
				case DataTypeEnum.GUID:
					jsonWriter.SetValue(path, fromValue.GetString());
					break;
				case DataTypeEnum.DateTime:
				case DataTypeEnum.Date:
					jsonWriter.SetValue(path, fromValue.GetDateTime());
					break;
				case DataTypeEnum.Time:
					var val = fromValue.GetDateTime();
					jsonWriter.SetValue(path, new TimeSpan(val.Hour, val.Minute, val.Second));
					break;
				case DataTypeEnum.Binary:
					jsonWriter.SetValue(path, fromValue.GetBytesFromBase64());
					break;
				case DataTypeEnum.Void:
				case DataTypeEnum.Array:
					break;
				case DataTypeEnum.Object:
					break;
				default:
					break;
			}
		}

		private async Task<JsonElement> GetValue(Stream jsonStream, StructureDefinition structure, string fieldName)
		{
			using(JsonDocument doc = await JsonDocument.ParseAsync(jsonStream))
			{
				var element = doc.RootElement;

				var pathItems = fieldName.Split('.');

				Field field = null;
				foreach (var item in pathItems)
				{
					if (field == null)
						field = structure.Fields.Single(m => m.Name.Equals(item));
					else
						field = field.Structure.Fields.Single(m => m.Name.Equals(item));
					if (field == null)
						throw new InvalidStructureException();
					element = element.GetProperty(item);
				}

				return element;
			}
		}

		private void AddElement(Utf8JsonWriter jsonWriter, string toField, object value)
		{
			List<Field> fieldsPath = GetFieldFrom(DestinationStructure, toField);
			if(fieldsPath.Count == 1)
			{
				WriteJsonValue(jsonWriter, fieldsPath.First(), value);
			}else if(fieldsPath.Count > 1){
				for (int i = 0; i < fieldsPath.Count - 1; i++)
				{
					var field = fieldsPath[i];
					if(field.DataType == DataTypeEnum.Object)
					{
						jsonWriter.WriteStartObject(field.Name);
					}
				}
				WriteJsonValue(jsonWriter, fieldsPath.Last(), value);
				for (int i = 0; i < fieldsPath.Count - 1; i++)
				{
					var field = fieldsPath[i];
					if (field.DataType == DataTypeEnum.Object)
					{
						jsonWriter.WriteEndObject();
					}
				}
			}
		}

		private void WriteJsonValue(Utf8JsonWriter jsonWriter, Field field, object value)
		{
			
		}

		private List<Field> GetFieldFrom(StructureDefinition structure, string fieldName)
		{
			var path = fieldName.Split('.');
			var res = new List<Field>();
			Field field = null;
			if (path.Length > 0)
			{
				for (int i = 0; i < path.Length; i++)
				{
					string item = path[i];
					if (i == 0)
						field = structure.Fields.Single(m => m.Name == item);
					else
						field = field.Structure.Fields.Single(m => m.Name == item);
					res.Add(field);
				}
			}
			return res;
		}

		private string DictionaryToJson(Dictionary<string, object> destJsonValues) => JsonSerializer.Serialize(destJsonValues);

		private void AddToValueDictionary(Dictionary<string, object> destJsonValues, string toField, object value)
		{
			var path = toField.Split('.');
			Field field = null;
			if(path.Length > 0)
			{
				for (int i = 0; i < path.Length-1; i++)
				{
					string item = path[i];
					if (i == 0)
						field = DestinationStructure.Fields.Single(m => m.Name == item);
					else
						field = field.Structure.Fields.Single(m => m.Name == item);
					if (field.DataType == DataTypeEnum.Object)
					{
						destJsonValues[item] = new Dictionary<string, object>();
						destJsonValues = destJsonValues[item] as Dictionary<string, object>;
					}
					else if(field.DataType == DataTypeEnum.Array)
					{
						destJsonValues[item] = new Dictionary<string, object[]>();
						//destJsonValues = (Dictionary<string, object>)destJsonValues[item];
					}
				}
				destJsonValues[path[path.Length - 1]] = value;
			}
			else
			{
				destJsonValues[toField] = value;
			}
		}

		public async Task<string> Map(object input)
			=> await Map(JsonSerializer.Serialize(input));

		public async Task<T> Map<T>(string sourceJson)
			=> JsonSerializer.Deserialize<T>(await Map(sourceJson));

		public async Task<T> Map<T>(object input)
			=> await Map<T>(JsonSerializer.Serialize(input));
	}
}
