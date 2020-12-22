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
			if (await SourceStructure.ValidateJsonStructure(sourceJson))
			{
				var destJsonValues = new Dictionary<string, object>();
				foreach (var mapItem in Mapping.Mappings)
				{
					var value = await SourceStructure.GetValue(mapItem.FromField, sourceJson);
					AddToValueDictionary(destJsonValues, mapItem.ToField, value);
				}
				return DictionaryToJson(destJsonValues);
			}
			throw new InvalidStructureException();
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
			switch (field.DataType)
			{
				case DataTypeEnum.Booelan:
					jsonWriter.WriteBoolean(field.Name, Convert.ToBoolean(value));
					break;
				case DataTypeEnum.Integer:
				case DataTypeEnum.Float:
					jsonWriter.WriteNumber(field.Name, Convert.ToDouble(value));
					break;
				case DataTypeEnum.String:
				case DataTypeEnum.GUID:
					jsonWriter.WriteString(field.Name, Convert.ToString(value));
					break;
				case DataTypeEnum.DateTime:
				case DataTypeEnum.Date:
					jsonWriter.WriteString(field.Name, Convert.ToDateTime(value));
					break;
				case DataTypeEnum.Time:
					jsonWriter.WriteString(field.Name, ((TimeSpan)value).ToString());
					break;
				case DataTypeEnum.Binary:
					jsonWriter.WriteBase64String(field.Name, value.ToString().ToBytes());
					break;
				case DataTypeEnum.Void:
				case DataTypeEnum.Object:
				case DataTypeEnum.Array:
					break;
				default:
					break;
			}
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
