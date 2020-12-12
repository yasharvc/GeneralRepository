using Core.Enums;
using Core.Models.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.StructureDefintionTests
{
	public class StructureValidationTests
	{
		[Fact]
		public async void ValidateJsonStructure_WithSimpleString_ShouldValidate()
		{
			var structure = new StructureDefinition
			{
				Id = "test",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "test_name",
						Name = "name",
						DataType = DataTypeEnum.String,
						ParentPath = "test",
						Structure = null
					}
				}
			};

			Assert.True(await structure.ValidateJsonStructure(new { name = "Yashar" }));
		}
		[Fact]
		public async void ValidateJsonStructure_WithSimpleStringAndIntegerInput_ShouldNotValidate()
		{
			var structure = new StructureDefinition
			{
				Id = "test",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "test_name",
						Name = "name",
						DataType = DataTypeEnum.String,
						ParentPath = "test",
						Structure = null
					}
				}
			};

			Assert.False(await structure.ValidateJsonStructure(new { name = 34 }));
		}
		[Fact]
		public async void ValidateJsonStructure_WithSimpleInteger_ShouldValidate()
		{
			var structure = new StructureDefinition
			{
				Id = "test",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "test_age",
						Name = "age",
						DataType = DataTypeEnum.Integer,
						ParentPath = "test",
						Structure = null
					}
				}
			};

			Assert.True(await structure.ValidateJsonStructure(new { age = 34 }));
		}
	}
}
