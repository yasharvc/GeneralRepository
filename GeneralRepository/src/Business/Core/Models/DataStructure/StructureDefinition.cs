using Core.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Models.DataStructure
{
	public class StructureDefinition : Model
	{
		public string Name { get; set; }
		public List<Field> Fields { get; set; }
		public List<Validator> Validators { get; set; }

		public StructureDefinition()
		{

		}

		public async Task<bool> ValidateJsonStructure(string input) => 
			(await StructureDefinitionValidator.Validate(this, input)).Count() == 0;

		public async Task<bool> ValidateJsonStructure(object input) =>
			await ValidateJsonStructure(JsonSerializer.Serialize(input));

		public async Task<object> GetValue(string fullPath, string sourceJson) => await StructureDefinitionValidator.GetValue(this, sourceJson, fullPath);

		public Field GetFieldByPath(string fromField)
		{
			Field res = null;
			var arr = fromField.Split('.');
			foreach (var item in arr)
			{
				if (res == null)
					res = Fields.Single(m => m.Name.Equals(item, StringComparison.OrdinalIgnoreCase));
				else
					res = res.Structure.Fields.Single(m => m.Name.Equals(item, StringComparison.OrdinalIgnoreCase));
			}
			return res;
		}

		public StructureDefinition(Type type)
		{
			GetStructureFromType(type);
		}

		private void GetStructureFromType(Type type)
		{
			var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				.Where(m => m.CanRead && m.CanWrite)
				.Where(m => m.GetGetMethod(true).IsPublic)
				.Where(m => m.GetSetMethod(true).IsPublic);
			Fields = new List<Field>();
			foreach (var prop in properties)
			{
				AddPropIntoStructure(type, prop, this);
			}
		}

		private void AddPropIntoStructure(Type type, PropertyInfo prop, StructureDefinition structureDefinition)
		{
			if (prop.PropertyType == typeof(string))
				structureDefinition.Fields.Add(Field.NotNullString($"{type.Name}_{prop.Name}", prop.Name));
			else if (prop.PropertyType == typeof(int) || prop.PropertyType == typeof(long))
				structureDefinition.Fields.Add(Field.NotNullInteger($"{type.Name}_{prop.Name}", prop.Name));
			else if (prop.PropertyType == typeof(DateTime))
				structureDefinition.Fields.Add(Field.NotNullDateTime($"{type.Name}_{prop.Name}", prop.Name));
			else if(prop.PropertyType == typeof(DateTime?))
				structureDefinition.Fields.Add(Field.NullableDateTime($"{type.Name}_{prop.Name}", prop.Name));
			else if (prop.PropertyType == typeof(TimeSpan))
				structureDefinition.Fields.Add(Field.NotNullTime($"{type.Name}_{prop.Name}", prop.Name));
			else if (prop.PropertyType == typeof(TimeSpan?))
				structureDefinition.Fields.Add(Field.NullableTime($"{type.Name}_{prop.Name}", prop.Name));
			else if (prop.PropertyType == typeof(bool))
				structureDefinition.Fields.Add(Field.NotNullBoolean($"{type.Name}_{prop.Name}", prop.Name));
			else if (prop.PropertyType == typeof(bool?))
				structureDefinition.Fields.Add(Field.NullableBoolean($"{type.Name}_{prop.Name}", prop.Name));
			else if (prop.PropertyType == typeof(double) || prop.PropertyType == typeof(float))
				structureDefinition.Fields.Add(Field.NotNullFloat($"{type.Name}_{prop.Name}", prop.Name));
			else if (prop.PropertyType == typeof(double?) || prop.PropertyType == typeof(float?))
				structureDefinition.Fields.Add(Field.NullableFloat($"{type.Name}_{prop.Name}", prop.Name));
			else if (prop.PropertyType == typeof(Guid))
				structureDefinition.Fields.Add(Field.NotNullGuid($"{type.Name}_{prop.Name}", prop.Name));
			else if (prop.PropertyType == typeof(Guid?))
				structureDefinition.Fields.Add(Field.NullableGuid($"{type.Name}_{prop.Name}", prop.Name));
			else if (prop.PropertyType == typeof(byte[]))
				structureDefinition.Fields.Add(Field.NotNullBinary($"{type.Name}_{prop.Name}", prop.Name));
		}
	}
}