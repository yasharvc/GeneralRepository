using Core.Enums;
using Core.Models.DataStructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Json
{
	public class StructureDefinitionValidator
	{
		public static async Task<List<string>> Validate(StructureDefinition structure, string jsonInput)
		=> await Validate(structure, jsonInput, "");

		private static async Task<List<string>> Validate(StructureDefinition structure, string jsonInput,string parent = "")
		{
			var errors = new List<string>();
			var bytesOfJson = Encoding.UTF8.GetBytes(jsonInput);
			var memory = new MemoryStream(bytesOfJson);
			using (var document = await JsonDocument.ParseAsync(memory, new JsonDocumentOptions { AllowTrailingCommas = true }))
			{
				var root = document.RootElement;
				foreach (var field in structure.Fields)
					await ValidateFieldByValue(root, field, errors, parent);
			}
			return errors;

		}

		private static async Task ValidateFieldByValue(JsonElement root, Field field, List<string> errors, string parent = "")
		{
			parent += parent.EndsWith(".") ? "" : ".";
			try
			{
				var prop = root.GetProperty(field.Name);
				if (!IsValueKindSame(prop.ValueKind, field.DataType))
					errors.Add($"{parent}{field.Name} type is not equal with {prop.ValueKind}");
				if (field.IsDataTypeSimple())
				{
					if (!ValidateSimpleDataType(field.DataType, prop, errors))
						errors.Add($"{parent}{field.Name} type is not equal with {prop.ValueKind}");
					else
						return;
				}
				if (field.DataType == DataTypeEnum.Object)
					errors.AddRange(await Validate(field.Structure, prop.GetRawText(), $"{parent}{field.Name}"));
				else if (field.DataType == DataTypeEnum.Array)
				{
					var index = 0;
					foreach (var item in prop.EnumerateArray())
						errors.AddRange(await Validate(field.Structure, item.GetRawText(), $"{parent}{field.Name}[{index++}]"));
				}
			}
			catch (KeyNotFoundException)
			{
				if (!field.Nullable)
					errors.Add($"{parent}{field.Name} type is not nullable but is not exists in data");
			}
		}

		private static bool ValidateSimpleDataType(DataTypeEnum dataType, JsonElement prop, List<string> errors)
		{
			if (dataType == DataTypeEnum.Time)
				return CanConvertToTime(prop.GetString());
			else if (dataType == DataTypeEnum.DateTime || dataType == DataTypeEnum.Date)
				return CanConvertToDateTime(prop.GetString());
			else if (dataType == DataTypeEnum.Binary)
				return CanConvertToBase64(prop.GetString());
			else if (dataType == DataTypeEnum.GUID)
				return CanConvertToGuid(prop.GetString());
			else if (dataType == DataTypeEnum.Booelan)
				return CanConvertToBoolean(prop.GetString());
			else if (dataType == DataTypeEnum.String)
				return true;
			else if (dataType == DataTypeEnum.Float)
			{
				try
				{
					prop.GetDouble();
					return true;
				}
				catch
				{
					return false;
				}
			}
			else if (dataType == DataTypeEnum.Integer)
			{
				try
				{
					prop.GetInt64();
					return true;
				}
				catch
				{
					return false;
				}
			}
			return false;
		}

		private static bool CanConvertToBoolean(string value)
		{
			try
			{
				Convert.ToBoolean(value);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static bool CanConvertToGuid(string value)
		{
			try
			{
				Guid.Parse(value);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static bool CanConvertToBase64(string value)
		{
			try
			{
				Convert.FromBase64String(value);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static bool CanConvertToDateTime(string value)
		{
			try
			{
				Convert.ToDateTime(value);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static bool CanConvertToTime(string value)
		{
			try
			{
				TimeSpan.Parse(value);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static bool IsValueKindSame(JsonValueKind valueKind, DataTypeEnum dataType) => 
			(valueKind == JsonValueKind.Array && dataType == DataTypeEnum.Array) ||
				((valueKind == JsonValueKind.False || valueKind == JsonValueKind.True) && dataType == DataTypeEnum.Booelan) ||
				(valueKind == JsonValueKind.Number && (dataType == DataTypeEnum.Float || dataType == DataTypeEnum.Integer)) ||
				(valueKind == JsonValueKind.Object && dataType == DataTypeEnum.Object) ||
				(valueKind == JsonValueKind.String && (
				dataType == DataTypeEnum.String ||
				dataType == DataTypeEnum.Time ||
				dataType == DataTypeEnum.GUID ||
				dataType == DataTypeEnum.Binary ||
				dataType == DataTypeEnum.Date ||
				dataType == DataTypeEnum.DateTime ||
				dataType == DataTypeEnum.GUID ||
				dataType == DataTypeEnum.Time));
	}
}