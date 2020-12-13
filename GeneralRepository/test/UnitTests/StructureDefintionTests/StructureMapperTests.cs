using Core.Enums;
using Core.Models.DataStructure;
using Core.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			Assert.NotNull(res);
		}
	}
}
