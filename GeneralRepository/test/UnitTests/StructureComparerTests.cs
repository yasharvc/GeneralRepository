using Core.Enums;
using Core.Extensions;
using Core.Models.DataStructure;
using Core.Services;
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
	}
}
