using Core.Enums;
using System.Collections.Generic;

namespace Core.Models
{
	public class JsonProperty
	{
		public List<JsonSchemaTypeEnum> Type { get; set; }
		public string Format { get; set; }
		public List<JsonSchemaTypeEnum> OneOf { get; set; }
		public JsonArrayType Items { get; set; }
	}
}