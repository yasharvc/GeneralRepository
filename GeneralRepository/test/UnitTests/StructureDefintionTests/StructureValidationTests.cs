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
						Structure = null
					}
				}
			};

			Assert.True(await structure.ValidateJsonStructure(new { age = 34 }));
		}
		[Fact]
		public async void ValidateJsonStructure_WithNotNullableInteger_ShouldReturnFalse()
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
						Nullable = false,
						Structure = null
					}
				}
			};

			Assert.False(await structure.ValidateJsonStructure(new { name = "Yashar" }));
		}
		[Fact]
		public async void ValidateJsonStructure_WithNullableInteger_ShouldReturnTrue()
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
						Nullable = true,
						Structure = null
					}
				}
			};

			Assert.True(await structure.ValidateJsonStructure(new { name = "Yashar" }));
		}

		[Fact]
		public async void ValidateJsonStructure_WithNullableIntegerAndNotNullableString_ShouldReturnTrue()
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
						Nullable = true
					},
					new Field
					{
						Id = "test_name",
						Name = "name",
						DataType = DataTypeEnum.String,
						Nullable = false
					}
				}
			};

			Assert.True(await structure.ValidateJsonStructure(new { name = "Yashar" }));
		}
		[Fact]
		public async void ValidateJsonStructure_WithNullableIntegerAndNotNullableStringWithFullValue_ShouldReturnTrue()
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
						Nullable = true
					},
					new Field
					{
						Id = "test_name",
						Name = "name",
						DataType = DataTypeEnum.String,
						Nullable = false
					}
				}
			};

			Assert.True(await structure.ValidateJsonStructure(new { name = "Yashar", age=34 }));
		}
		[Fact]
		public async void ValidateJsonStructure_WithNullableIntegerAndNotNullableStringWithWrongValue_ShouldReturnFalse()
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
						Nullable = true
					},
					new Field
					{
						Id = "test_name",
						Name = "name",
						DataType = DataTypeEnum.String,
						Nullable = false
					}
				}
			};

			Assert.False(await structure.ValidateJsonStructure(new { name = "Yashar", age = "gfh" }));
		}
		[Fact]
		public async void ValidateJsonStructure_WithObjectInStructure_ShouldReturnTrue()
		{
			var structure = new StructureDefinition
			{
				Id = "test",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "test_address",
						Name = "address",
						DataType = DataTypeEnum.Object,
						Nullable = false,
						Structure=new StructureDefinition
						{
							Id = "address",
							Name = "address",
							Fields = new List<Field>
							{
								new Field
								{
									Id = "test_city",
									Name = "city",
									DataType = DataTypeEnum.String,
									Nullable = false
								},
								new Field
								{
									Id = "test_city",
									Name = "country",
									DataType = DataTypeEnum.String,
									Nullable = false
								}
							}
						}
					}
				}
			};

			Assert.False(await structure.ValidateJsonStructure(new { name = "Yashar", age = "gfh" }));
		}

		//Null checking
		//Not null fields that are not met
		//Binary => Base64
		//Time
		//DateTime
	}
}
