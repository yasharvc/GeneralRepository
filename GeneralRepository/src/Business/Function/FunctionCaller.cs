using Core.Extensions;
using Core.Models;
using Function.Interfaces;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Function
{
	public class FunctionCaller : IFunctionCaller
	{
		public async Task<T> Call<T>(Core.Models.Function.Function function, string input) 
			=> await Call<T>(function, input.ToGeneralDictionary());

		public async Task<T> Call<T>(Core.Models.Function.Function function, JsonTranslation input)
		{
			var functionType = function.CallPath.GetFunctionPathType();
			try
			{
				switch (functionType)
				{
					case Core.Enums.FunctionPathTypeEnum.Function:
						return CallFunction<T>(function, input);
					case Core.Enums.FunctionPathTypeEnum.HTTP:
						throw new NotImplementedException();
					case Core.Enums.FunctionPathTypeEnum.HTTPS:
						throw new NotImplementedException();
					default:
						throw new ArgumentException();
				}
			}
			catch
			{
				throw;
			}
		}

		private T CallFunction<T>(Core.Models.Function.Function function, JsonTranslation input)
		{
			var functionPath = function.CallPath.GetPath();
			Assembly assembly;
			var dllPath = functionPath.Substring(0,functionPath.IndexOf("@"));
			functionPath = functionPath.Substring(functionPath.IndexOf("@") + 1);
			assembly = Assembly.Load(File.ReadAllBytes(dllPath));
			
			if (assembly == null)
				throw new ArgumentException();
			var obj = assembly.CreateInstance(functionPath);
			return ConvertType<T>(obj.GetType().GetMethod(function.Name).Invoke(obj, null));
		}

		public Task<T> Call<T>(Core.Models.Function.Function function, params object[] parameters)
		{
			throw new NotImplementedException();
		}

		private T ConvertType<T>(object input)
		{
			try
			{
				return (T)Convert.ChangeType(input, typeof(T));
			}
			catch
			{
				throw;
			}
		}
	}
}
