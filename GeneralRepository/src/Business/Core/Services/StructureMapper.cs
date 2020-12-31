using Core.Extensions;
using Core.Models.DataStructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Services
{
	public class StructureMapper
	{
		public StructureDefinition SourceStructure { get; set; }
		public StructureDefinition DestinationStructure { get; set; }
		public StructureMapping Mapping { get; set; }

		public async Task<string> Map(string sourceJson) {
			var jsonWriter = new JsonWriter();
			ExtractValue(SourceStructure, Mapping, await JsonDocument.ParseAsync(new MemoryStream(sourceJson.ToBytes())), jsonWriter);
			return jsonWriter.ToJson();
		}

		void ExtractValue(StructureDefinition fromStructure, StructureMapping mapping, JsonDocument doc, JsonWriter jsonWriter, Field fromField = null)
		{
			foreach (var element in doc.RootElement.EnumerateObject())
			{
				if (mapping.Mappings.Any(m => m.FromField.StartsWithIgnoreCase(element.Name)))
				{
					var itemsStartsWithElementName = mapping.Mappings.Where(m => m.FromField.StartsWithIgnoreCase(element.Name));
					if (element.Value.IsSimpleType())
					{
						fromField = (fromField?.Structure ?? fromStructure).Fields.SingleOrDefault(m => m.Name.EqualsIgnoreCase(element.Name));
						if (fromField == null)
							throw new Exception();
						GetSimpleProperty(jsonWriter, fromField, element, itemsStartsWithElementName);
					}
					else if (element.Value.ValueKind == JsonValueKind.Object)
					{
						fromField = (fromField?.Structure ?? fromStructure).Fields.SingleOrDefault(m => m.Name.EqualsIgnoreCase(element.Name));
						if (fromField == null)
							throw new Exception();
						GetObjectProperty(mapping, jsonWriter, fromField, element);
					}
					else if (element.Value.ValueKind == JsonValueKind.Array)
					{
						fromField = (fromField?.Structure ?? fromStructure).Fields.SingleOrDefault(m => m.Name.EqualsIgnoreCase(element.Name));
						if (fromField == null)
							fromField = fromStructure.Fields.SingleOrDefault(m => m.Name.EqualsIgnoreCase(element.Name));
						if (fromField == null)
							throw new Exception();
						GetArrayProerties(mapping, jsonWriter, fromField, element);
					}
				}
			}
		}

		void GetObjectProperty(StructureMapping mapping, JsonWriter jsonWriter, Field fromField, JsonProperty element)
		{
			StructureMapping temp = GoOneLevelDeeper(mapping);
			ExtractValue(fromField.Structure, temp, JsonDocument.Parse(element.Value.GetRawText()),
				jsonWriter, fromField);
		}

		void GetSimpleProperty(JsonWriter jsonWriter, Field fromField, JsonProperty element, IEnumerable<FieldMapping> itemsStartsWithElementName)
		{
			foreach (var item in itemsStartsWithElementName)
			{
				jsonWriter.SetValue(item.ToField, element.Value.Cast(fromField));
			}
		}

		void GetArrayProerties(StructureMapping mapping, JsonWriter jsonWriter, Field fromField, JsonProperty element)
		{
			var temp = new StructureMapping
			{
				Mappings = mapping.Mappings.Where(m => m.FromField.StartsWithIgnoreCase(element.Name)).ToList()
			};
			foreach (var item in element.Value.EnumerateArray())
			{
				var tempJsonWriter = new JsonWriter();
				var doc = JsonDocument.Parse(item.GetRawText());
				ExtractValue(fromField.Structure, GoOneLevelDeeper(temp), doc, tempJsonWriter, fromField);
				var itemWithoutName = tempJsonWriter.GetRoot().SubItems[0];
				itemWithoutName.Name = "";
				jsonWriter.AddItemToArray(temp.Mappings.First().ToField.Split('.')[0], itemWithoutName);
			}
		}

		static StructureMapping GoOneLevelDeeper(StructureMapping mapping)
		{
			var temp = new StructureMapping();
			foreach (var item in mapping.Mappings)
			{
				var from = item.FromField.Split('.');
				if (from.Length > 1)
				{
					temp.Mappings.Add(new FieldMapping
					{
						ToField = item.ToField,
						FromField = string.Join(".", from.Skip(1))
					});
				}
			}

			return temp;
		}

		public async Task<string> Map(object input)
			=> await Map(JsonSerializer.Serialize(input));

		public async Task<T> Map<T>(string sourceJson)
			=> JsonSerializer.Deserialize<T>(await Map(sourceJson));

		public async Task<T> Map<T>(object input)
			=> await Map<T>(JsonSerializer.Serialize(input));
	}
}
