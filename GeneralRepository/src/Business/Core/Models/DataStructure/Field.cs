using Core.Enums;

namespace Core.Models.DataStructure
{
	public class Field : Model
	{
		public DataTypeEnum DataType { get; set; }
		public string Name { get; set; }
		//public string ParentPath { get; set; }
		public StructureDefinition Structure { get; set; }
		public bool Nullable { get; set; } = true;

		public bool IsDataTypeSimple() => DataType != DataTypeEnum.Object && DataType != DataTypeEnum.Array;
	}
}
