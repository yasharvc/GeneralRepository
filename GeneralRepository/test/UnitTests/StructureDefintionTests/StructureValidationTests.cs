using Core.Enums;
using Core.Exceptions.Application;
using Core.Extensions;
using Core.Models.DataStructure;
using System.Collections.Generic;
using System.Text.Json;
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
		public async void ValidateJsonStructure_WithObjectInStructureAndWrongInput_ShouldReturnFalse()
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
									Id = "test_country",
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

		[Fact]
		public async void ValidateJsonStructure_WithNotNullElementAndNotGivenValueInsideJson_ShouldReturnFalse()
		{
			var structure = new StructureDefinition
			{
				Id = "test",
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
						Id = "test_country",
						Name = "country",
						DataType = DataTypeEnum.String,
						Nullable = false
					}
				}
			};

			Assert.False(await structure.ValidateJsonStructure(new { city = "Tabriz" }));
		}
		[Fact]
		public async void ValidateJsonStructure_WithNotNullElementAndFullValueInsideJson_ShouldReturnTrue()
		{
			var structure = new StructureDefinition
			{
				Id = "test",
				Fields = new List<Field>
				{
					Field.NotNullString("test_city","city"),
					Field.NotNullString("test_country","country")
				}
			};

			Assert.True(await structure.ValidateJsonStructure(new { city = "Tabriz", country="Iran" }));
		}

		[Fact]
		public async void ValidateJsonStructure_WithTimeElementInStructureAndNotInsideJson_ShouldReturnFalse()
		{
			var structure = new StructureDefinition
			{
				Id = "test",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "test_time",
						Name = "time",
						DataType = DataTypeEnum.Time,
						Nullable = false
					}
				}
			};

			Assert.False(await structure.ValidateJsonStructure(new { time = "yashar" }));
		}
		[Fact]
		public async void ValidateJsonStructure_WithTimeElementInStructureAndInsideJson_ShouldReturnTrue()
		{
			var structure = new StructureDefinition
			{
				Id = "test",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "test_time",
						Name = "time",
						DataType = DataTypeEnum.Time,
						Nullable = false
					}
				}
			};

			Assert.True(await structure.ValidateJsonStructure(new { time = "12:30" }));
		}
		[Fact]
		public async void ValidateJsonStructure_WithBase64ElementInStructureAndInsideJson_ShouldReturnTrue()
		{
			var structure = new StructureDefinition
			{
				Id = "test",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "test_binary",
						Name = "binary",
						DataType = DataTypeEnum.Binary,
						Nullable = false
					}
				}
			};

			Assert.True(await structure.ValidateJsonStructure(new { binary = "123".ToBase64() }));
		}
		[Fact]
		public async void ValidateJsonStructure_WithBase64ElementInStructureAndWrongDataInsideJson_ShouldReturnFalse()
		{
			var structure = new StructureDefinition
			{
				Id = "test",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "test_binary",
						Name = "binary",
						DataType = DataTypeEnum.Binary,
						Nullable = false
					}
				}
			};

			Assert.False(await structure.ValidateJsonStructure(new { binary = "123" }));
		}
		//Time
		//DateTime
		//Array

		[Fact]
		public async void GetValue_WithNotNullableString_ShouldThrowException()
		{
			var structure = new StructureDefinition
			{
				Id = "test",
				Fields = new List<Field>
				{
					Field.NotNullString("name","name")
				}
			};

			await Assert.ThrowsAsync<InvalidJsonStringInputException>(async () => await structure.GetValue("name", JsonSerializer.Serialize(new { age = 34 })));
		}

		[Fact]
		public async void GetValue_WithNullableString_ShouldReturnNull()
		{
			var structure = new StructureDefinition
			{
				Id = "test",
				Fields = new List<Field>
				{
					Field.NullableString("name","name")
				}
			};

			Assert.Null(await structure.GetValue("name", JsonSerializer.Serialize(new { age = 34 })));
		}

		[Fact]
		public async void GetValue_WithObjectInsideStructureAndRightJson_ShouldReturnItemsValues()
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
								Field.NotNullString("test_city","city"),
								Field.NotNullString("test_country","country")
							}
						}
					}
				}
			};

			var city = await structure.GetValue("address.city",JsonSerializer.Serialize(new { address = new { city = "Tabriz", country = "Iran" } }));

			Assert.Equal("Tabriz", city);
		}
	}
}