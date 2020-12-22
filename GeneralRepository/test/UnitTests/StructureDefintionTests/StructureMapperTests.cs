using Core.Enums;
using Core.Models.DataStructure;
using Core.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
									Id = "test_address_city", DataType = DataTypeEnum.String, Name = "city",
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
									Id = "test_address_city",
									DataType= DataTypeEnum.String,
									Name = "city",
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
									Id = "test_dest_city",
									DataType= DataTypeEnum.String,
									Name = "city",
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
						Id = "test_items",
						Name = "items",
						DataType = DataTypeEnum.Array,
						Nullable = false,
						Structure = new StructureDefinition
						{
							Fields = new List<Field>
							{
								new Field
								{
									Id = "test_items_name",
									DataType= DataTypeEnum.String,
									Name = "name",
									Nullable=true
								},
								new Field
								{
									Id = "test_items_count",
									DataType= DataTypeEnum.Integer,
									Name = "count",
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
						DataType = DataTypeEnum.Array,
						Nullable = false,
						Structure = new StructureDefinition
						{
							Fields = new List<Field>
							{
								new Field
								{
									Id = "test_dest_itemname",
									DataType = DataTypeEnum.String,
									Name = "itemName",
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
							FromField = "items.name",
							ToField = "dest.itemName"
						}
					}
				}
			};

			var res = await mapper.Map(JsonConvert.SerializeObject(new { items = new[] { new { name = "First",count=3 }, new { name = "Second", count = 1 } } }));

			var root = JsonDocument.Parse(res).RootElement;

			Assert.Equal(2, root.GetProperty("dest").GetArrayLength());
		}
		[Fact]
		public void DictionaryToJsonTest()
		{
			var dict = new Dictionary<string, object>();
			dict["aa"] =  new { city = "SDF" };
			dict["add"] = new object[] { new { name = "a" }, new { name = "b" } } ;
			var str = System.Text.Json.JsonSerializer.Serialize(dict);
			Assert.NotNull(str);
		}
		[Fact]
		public void JsonWriter_WithSimpleString_ShouldCreateCorrectJson()
		{
			var writer = new Core.Services.JsonWriter();
			writer.SetValue("name", "Yashar");
			var json = writer.ToJson();
			var root = JsonDocument.Parse(json).RootElement;
			Assert.Equal("Yashar", root.GetProperty("name").GetString());
		}
		[Fact]
		public void JsonWriter_WithSimpleInteger_ShouldCreateCorrectJson()
		{
			var writer = new Core.Services.JsonWriter();
			writer.SetValue("age", 34);
			var json = writer.ToJson();
			var root = JsonDocument.Parse(json).RootElement;
			Assert.Equal(34, root.GetProperty("age").GetInt32());
		}
		[Fact]
		public void JsonWriter_WithObject_ShouldCreateCorrectJson()
		{
			var writer = new Core.Services.JsonWriter();
			writer.SetObjectValue("addr", new { city = "Tabriz" });
			var json = writer.ToJson();
			var root = JsonDocument.Parse(json).RootElement;
			Assert.Equal("Tabriz", root.GetProperty("addr").GetProperty("city").GetString());
		}
		[Fact]
		public void JsonWriter_WithObjectCorrectingValue_ShouldCreateCorrectJson()
		{
			var writer = new Core.Services.JsonWriter();
			writer.SetObjectValue("addr", new { city = "XYZ" });
			writer.SetObjectValue("addr", new { city = "Tabriz" });
			var json = writer.ToJson();
			var root = JsonDocument.Parse(json).RootElement;
			Assert.Equal("Tabriz", root.GetProperty("addr").GetProperty("city").GetString());
		}

		[Fact]
		public void JsonWriter_WithNestedObject_ShouldCreateCorrectJson()
		{
			var writer = new Core.Services.JsonWriter();
			writer.SetObjectValue("addr", new { area = new { city = "Tabriz" } });
			var json = writer.ToJson();
			var root = JsonDocument.Parse(json).RootElement;
			Assert.Equal("Tabriz", root.GetProperty("addr").GetProperty("area").GetProperty("city").GetString());
		}
	}
}
