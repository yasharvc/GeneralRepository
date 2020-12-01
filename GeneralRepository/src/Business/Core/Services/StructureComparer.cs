using Core.Enums;
using Core.Exceptions.Application;
using Core.Extensions;
using Core.Interfaces;
using Core.Models;
using Core.Models.DataStructure;
using System;
using System.Linq;
using System.Text.Json;

namespace Core.Services
{
	public class StructureComparer : IStructureComparer
	{
		public bool Compare(StructureDefinition structure, JsonTranslation jsonTranslation)
		{

			foreach (var element in jsonTranslation.ElementValueKind)
			{
				Field field = new Field();
				try
				{
					field = structure.Fields.Single(m => m.Name == element.Key);
					if (!IsFieldEqual(field.DataType, element.Value))
						return false;
				}
				catch (InvalidComparisonException)
				{
					string value = jsonTranslation.Elements[element.Key].ToString();
					if ((field.DataType == BasicDataTypeEnum.DateTime || field.DataType == BasicDataTypeEnum.Date)
						&& element.Value == JsonValueKind.String
						&& !IsDateTime(value))
						return false;
					if (field.DataType == BasicDataTypeEnum.Time
						&& element.Value == JsonValueKind.String
						&& !IsTime(value))
						return false;
					if (field.DataType == BasicDataTypeEnum.Binary
						&& element.Value == JsonValueKind.String
						&& !IsBase64(value))
						return false;
					if (field.DataType == BasicDataTypeEnum.GUID
						&& element.Value == JsonValueKind.String
						&& !IsGuid(value))
						return false;
				}
				catch
				{
					return false;
				}
			}
			return true;
		}

		private static bool IsGuid(string value)
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

		private static bool IsBase64(string value)
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

		private static bool IsTime(string value)
		{
			try
			{
				TimeSpan.Parse(value);
				return true;
			}
			catch (FormatException)
			{
				return false;
			}
			catch (OverflowException)
			{
				return false;
			}
		}

		private static bool IsDateTime(string value)
		{
			try
			{
				Convert.ToDateTime(value);
				return true;
			}
			catch
			{
				try
				{
					DateTime.Parse(value);
					return true;
				}
				catch
				{
					try
					{
						DateTime.Parse(value, null, System.Globalization.DateTimeStyles.RoundtripKind);
						return true;
					}
					catch
					{
						return false;
					}
				}
			}
		}

		private static bool IsFieldEqual(BasicDataTypeEnum dataType, JsonValueKind value) => dataType.AreEqual(value);
	}
}
