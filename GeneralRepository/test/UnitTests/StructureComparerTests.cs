using Core.Enums;
using Core.Extensions;
using Core.Models.DataStructure;
using Core.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests
{
	public class StructureComparerTests
	{
		[Fact]
		public void Compare_WithOneStringField_ShouldReturnTrue()
		{
			var obj = new { name = "yashar" }.ToGeneralDictionary();
			var structure = new StructureDefinition
			{
				Id = "test",
				Name = "User",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "User_Name",
						FullName = "User.name",
						Name = "name",
						DataType = BasicDataTypeEnum.String,
						RelationType = RelationTypeEnum.NoRelation
					}
				}
			};
			var comparer = new StructureComparer();

			Assert.True(comparer.Compare(structure, obj));
		}

		[Fact]
		public void Compare_WithOneIntegerField_ShouldReturnTrue()
		{
			var obj = new { age = 34 }.ToGeneralDictionary();
			var structure = new StructureDefinition
			{
				Id = "test",
				Name = "User",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "User_Age",
						FullName = "User.age",
						Name = "age",
						DataType = BasicDataTypeEnum.Integer,
						RelationType = RelationTypeEnum.NoRelation
					}
				}
			};
			var comparer = new StructureComparer();

			Assert.True(comparer.Compare(structure, obj));
		}

		[Fact]
		public void Compare_WithOneGUIDField_ShouldReturnTrue()
		{
			var obj = new { guid = Guid.NewGuid().ToString() }.ToGeneralDictionary();
			var structure = new StructureDefinition
			{
				Id = "test",
				Name = "User",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "User_Guid",
						FullName = "User.guid",
						Name = "guid",
						DataType = BasicDataTypeEnum.GUID,
						RelationType = RelationTypeEnum.NoRelation
					}
				}
			};
			var comparer = new StructureComparer();

			Assert.True(comparer.Compare(structure, obj));
		}

		[Fact]
		public void Compare_WithOneISODateTimeField_ShouldReturnTrue()
		{
			var obj = new { date = "2011-10-05T14:48:00.000Z" }.ToGeneralDictionary();
			var structure = new StructureDefinition
			{
				Id = "test",
				Name = "User",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "User_date",
						FullName = "User.date",
						Name = "date",
						DataType = BasicDataTypeEnum.DateTime,
						RelationType = RelationTypeEnum.NoRelation
					}
				}
			};
			var comparer = new StructureComparer();

			Assert.True(comparer.Compare(structure, obj));
		}
		[Fact]
		public void Compare_WithDateTime_YYYYMMDD_FormatField_ShouldReturnTrue()
		{
			var obj = new { date = "2011/10/05" }.ToGeneralDictionary();
			var structure = new StructureDefinition
			{
				Id = "test",
				Name = "User",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "User_date",
						FullName = "User.date",
						Name = "date",
						DataType = BasicDataTypeEnum.DateTime,
						RelationType = RelationTypeEnum.NoRelation
					}
				}
			};
			var comparer = new StructureComparer();

			Assert.True(comparer.Compare(structure, obj));
		}
		[Fact]
		public void Compare_WithStringFieldAndIntegerStructure_ShouldReturnFlase()
		{
			var obj = new { name = "yashar" }.ToGeneralDictionary();
			var structure = new StructureDefinition
			{
				Id = "test",
				Name = "User",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "User_age",
						FullName = "User.age",
						Name = "age",
						DataType = BasicDataTypeEnum.Integer,
						RelationType = RelationTypeEnum.NoRelation
					}
				}
			};
			var comparer = new StructureComparer();

			Assert.False(comparer.Compare(structure, obj));
		}
		[Fact]
		public void Compare_WithStringAndIntegerFieldAndStringStructure_ShouldReturnFlase()
		{
			var obj = new { name = "yashar" , age= 34}.ToGeneralDictionary();
			var structure = new StructureDefinition
			{
				Id = "test",
				Name = "User",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "User_name",
						FullName = "User.name",
						Name = "name",
						DataType = BasicDataTypeEnum.String,
						RelationType = RelationTypeEnum.NoRelation
					},
					new Field
					{
						Id = "User_age",
						FullName = "User.age",
						Name = "age",
						DataType = BasicDataTypeEnum.String,
						RelationType = RelationTypeEnum.NoRelation
					}
				}
			};
			var comparer = new StructureComparer();

			Assert.False(comparer.Compare(structure, obj));
		}

		[Fact]
		public void Compare_WithObjectFieldInStructure_ShouldReturnTrue()
		{
			var objDictionary = new { address = new { city = "tabriz" } }.ToGeneralDictionary();
			var structure = new StructureDefinition
			{
				Id = "user",
				Name = "User",
				Fields = new List<Field>
				{
					new Field
					{
						DataType = BasicDataTypeEnum.None,
						FullName="User.address",
						Name = "address",
						RelationType = RelationTypeEnum.OneToOne,
						Id = "User_address"
					},
					new Field
					{
						DataType = BasicDataTypeEnum.String,
						FullName="User.address.city",
						Name = "address.city",
						RelationType = RelationTypeEnum.NoRelation,
						Id = "User_address_city"
					}
				}
			};

			Assert.True(new StructureComparer().Compare(structure, objDictionary));
		}
	}
}
