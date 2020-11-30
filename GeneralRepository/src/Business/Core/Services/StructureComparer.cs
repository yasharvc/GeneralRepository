using Core.Enums;
using Core.Extensions;
using Core.Interfaces;
using Core.Models;
using Core.Models.DataStructure;
using System.Linq;
using System.Text.Json;

namespace Core.Services
{
	public class StructureComparer : IStructureComparer
	{
		public bool Compare(StructureDefinition structure, JsonTranslation jsonTranslation)
		{
			try
			{
				foreach (var element in jsonTranslation.ElementValueKind)
				{
					if (!IsFieldEqual(structure.Fields.Single(m => m.Name == element.Key).DataType, element.Value))
						return false;
				}
				return true;
			}catch
			{
				return false;
			}
		}

		private static bool IsFieldEqual(BasicDataTypeEnum dataType, JsonValueKind value) => dataType.AreEqual(value);
	}
}
