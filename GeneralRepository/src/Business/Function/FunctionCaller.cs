using Core.Enums;
using Core.Models.Service;
using Function.Callers;
using Function.Interfaces;
using System;
using System.Threading.Tasks;

namespace Function
{
	public class FunctionCaller : IFunctionCaller
	{
		private async Task<GeneralResult> CallFunction(Core.Models.Function.Function function, string input) => 
			await new FunctionTypeCaller().Call(function, input);


		public async Task<GeneralResult> Call(Core.Models.Function.Function function, string input)
		{
			var functionType = function.CallPath.GetFunctionPathType();
			try
			{
				switch (functionType)
				{
					case FunctionPathTypeEnum.Function:
						return await CallFunction(function, input);
					case FunctionPathTypeEnum.HTTP:
						throw new NotImplementedException();
					case FunctionPathTypeEnum.HTTPS:
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
