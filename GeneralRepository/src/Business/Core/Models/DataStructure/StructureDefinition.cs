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
				case BasicDataTypeEnum.None:
					return GetDefaultValueForComplexField(field);
				case BasicDataTypeEnum.Booelan:
					return $"\"{field.Name}\":false";
				case BasicDataTypeEnum.Integer:
				case BasicDataTypeEnum.Float:
					return $"\"{field.Name}\":0";
				case BasicDataTypeEnum.String:
					return $"\"{field.Name}\":\"\"";
				case BasicDataTypeEnum.DateTime:
					return $"\"{field.Name}\":\"{DateTime.Now:O}\"";
				case BasicDataTypeEnum.Date:
					return $"\"{field.Name}\":\"{DateTime.Now:yyyyy/MM/dd}\"";
				case BasicDataTypeEnum.Time:
					return $"\"{field.Name}\":\"01:10\"";
				case BasicDataTypeEnum.Binary:
					return $"\"{field.Name}\":\"iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5+hHgAHggJ/PchI7wAAAABJRU5ErkJggg==\"";
				case BasicDataTypeEnum.GUID:
					return $"\"{field.Name}\":\"{Guid.NewGuid()}\"";
				default:
					return "";
			}
		}

		private string GetDefaultValueForComplexField(Field field)
		{
			var res = "";
			if (field.RelationType == RelationTypeEnum.NoRelation)
				throw new ArgumentException();
			if (field.RelationType == RelationTypeEnum.OneToOne)
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
