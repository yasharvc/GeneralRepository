using Core.Enums;
using Core.Exceptions;
using Core.Extensions;
using Core.Models.Service;
using Function.Exceptions;
using Function.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Function.Callers
{
	internal class FunctionTypeCaller : IFunctionCaller
	{
		private static List<Type> AllTypesInRuntime { get; set; } = new List<Type>();

		static FunctionTypeCaller(){
			var assemblies = new HashSet<Assembly>
			{
				Assembly.GetExecutingAssembly(),
				Assembly.GetEntryAssembly(),
				Assembly.GetCallingAssembly()
			};
			Assembly.GetExecutingAssembly().GetReferencedAssemblies().ToList()
				.ForEach(an => assemblies.Add(Assembly.Load(an.ToString())));
			Assembly.GetEntryAssembly().GetReferencedAssemblies().ToList()
				.ForEach(an => assemblies.Add(Assembly.Load(an.ToString())));
			Assembly.GetCallingAssembly().GetReferencedAssemblies().ToList()
				.ForEach(an => assemblies.Add(Assembly.Load(an.ToString())));

			assemblies.ToList().ForEach(assm => AllTypesInRuntime.AddRange(assm.GetTypes()));
		}

		public async Task<GeneralResult> Call(Core.Models.Function.Function function, string input)
		{
			var functionPath = function.CallPath.GetPath();
			Assembly assembly;
			var dllPath = functionPath.Substring(0, functionPath.IndexOf("@"));
			functionPath = functionPath.Substring(functionPath.IndexOf("@") + 1);
			if (AllTypesInRuntime.Any(m => m.FullName.Equals(functionPath)))
			{
				assembly = AllTypesInRuntime.Single(m => m.FullName.Equals(functionPath)).Assembly;
			}
			else
			{
				assembly = Assembly.Load(File.ReadAllBytes(dllPath));
			}

			if (assembly == null)
				return new GeneralResult
				{
					Id = Guid.NewGuid().ToString(),
					CallResult = CallResultEnum.Failure,
					Result = null,
					Exceptions = new List<ExceptionOfApplication>
					{
						new InvalidTypeException()
					}
				};
			var obj = assembly.CreateInstance(functionPath);
			var method = obj.GetType().GetMethod(function.Name);
			var res = method.Invoke(obj, JsonToParameters(method.GetParameters(),input));
			if (res is Task task)
			{
				await task.ConfigureAwait(false);
				return new GeneralResult
				{
					Id = Guid.NewGuid().ToString(),
					CallResult = CallResultEnum.Success,
					Result = task.GetType().GetProperty("Result").GetValue(task),
					Exceptions = new List<ExceptionOfApplication>()
				};
			}
			else
			{
				return new GeneralResult
				{
					Id = Guid.NewGuid().ToString(),
					CallResult = CallResultEnum.Success,
					Result = res,
					Exceptions = new List<ExceptionOfApplication>()
				};
			}
		}

		private object[] JsonToParameters(ParameterInfo[] parameterInfos, string input)
		{
			var doc = JsonDocument.Parse(input);
			List<object> res = new List<object>();
			foreach (var param in parameterInfos)
			{
				try
				{
					var tempJson = doc.RootElement.GetProperty(param.Name);
					if (tempJson.ValueKind.IsSimpleType())
						res.Add(tempJson.Cast(param.ParameterType));
					else if (tempJson.ValueKind == JsonValueKind.Object)
						res.Add(JsonSerializer.Deserialize(tempJson.GetRawText(), param.ParameterType));
				}
				catch { }
			}
			return res.ToArray();
		}
	}
}
