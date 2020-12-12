using Core.Enums;
using System.Collections.Generic;

namespace Core.Models.DataStructure
{
	public class Field : Model
	{
		public DataTypeEnum DataType { get; set; }
		public string Name { get; set; }
		public string ParentPath { get; set; }
		public StructureDefinition Structure { get; set; }

		public string GetFullPath() => $"{ParentPath}.{Name}";
	}
}
