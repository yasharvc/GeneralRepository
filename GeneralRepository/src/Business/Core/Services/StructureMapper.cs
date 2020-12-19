using Core.Exceptions.Application;
using Core.Models.DataStructure;
using System;
using System.Collections.Generic;
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
			if(await SourceStructure.ValidateJsonStructure(sourceJson))
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

		private string DictionaryToJson(Dictionary<string, object> destJsonValues) => JsonSerializer.Serialize(destJsonValues);

		private void AddToValueDictionary(Dictionary<string, object> destJsonValues, string toField, object value)
		{
			var path = toField.Split('.');
			if(path.Length > 0)
			{
				for (int i = 0; i < path.Length-1; i++)
				{
					string item = path[i];
					destJsonValues[item] = new Dictionary<string, object>();
					destJsonValues = destJsonValues[item] as Dictionary<string,object>;
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
