using Core.Enums;
using Core.Models.DataStructure;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
	public class StructureMapper
	{
		public StructureDefinition SourceStructure { get; set; }
		public StructureDefinition DestinationStructure { get; set; }
		public StructureMapping Mapping { get; set; }
		private string SourceSampleJson { get => SourceStructure?.ToSampleJson() ?? "{}"; }
		private string DestinationSampleJson
		{
			get => DestinationStructure?.ToSampleJson() ?? "{}";
		}

		public async Task<string> Map(string sourceJson)
		{
			var sourceJsonValidator = await JsonSchema.FromJsonAsync(SourceSampleJson);
			var destinationJsonValidator = await JsonSchema.FromJsonAsync(DestinationSampleJson);
			if(sourceJsonValidator.Validate(sourceJson).Count == 0)
			{
				var res = "";
				foreach (var mapping in Mapping.Mappings)
				{
					var fieldValue = GetFieldValue(mapping.FromField, sourceJson);
					var temp = $@"""{DestinationStructure.Fields.Single(m => m.FullName == mapping.ToField).Name}"":{fieldValue}";
					res += (res.Length > 0 ? "," : "") + temp;
				}
				res = $"{{{res}}}";
				if (destinationJsonValidator.Validate(res).Count == 0)
					return res;
			}
			throw new ArgumentException(nameof(sourceJson));
		}

		private string GetFieldValue(string fromField, string sourceJson)
		{
			var jObject = JObject.Parse(sourceJson);
			var field = SourceStructure.Fields.Single(m => m.FullName == fromField);
			var parentName = fromField.Substring(0, fromField.IndexOf('.'));
			fromField = fromField.Substring(fromField.IndexOf('.') + 1);
			var subItems = fromField.Split('.');
			var path = parentName;

			JToken token = null;
			foreach (var subItem in subItems)
			{
				token = token == null ? jObject[subItem] : token[subItem];
			}
			if(field.DataType != DataTypeEnum.Object && field.DataType != DataTypeEnum.Array)
			{
				if(field.DataType == DataTypeEnum.Booelan || field.DataType == DataTypeEnum.Float
					|| field.DataType == DataTypeEnum.Integer)
					return token.ToString();
				else
					return $@"""{token}""";
			}
			throw new NotImplementedException();
		}
	}
}
