using Core.Enums;
using Core.Models.DataStructure;
using Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.FunctionCallingTests
{
	public class FunctionCallTests
	{
		public class CallTest
		{
			public string GetName() => "Name";
			public Task<string> GetNameAsync() => Task.FromResult("Name");
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
	}
}
