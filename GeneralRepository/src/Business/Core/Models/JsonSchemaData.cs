using Core.Enums;
using System.Collections.Generic;
using System.Text.Json;

namespace Core.Models
{
	public class JsonSchemaData
	{
		public string Schema { get; set; } = "http://json-schema.org/draft-04/schema#";
		public string Title { get; set; }
		public JsonSchemaTypeEnum Type { get; set; }
		public bool AdditionalProperties { get; set; }
		public Dictionary<string,JsonProperty> Properties { get; set; }
		public Dictionary<string,JsonSchemaData> Definitions { get; set; }

		public string ToJson() => 
			JsonSerializer.Serialize(this).Replace("\"schema\"", "\"$schema\"").Replace("\"ref\"", "\"$ref\"");
	}
}