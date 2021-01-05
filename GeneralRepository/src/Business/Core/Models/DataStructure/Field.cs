using Core.Enums;

namespace Core.Models.DataStructure
{
	public class Field : Model
	{
		public DataTypeEnum DataType { get; set; }
		public string Name { get; set; }
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

		public static Field NotNullInteger(string id,string name)
		{
			return new Field
			{
				Id = id,
				Name = name,
				DataType = DataTypeEnum.Integer,
				Nullable = false
			};
		}
		public static Field NullableInteger(string id, string name)
		{
			return new Field
			{
				Id = id,
				Name = name,
				DataType = DataTypeEnum.Integer,
				Nullable = true
			};
		}
		public static Field NotNullDateTime(string id, string name)
		{
			return new Field
			{
				Id = id,
				Name = name,
				DataType = DataTypeEnum.DateTime,
				Nullable = false
			};
		}
		public static Field NullableDateTime(string id, string name)
		{
			return new Field
			{
				Id = id,
				Name = name,
				DataType = DataTypeEnum.Date,
				Nullable = true
			};
		}

		public static Field NotNullTime(string id, string name)
		{
			return new Field
			{
				Id = id,
				Name = name,
				DataType = DataTypeEnum.Time,
				Nullable = false
			};
		}
		public static Field NullableTime(string id, string name)
		{
			return new Field
			{
				Id = id,
				Name = name,
				DataType = DataTypeEnum.Time,
				Nullable = true
			};
		}
		public static Field NotNullBoolean(string id, string name)
		{
			return new Field
			{
				Id = id,
				Name = name,
				DataType = DataTypeEnum.Booelan,
				Nullable = false
			};
		}
		public static Field NullableBoolean(string id, string name)
		{
			return new Field
			{
				Id = id,
				Name = name,
				DataType = DataTypeEnum.Booelan,
				Nullable = true
			};
		}

		public static Field NotNullFloat(string id, string name)
		{
			return new Field
			{
				Id = id,
				Name = name,
				DataType = DataTypeEnum.Float,
				Nullable = false
			};
		}
		public static Field NullableFloat(string id, string name)
		{
			return new Field
			{
				Id = id,
				Name = name,
				DataType = DataTypeEnum.Float,
				Nullable = true
			};
		}

		public static Field NotNullGuid(string id, string name)
		{
			return new Field
			{
				Id = id,
				Name = name,
				DataType = DataTypeEnum.GUID,
				Nullable = false
			};
		}
		public static Field NullableGuid(string id, string name)
		{
			return new Field
			{
				Id = id,
				Name = name,
				DataType = DataTypeEnum.GUID,
				Nullable = true
			};
		}

		public static Field NotNullBinary(string id, string name)
		{
			return new Field
			{
				Id = id,
				Name = name,
				DataType = DataTypeEnum.Binary,
				Nullable = false
			};
		}
	}
}