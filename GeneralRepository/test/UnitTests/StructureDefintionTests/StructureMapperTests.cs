using Core.Enums;
using Core.Models.DataStructure;
using Core.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json;
using Xunit;

namespace UnitTests.StructureDefintionTests
{
	public class StructureMapperTests
	{
		[Fact]
		public async void Map_WithSimpleStringButDifferentName_ShouldMap()
		{
			var fromStructure = new StructureDefinition
			{
				Id = "test",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "test_name",
						Name = "name",
						DataType = DataTypeEnum.String,
						Nullable = false
					}
				}
			};
			var toStructure = new StructureDefinition
			{
				Id = "testTo",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "test_dest",
						Name = "dest",
						DataType = DataTypeEnum.String,
						Nullable = false
					}
				}
			};

			var mapper = new StructureMapper
			{
				DestinationStructure = toStructure,
				SourceStructure = fromStructure,
				Mapping = new StructureMapping { 
					Mappings = new List<FieldMapping> {
						new FieldMapping
						{
							FromField = "name",
							ToField = "dest"
						}
					}
				}
			};

			var res = await mapper.Map(JsonConvert.SerializeObject(new { name = "Test" }));

			var root = JsonDocument.Parse(res).RootElement;

			Assert.Equal("Test", root.GetProperty("dest").GetString());
		}
		[Fact]
		public async void Map_WithObjectInside_ShouldMapToSimpleItem()
		{
			var fromStructure = new StructureDefinition
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
						Structure = new StructureDefinition
						{
							Fields = new List<Field>
							{
								new Field
								{
									Id = "city",
									DataType= DataTypeEnum.String,
									Name = "test_address_city",
									Nullable=true
								}
							}
						}
					}
				}
			};
			var toStructure = new StructureDefinition
			{
				Id = "testTo",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "test_dest",
						Name = "dest",
						DataType = DataTypeEnum.String,
						Nullable = false
					}
				}
			};

			var mapper = new StructureMapper
			{
				DestinationStructure = toStructure,
				SourceStructure = fromStructure,
				Mapping = new StructureMapping
				{
					Mappings = new List<FieldMapping> {
						new FieldMapping
						{
							FromField = "address.city",
							ToField = "dest"
						}
					}
				}
			};

			var res = await mapper.Map(JsonConvert.SerializeObject(new { address = new { city = "Tabriz" } }));

			var root = JsonDocument.Parse(res).RootElement;

			Assert.Equal("Tabriz", root.GetProperty("dest").GetString());
		}

		[Fact]
		public async void Map_WithObjectInside_ShouldMapToObjectWithDifferentName()
		{
			var fromStructure = new StructureDefinition
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
						Structure = new StructureDefinition
						{
							Fields = new List<Field>
							{
								new Field
								{
									Id = "city",
									DataType= DataTypeEnum.String,
									Name = "test_address_city",
									Nullable=true
								}
							}
						}
					}
				}
			};
			var toStructure = new StructureDefinition
			{
				Id = "testTo",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "test_dest",
						Name = "dest",
						DataType = DataTypeEnum.Object,
						Nullable = false,
						Structure = new StructureDefinition
						{
							Fields = new List<Field>
							{
								new Field
								{
									Id = "city",
									DataType= DataTypeEnum.String,
									Name = "test_dest_city",
									Nullable=true
								}
							}
						}
					}
				}
			};

			var mapper = new StructureMapper
			{
				DestinationStructure = toStructure,
				SourceStructure = fromStructure,
				Mapping = new StructureMapping
				{
					Mappings = new List<FieldMapping> {
						new FieldMapping
						{
							FromField = "address.city",
							ToField = "dest.city"
						}
					}
				}
			};

			var res = await mapper.Map(JsonConvert.SerializeObject(new { address = new { city = "Tabriz" } }));

			var root = JsonDocument.Parse(res).RootElement;

			Assert.Equal("Tabriz", root.GetProperty("dest").GetProperty("city").GetString());
		}

		[Fact]
		public async void Map_WithArrayInside_ShouldMapToArrayWithDifferentName()
		{
			var fromStructure = new StructureDefinition
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
						Structure = new StructureDefinition
						{
							Fields = new List<Field>
							{
								new Field
								{
									Id = "city",
									DataType= DataTypeEnum.String,
									Name = "test_address_city",
									Nullable=true
								}
							}
						}
					}
				}
			};
			var toStructure = new StructureDefinition
			{
				Id = "testTo",
				Fields = new List<Field>
				{
					new Field
					{
						Id = "test_dest",
						Name = "dest",
						DataType = DataTypeEnum.Object,
						Nullable = false,
						Structure = new StructureDefinition
						{
							Fields = new List<Field>
							{
								new Field
								{
									Id = "city",
									DataType= DataTypeEnum.String,
									Name = "test_dest_city",
									Nullable=true
								}
							}
						}
					}
				}
			};

			var mapper = new StructureMapper
			{
				DestinationStructure = toStructure,
				SourceStructure = fromStructure,
				Mapping = new StructureMapping
				{
					Mappings = new List<FieldMapping> {
						new FieldMapping
						{
							FromField = "address.city",
							ToField = "dest.city"
						}
					}
				}
			};

			var res = await mapper.Map(JsonConvert.SerializeObject(new { address = new { city = "Tabriz" } }));

			var root = JsonDocument.Parse(res).RootElement;

			Assert.Equal("Tabriz", root.GetProperty("dest").GetProperty("city").GetString());
		}
		[Fact]
		public void DictionaryToJsonTest()
		{
			var dict = new Dictionary<string, object>
			{
				{ "address",new Dictionary<string,object>{
					{"city","tabriz" },
					{ "country","Iran"}
				} }
			};
			var str = JsonConvert.SerializeObject(dict);
			Assert.NotNull(str);
		}
	}
}
