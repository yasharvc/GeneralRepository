using Core.Enums;
using Core.Models.DataStructure;
using Function;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.FunctionCallingTests
{
	public class FunctionCallTests
	{
		public class data
		{
			public string Fname { get; set; }
			public string Lname { get; set; }
		}

		public class CallTest
		{
			public string GetName() => "Name";
			public Task<string> GetNameAsync() => Task.FromResult("Name");
			public Task<string> SayHello(string name) => Task.FromResult($"Hello {name}!");
			public Task<double> Mult(double value) => Task.FromResult(value * 25);
			public Task<DateTime> AddTwoYear(DateTime input) => Task.FromResult(input.AddYears(2));
			public Task<string> GetFullName(data input) => Task.FromResult($"{input.Fname}-{input.Lname}");
		}

		[Fact]
		public async void FunctionCaller_WithNoParameterAsyncFunction_ShouldReturnString()
		{
			var caller = new FunctionCaller();
			var path = CallPathMaker.MakePath(FunctionPathTypeEnum.Function, $"{GetType().Assembly.Location}@{typeof(CallTest).FullName}");
			var res = await caller.Call(new Core.Models.Function.Function
			{
				CallPath = path,
				Name = nameof(CallTest.GetNameAsync),
				ReturnType = new StructureDefinition(typeof(string))
			}, "{}");

			Assert.Equal(CallResultEnum.Success, res.CallResult);
			Assert.Equal(await new CallTest().GetNameAsync(), res.Result.ToString());
			Assert.Empty(res.Exceptions);
		}
		[Fact]
		public async void FunctionCaller_WithNoParameterFunction_ShouldReturnString()
		{
			var caller = new FunctionCaller();
			var path = CallPathMaker.MakePath(FunctionPathTypeEnum.Function, $"{GetType().Assembly.Location}@{typeof(CallTest).FullName}");
			var res = await caller.Call(new Core.Models.Function.Function
			{
				CallPath = path,
				Name = nameof(CallTest.GetName),
				ReturnType = new StructureDefinition(typeof(string))
			}, "{}");

			Assert.Equal(CallResultEnum.Success, res.CallResult);
			Assert.Equal(new CallTest().GetName(), res.Result.ToString());
			Assert.Empty(res.Exceptions);
		}
		[Fact]
		public async void FunctionCaller_WithStringParameterFunction_ShouldReturnString()
		{
			var caller = new FunctionCaller();
			var path = CallPathMaker.MakePath(FunctionPathTypeEnum.Function, $"{GetType().Assembly.Location}@{typeof(CallTest).FullName}");
			var res = await caller.Call(new Core.Models.Function.Function
			{
				CallPath = path,
				Name = nameof(CallTest.SayHello),
				ReturnType = new StructureDefinition(typeof(string)),
				Parameters = new StructureDefinition
				{
					Fields = new List<Field>
					{
						Field.NotNullString("name","name")
					}
				}
			}, System.Text.Json.JsonSerializer.Serialize(new { name = "Yashar" }));

			Assert.Equal(CallResultEnum.Success, res.CallResult);
			Assert.Equal(await new CallTest().SayHello("Yashar"), res.Result.ToString());
			Assert.Empty(res.Exceptions);
		}
		[Fact]
		public async void FunctionCaller_WithDoubleParameterFunction_ShouldReturnCalculatedValue()
		{
			var caller = new FunctionCaller();
			var path = CallPathMaker.MakePath(FunctionPathTypeEnum.Function,
				$"{GetType().Assembly.Location}@{typeof(CallTest).FullName}");
			var res = await caller.Call(new Core.Models.Function.Function
			{
				CallPath = path,
				Name = nameof(CallTest.Mult),
				ReturnType = new StructureDefinition(typeof(double)),
				Parameters = new StructureDefinition
				{
					Fields = new List<Field>
					{
						Field.NotNullFloat("value","value")
					}
				}
			}, System.Text.Json.JsonSerializer.Serialize(new { value = 100 }));

			Assert.Equal(CallResultEnum.Success, res.CallResult);
			Assert.Equal(await new CallTest().Mult(100), Convert.ToDouble(res.Result));
			Assert.Empty(res.Exceptions);
		}
		[Fact]
		public async void FunctionCaller_WithDateTimeParameterFunction_ShouldReturnCalculatedDateTime()
		{
			var caller = new FunctionCaller();
			var path = CallPathMaker.MakePath(FunctionPathTypeEnum.Function,
				$"{GetType().Assembly.Location}@{typeof(CallTest).FullName}");

			var date = DateTime.Now;

			var res = await caller.Call(new Core.Models.Function.Function
			{
				CallPath = path,
				Name = nameof(CallTest.AddTwoYear),
				ReturnType = new StructureDefinition(typeof(double)),
				Parameters = new StructureDefinition
				{
					Fields = new List<Field>
					{
						Field.NotNullDateTime("input","input")
					}
				}
			}, System.Text.Json.JsonSerializer.Serialize(new { input = date }));

			Assert.Equal(CallResultEnum.Success, res.CallResult);
			Assert.Equal(await new CallTest().AddTwoYear(date), Convert.ToDateTime(res.Result));
			Assert.Empty(res.Exceptions);
		}
		[Fact]
		public async void FunctionCaller_WithClassParameterFunction_ShouldReturnString()
		{
			var caller = new FunctionCaller();
			var path = CallPathMaker.MakePath(FunctionPathTypeEnum.Function,
				$"{GetType().Assembly.Location}@{typeof(CallTest).FullName}");
			var input = new data
			{
				Fname = "Yashar",
				Lname = "Aliabbasi"
			};

			var res = await caller.Call(new Core.Models.Function.Function
			{
				CallPath = path,
				Name = nameof(CallTest.GetFullName),
				ReturnType = new StructureDefinition(typeof(string)),
				Parameters = new StructureDefinition
				{
					Fields = new List<Field>
					{
						new Field
						{
							DataType = DataTypeEnum.Object,
							Name = "input",
							Id="input",
							Nullable=false,
							Structure = new StructureDefinition
							{
								Fields=new List<Field>
								{
									Field.NotNullString("Fname","Fname"),
									Field.NotNullString("Lname","Lname"),
								}
							}
						}
					}
				}
			}, System.Text.Json.JsonSerializer.Serialize(new { input }));

			Assert.Equal(CallResultEnum.Success, res.CallResult);
			Assert.Equal(await new CallTest().GetFullName(input), res.Result.ToString());
			Assert.Empty(res.Exceptions);
		}
	}
}
