using Core.Enums;
using Core.Extensions;
using Core.Models.DataStructure;
using Core.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			writer.SetValue("addr.city", "Tabriz");
			writer.SetValue("addr.country", "Iran");
			writer.SetValue("addr.zip", "+98");
			var json = writer.ToJson();
			var root = JsonDocument.Parse(json).RootElement;
			Assert.Equal("Tabriz", root.GetProperty("addr").GetProperty("city").GetString());
			Assert.Equal("Iran", root.GetProperty("addr").GetProperty("country").GetString());
			Assert.Equal("+98", root.GetProperty("addr").GetProperty("zip").GetString());
		}
		[Fact]
		public void JsonWriter_WithObjectCorrectingValue_ShouldCreateCorrectJson()
		{
			var writer = new Core.Services.JsonWriter();
			writer.SetValue("addr.city", "XYZ");
			writer.SetValue("addr.city", "Tabriz");
			var json = writer.ToJson();
			var root = JsonDocument.Parse(json).RootElement;
			Assert.Equal("Tabriz", root.GetProperty("addr").GetProperty("city").GetString());
		}

		[Fact]
		public void JsonWriter_WithNestedObject_ShouldCreateCorrectJson()
		{
			var writer = new Core.Services.JsonWriter();
			writer.SetValue("addr.area.city", "Tabriz");
			var json = writer.ToJson();
			var root = JsonDocument.Parse(json).RootElement;
			Assert.Equal("Tabriz", root.GetProperty("addr").GetProperty("area").GetProperty("city").GetString());
		}
		[Fact]
		public void JsonWriter_WithArray_ShouldCreateCorrectJson()
		{
			var writer = new Core.Services.JsonWriter();
			writer.AddItemToArray("items", new Core.Models.JsonValue { Name = "name", Value="\"Item1\""});
			writer.AddItemToArray("items", new Core.Models.JsonValue { Name = "name", Value="\"Item2\""});
			var json = writer.ToJson();
			var root = JsonDocument.Parse(json).RootElement;
			Assert.Equal(2, root.GetProperty("items").GetArrayLength());
			Assert.Equal("Item1", root.GetProperty("items").EnumerateArray().ToList().First().GetProperty("name").GetString());
			Assert.Equal("Item2", root.GetProperty("items").EnumerateArray().ToList()[1].GetProperty("name").GetString());
		}

		[Fact]
		public async void FromJsonToStructureTest()
		{
			var json = System.Text.Json.JsonSerializer.Serialize(new
			{
				name = "Yashar",
				age = 34,
				address = new
				{
					country = "Iran",
					city = "Tabriz"
				},
				items = new[] { new { name = "First", count = 3 }, new { name = "Second", count = 1 } }
			});

			var fromStructure = new StructureDefinition
			{
				Id = "test",
				Fields = new List<Field>
				{
					Field.NotNullString("name","name"),
					Field.NotNullInteger("age","age"),
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
								Field.NullableString("test_address_city","city"),
								Field.NullableString("test_address_country","country")
							}
						}
					},
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
					Field.NotNullString("test_dest","name"),
					Field.NotNullString("test_dest","ad"),
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
								Field.NullableString("test_address_city","city")
							}
						}
					},
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

			var mapping = new StructureMapping
			{
				Mappings = new List<FieldMapping>
				{
					new FieldMapping
					{
						FromField = "name",
						ToField = "ad"
					},
					new FieldMapping
					{
						FromField = "address.city",
						ToField = "addr.city"
					},
					new FieldMapping
					{
						FromField = "name",
						ToField = "id"
					},
					new FieldMapping
					{
						FromField = "items.name",
						ToField = "dest.itemName"
					},
					new FieldMapping
					{
						FromField = "items.count",
						ToField = "dest.itemCount"
					}
				}
			};

			using var doc = JsonDocument.Parse(json);
			var jsonWriter = new Core.Services.JsonWriter();
			Field fromField = null;
			fromField = ExtractValue(fromStructure, mapping, doc, jsonWriter, fromField);
			var str = jsonWriter.ToJson();
			Assert.NotNull(str);

			static Field ExtractValue(StructureDefinition fromStructure, StructureMapping mapping, JsonDocument doc, Core.Services.JsonWriter jsonWriter, Field fromField)
			{
				foreach (var element in doc.RootElement.EnumerateObject())
				{
					if (mapping.Mappings.Any(m => m.FromField.StartsWithIgnoreCase(element.Name)))
					{
						var itemsStartsWithElementName = mapping.Mappings.Where(m => m.FromField.StartsWithIgnoreCase(element.Name));
						if (element.Value.IsSimpleType())
						{
							fromField = (fromField?.Structure ?? fromStructure).Fields.SingleOrDefault(m => m.Name.EqualsIgnoreCase(element.Name));
							if (fromField == null)
								throw new Exception();
							GetSimpleProperty(jsonWriter, fromField, element, itemsStartsWithElementName);
						}
						else if(element.Value.ValueKind == JsonValueKind.Object)
						{
							fromField = (fromField?.Structure ?? fromStructure).Fields.SingleOrDefault(m => m.Name.EqualsIgnoreCase(element.Name));
							if (fromField == null)
								throw new Exception();
							GetObjectProperty(mapping, jsonWriter, fromField, element);
						}
						else if(element.Value.ValueKind == JsonValueKind.Array)
						{
							fromField = (fromField?.Structure ?? fromStructure).Fields.SingleOrDefault(m => m.Name.EqualsIgnoreCase(element.Name));
							if (fromField == null)
								fromField = fromStructure.Fields.SingleOrDefault(m => m.Name.EqualsIgnoreCase(element.Name));
							if (fromField == null)
								throw new Exception();
							GetArrayProerties(mapping, jsonWriter, fromField, element);
						}
					}
				}

				return fromField;

				static void GetObjectProperty(StructureMapping mapping, Core.Services.JsonWriter jsonWriter, Field fromField, JsonProperty element)
				{
					StructureMapping temp = GoOneLevelDeeper(mapping);
					ExtractValue(fromField.Structure, temp, JsonDocument.Parse(element.Value.GetRawText()),
						jsonWriter, fromField);
				}

				static void GetSimpleProperty(Core.Services.JsonWriter jsonWriter, Field fromField, JsonProperty element, IEnumerable<FieldMapping> itemsStartsWithElementName)
				{
					foreach (var item in itemsStartsWithElementName)
					{
						jsonWriter.SetValue(item.ToField, element.Value.Cast(fromField));
					}
				}

				static void GetArrayProerties(StructureMapping mapping, Core.Services.JsonWriter jsonWriter, Field fromField, JsonProperty element)
				{
					var temp = new StructureMapping
					{
						Mappings = mapping.Mappings.Where(m => m.FromField.StartsWithIgnoreCase(element.Name)).ToList()
					};
					foreach (var item in element.Value.EnumerateArray())
					{
						var tempJsonWriter = new Core.Services.JsonWriter();
						var doc = JsonDocument.Parse(item.GetRawText());
						ExtractValue(fromField.Structure, GoOneLevelDeeper(temp), doc, tempJsonWriter, fromField);
						var xxx = tempJsonWriter.GetRoot().SubItems[0];
						xxx.Name = "";
						jsonWriter.AddItemToArray(temp.Mappings.First().ToField.Split('.')[0]
							, xxx);
					}
				}

				static StructureMapping GoOneLevelDeeper(StructureMapping mapping)
				{
					var temp = new StructureMapping();
					foreach (var item in mapping.Mappings)
					{
						var from = item.FromField.Split('.');
						if (from.Length > 1)
						{
							temp.Mappings.Add(new FieldMapping
							{
								ToField = item.ToField,
								FromField = string.Join(".", from.Skip(1))
							});
						}
					}

					return temp;
				}
			}
		}

		
	}
}
