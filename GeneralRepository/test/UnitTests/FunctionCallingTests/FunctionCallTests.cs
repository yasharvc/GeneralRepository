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
		public void CreateObjectFromRunningAssemblies()
		{
			var assemblies = new HashSet<Assembly>
			{
				Assembly.GetExecutingAssembly(),
				Assembly.GetEntryAssembly(),
				Assembly.GetCallingAssembly()
			};
			var allTypes = new List<Type>();
			Assembly.GetExecutingAssembly().GetReferencedAssemblies().ToList()
				.ForEach(an => assemblies.Add(Assembly.Load(an.ToString())));
			Assembly.GetEntryAssembly().GetReferencedAssemblies().ToList().ForEach(an => assemblies.Add(Assembly.Load(an.ToString())));

			assemblies.ToList().ForEach(assm => allTypes.AddRange(assm.GetTypes()));

			var type = allTypes.Single(m => m.FullName.Equals(typeof(CallTest).FullName));
			var obj = type.Assembly.CreateInstance(type.FullName);
			var method = type.GetMethod(nameof(CallTest.GetName));
			Assert.NotNull(obj);
			Assert.NotNull(method);
			Assert.Equal(new CallTest().GetName(), method.Invoke(obj, Array.Empty<object>())?.ToString() ?? "");
		}

		[Fact]
		public async void CCC()
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
	}
}
