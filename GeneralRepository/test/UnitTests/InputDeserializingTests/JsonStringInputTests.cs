﻿using Core.Exceptions.Application;
using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace UnitTests.InputDeserializingTests
{
	public class JsonStringInputTests
	{
		readonly object TestObject = new {
			name = "name",
			age = 24,
			address = new { city = "Tabriz", country = "Iran" },
			items = new object[] {
				new { id = 1, title = "Stuff 1" },
				new { id = 2, title = "Stuff 2" }
			}
		};

		class TestClass
		{
			public string Name { get; set; }
			public int Age { get; set; }
			public DateTime? DOB { get; set; }
		}

		readonly string TestObjectJson = "";
		public JsonStringInputTests()
		{
			TestObjectJson = JsonSerializer.Serialize(TestObject);
		}
		[Fact]
		public void ToDictionary_WithInvalidJson_ShouldThrowInvalidInputException()
		{
			var input = TestObjectJson[2..];

			Assert.Throws<InvalidJsonStringInputException>(() => input.ToGeneralDictionary());
		}

		[Fact]
		public void ToDictionary_WithValidJson_ShouldReturnValidDictionary()
		{
			var input = TestObjectJson;
			
			var obj = input.ToGeneralDictionary().Elements;

			Assert.True(obj.ContainsKey("name"));
			Assert.True(obj.ContainsKey("age"));
			Assert.True(obj.ContainsKey("address"));
			Assert.True(obj.ContainsKey("items"));

			Assert.Equal(2, (obj["items"] as IEnumerable<object>).Count());
		}

		[Fact]
		public void ToDictionary_WithNestedObject_ShouldReturnValidDictionary()
		{
			var input = JsonSerializer.Serialize(new { name = "name", innerObject = new { size=34 } });

			var obj = input.ToGeneralDictionary().Elements;

			Assert.True(obj.ContainsKey("name"));
			Assert.True(obj.ContainsKey("innerObject"));
			Assert.Equal("name",obj["name"]);
			Assert.Equal(34D, (obj["innerObject"] as IDictionary<string, object>)["size"]);
		}

		[Fact]
		public void ToDictionary_WithTestClassObject_ShouldReturnValidDictionary()
		{
			var obj = new TestClass
			{
				Name = "name",
				Age = 34,
				DOB = DateTime.Now.AddYears(-34)
			};

			var dictionary = obj.ToGeneralDictionary().Elements;

			Assert.True(dictionary.ContainsKey(nameof(TestClass.Name)));
			Assert.True(dictionary.ContainsKey(nameof(TestClass.Age)));
			Assert.True(dictionary.ContainsKey(nameof(TestClass.DOB)));

			Assert.Equal(obj.Name,dictionary[nameof(TestClass.Name)] as string);
			Assert.Equal(obj.Age, Convert.ToInt32(dictionary[nameof(TestClass.Age)]));
			Assert.Equal(obj.DOB, Convert.ToDateTime(dictionary[nameof(TestClass.DOB)]));
		}

		[Fact]
		public void Deserilize_WithIDictionary_ShouldCastToObject()
		{
			TestClass testClass = new TestClass
			{
				Name = "name",
				Age = 34,
				DOB = DateTime.Now.AddYears(-34)
			};
			var dictionary = testClass.ToGeneralDictionary().Elements;

			var deserializedObject = JsonSerializer.Deserialize<TestClass>(JsonSerializer.Serialize(dictionary));

			Assert.Equal("name", deserializedObject.Name);
			Assert.Equal(34, deserializedObject.Age);
			Assert.Equal(testClass.DOB, deserializedObject.DOB);
		}

		[Fact]
		public void ToDictionary_WithInnerObject_ShouldReturnElementValueKind()
		{
			var elementValueKinds = TestObject.ToGeneralDictionary().ElementValueKind;

			Assert.Equal(JsonValueKind.Number, elementValueKinds["age"]);
			Assert.Equal(JsonValueKind.String, elementValueKinds["name"]);

			Assert.Equal(JsonValueKind.String, elementValueKinds["address.city"]);
			Assert.Equal(JsonValueKind.String, elementValueKinds["address.country"]);
			Assert.Equal(JsonValueKind.Object, elementValueKinds["address"]);

			Assert.Equal(JsonValueKind.Array, elementValueKinds["items"]);
			Assert.Equal(JsonValueKind.Number, elementValueKinds["items.id"]);
			Assert.Equal(JsonValueKind.String, elementValueKinds["items.title"]);
		}
	}
}