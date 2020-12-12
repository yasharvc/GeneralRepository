using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Models.DataStructure
{
	public class StructureDefinition : Model
	{
		public string Name { get; set; }
		public List<Field> Fields { get; set; }
		public List<Validator> Validators { get; set; }

		public string ToSampleJson() => $"{{\r\n\t{GetFieldsJson()}\r\n}}";

		private string GetFieldsJson()
		{
			var res = "";
			foreach (var field in Fields.Where(m => m.FullName.Split('.').Count() == 2))
			{
				res += $"{(res.Length > 0 ? ",\r\n\t" : "")}{GetFieldJson(field)}";
			}
			return res;
		}

		private string GetFieldJson(Field field)
		{
			switch (field.DataType)
			{
				case DataTypeEnum.Array:
				case DataTypeEnum.Object:
					return GetDefaultValueForComplexField(field);
				case DataTypeEnum.Booelan:
					return $"\"{field.Name}\":false";
				case DataTypeEnum.Integer:
				case DataTypeEnum.Float:
					return $"\"{field.Name}\":0";
				case DataTypeEnum.String:
					return $"\"{field.Name}\":\"\"";
				case DataTypeEnum.DateTime:
					return $"\"{field.Name}\":\"{DateTime.Now:O}\"";
				case DataTypeEnum.Date:
					return $"\"{field.Name}\":\"{DateTime.Now:yyyyy/MM/dd}\"";
				case DataTypeEnum.Time:
					return $"\"{field.Name}\":\"01:10\"";
				case DataTypeEnum.Binary:
					return $"\"{field.Name}\":\"iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5+hHgAHggJ/PchI7wAAAABJRU5ErkJggg==\"";
				case DataTypeEnum.GUID:
					return $"\"{field.Name}\":\"{Guid.NewGuid()}\"";
				default:
					return "";
			}
		}

		private string GetDefaultValueForComplexField(Field field)
		{
			var res = "";
			if (field.DataType == DataTypeEnum.Object)
			{
				var subFields = Fields.Where(m => m.FullName.StartsWith($"{field.FullName}."));
				foreach (var subField in subFields)
				{
					res += $"{(res.Length > 0 ? "," : "")}{GetFieldJson(subField)}";
				}
				return $"\"{field.Name}\":{{{res}}}";
			}
			throw new NotImplementedException();
		}
	}
}
