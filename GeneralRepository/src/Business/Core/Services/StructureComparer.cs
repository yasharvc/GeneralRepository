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
					if (field.DataType == BasicDataTypeEnum.None)
						if (!CompareSubs(field, element.Value))
							return false;
						else
							continue;
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
				catch(Exception e)
				{
					var str = e.Message;
					return false;
				}
			}
			return true;
		}

		private bool CompareSubs(Field field, JsonValueKind elementValueKind)
		{
			switch (field.RelationType)
			{
				case RelationTypeEnum.OneToOne:return CompareObjectSub(field, elementValueKind);
				case RelationTypeEnum.OneToMany:return CompareArraySub(field, elementValueKind);
				default: throw new InvalidStructureException();
			}
		}

		private bool CompareArraySub(Field field, JsonValueKind elementValueKind)
			 => field.DataType == BasicDataTypeEnum.None && elementValueKind == JsonValueKind.Array;

		private bool CompareObjectSub(Field field, JsonValueKind elementValueKind) => field.DataType == BasicDataTypeEnum.None && elementValueKind == JsonValueKind.Object;

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
