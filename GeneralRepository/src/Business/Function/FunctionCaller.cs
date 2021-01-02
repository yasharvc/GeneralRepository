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
			=> throw new NotImplementedException();
		private T CallFunction<T>(Core.Models.Function.Function function, params object[] input)
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

		public async Task<T> Call<T>(Core.Models.Function.Function function, params object[] parameters)
		{
			var functionType = function.CallPath.GetFunctionPathType();
			try
			{
				switch (functionType)
				{
					case Core.Enums.FunctionPathTypeEnum.Function:
						return CallFunction<T>(function, parameters);
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
