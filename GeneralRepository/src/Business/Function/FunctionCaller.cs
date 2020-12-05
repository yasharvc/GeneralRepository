using Core.Models;
using Function.Interfaces;
using System;
using System.Threading.Tasks;

namespace Function
{
	public class FunctionCaller : IFunctionCaller
	{
		public Task<T> Call<T>(Core.Models.Function.Function function, string input)
		{
			function.
		}

		public Task<T> Call<T>(Core.Models.Function.Function function, JsonTranslation input)
		{
			throw new NotImplementedException();
		}

		public Task<T> Call<T>(Core.Models.Function.Function function, params object[] parameters)
		{
			throw new NotImplementedException();
		}
	}
}
