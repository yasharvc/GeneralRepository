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

		public static Field NotNullString(string id, string name)
		{
			return new Field
			{
				Id = id,
				Name = name,
				DataType = DataTypeEnum.String,
				Nullable = false
			};
		}

		public static Field NullableString(string id, string name)
		{
			return new Field
			{
				Id = id,
				Name = name,
				DataType = DataTypeEnum.String,
				Nullable = true
			};
		}
	}
}