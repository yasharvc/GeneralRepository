using Core.Enums;
using Core.Models.DataStructure;
using Newtonsoft.Json;
using NJsonSchema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.JsonTests
{
	public class StructureJsonSchemaTests
	{
		[Fact]
		public async void JsonSchemaValidate_WithString_ShouldGenerateCorrectSchema()
		{
			var structure = new StructureDefinition
			{
				Fields = new List<Field>
				{
					 new Field
					 {
						 Id = "user_name",
						 DataType = BasicDataTypeEnum.String,
						 FullName = "user.name",
						 Name = "name",
						 RelationType = RelationTypeEnum.NoRelation,
						 Validators = null
					 }
				},
				Name = "user",
				Id = "uu"
			};
			var json = structure.ToSampleJson();
			var res = await JsonSchema.FromJsonAsync(json);
			Assert.Empty(res.Validate(JsonConvert.SerializeObject(new { name = "yy"})));
		}

		[Fact]
		public async void JsonSchemaValidate_WithStructureWithStringAndInteger_ShouldValidate()
		{
			var structure = new StructureDefinition
			{
				Fields = new List<Field>
				{
					 new Field
					 {
						 Id = "user_name",
						 DataType = BasicDataTypeEnum.String,
						 FullName = "user.name",
						 Name = "name",
						 RelationType = RelationTypeEnum.NoRelation,
						 Validators = null
					 },
					 new Field
					 {
						 Id = "user_age",
						 DataType = BasicDataTypeEnum.Integer,
						 FullName = "user.age",
						 Name = "age",
						 RelationType = RelationTypeEnum.NoRelation,
						 Validators = null
					 }
				},
				Name = "user",
				Id = "uu"
			};
			var json = structure.ToSampleJson();
			var res = await JsonSchema.FromJsonAsync(json);
			Assert.Empty(res.Validate(JsonConvert.SerializeObject(new { name = "yy", age = 16 })));
		}
		[Fact]
		public async void JsonSchemaValidate_WithStructureObjectInside_ShouldValidate()
		{
			var structure = new StructureDefinition
			{
				Fields = new List<Field>
				{
					 new Field
					 {
						 Id = "user_name",
						 DataType = BasicDataTypeEnum.None,
						 FullName = "user.address",
						 Name = "address",
						 RelationType = RelationTypeEnum.OneToOne,
						 Validators = null
					 },
					 new Field
					 {
						 Id = "user_address_city",
						 DataType = BasicDataTypeEnum.String,
						 FullName = "user.address.city",
						 Name = "city",
						 RelationType = RelationTypeEnum.NoRelation,
						 Validators = null
					 },
					 new Field
					 {
						 Id = "user_address_country",
						 DataType = BasicDataTypeEnum.String,
						 FullName = "user.address.country",
						 Name = "country",
						 RelationType = RelationTypeEnum.NoRelation,
						 Validators = null
					 }
				},
				Name = "user",
				Id = "uu"
			};
			var json = structure.ToSampleJson();
			var res = await JsonSchema.FromJsonAsync(json);
			Assert.Empty(res.Validate(JsonConvert.SerializeObject(new { Address = new { Country = "Iran", city = "Tabriz" } })));
		}
	}
}
