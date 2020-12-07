using Core.Enums;
using Core.Models;
using Function;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.FunctionCallTests
{
	public class TestResult
	{
		public int Age { get; set; }
	}
	public class TestClass
	{
		public string SayHello() => "Hello";
		public TestResult GetAge() => new TestResult { Age = 30 };
	}
	public class FunctionCallTests
	{
		[Fact]
		public async Task Call_WithFunctionWithNoInputAndStringOutput_ShouldReturnString()
		{
			var path = CallPathMaker.MakePath(FunctionPathTypeEnum.Function, $"{typeof(TestClass).Assembly.Location}@{typeof(TestClass).FullName}");

			var runResult = await new FunctionCaller().Call<string>(new Core.Models.Function.Function
			{
				Id = "test",
				CallPath = path,
				Name = "SayHello",
				Parameters = new List<Core.Models.Function.Parameter>(),
				ReturnType = new Core.Models.DataStructure.Field
				{
					DataType = BasicDataTypeEnum.String,
					FullName = "res",
					Id = "res",
					Name = "res",
					RelationType = RelationTypeEnum.NoRelation,
					Validators = new List<Validator>()
				}
			}, new JsonTranslation());

			var expected = new TestClass().SayHello();

			Assert.Equal(expected, runResult);
		}

		[Fact]
		public async Task Call_WithFunctionWithNoInputAndObjectOutput_ShouldReturnString()
		{
			var path = CallPathMaker.MakePath(FunctionPathTypeEnum.Function, $"{typeof(TestClass).Assembly.Location}@{typeof(TestClass).FullName}");

			var runResult = await new FunctionCaller().Call<string>(new Core.Models.Function.Function
			{
				Id = "test",
				CallPath = path,
				Name = "GetAge",
				Parameters = new List<Core.Models.Function.Parameter>(),
				ReturnType = new Core.Models.DataStructure.Field
				{
					DataType = BasicDataTypeEnum.None,
					FullName = "res",
					Id = "res",
					Name = "res",
					RelationType = RelationTypeEnum.OneToOne,
					Validators = new List<Validator>()
				}
			}, new JsonTranslation());

			var expected = new TestClass().SayHello();

			Assert.Equal(expected, runResult);
		}
	}
}
