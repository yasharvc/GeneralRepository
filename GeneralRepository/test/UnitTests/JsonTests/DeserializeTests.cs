using Core.Models;
using NJsonSchema;
using NJsonSchema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.JsonTests
{
	public class DeserializeTests
	{
		class a
		{
			public int Count { get; set; }
			public string ItemName { get; set; }
		}

		class c
		{
			public string Name { get; set; }
		}

		class b
		{
			public int id { get; set; }
			public a item { get; set; }
			public List<c> Ces { get; set; }
			public List<string> Strings { get; set; }
		}


		[Fact]
		public async Task Test1()
		{
			var res = JsonSchema.FromType<string>( new JsonSchemaGeneratorSettings { 
				FlattenInheritanceHierarchy=true,
			});
			var jsonSchema = JsonSerializer.Deserialize<JsonSchemaData>(res.ToJson().Replace("\"$", "\""), new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = false
			});

			Assert.NotNull(res.ToJson());
		}
	}
}
