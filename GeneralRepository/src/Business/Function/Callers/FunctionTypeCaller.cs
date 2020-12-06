using Core.Models;
using Function.Interfaces;
using System.Threading.Tasks;

namespace Function.Callers
{
	internal class FunctionTypeCaller : IFunctionCaller
	{
		public Task<T> Call<T>(Core.Models.Function.Function function, string input)
		{
			throw new System.NotImplementedException();
		}

		public Task<T> Call<T>(Core.Models.Function.Function function, JsonTranslation input)
		{
			throw new System.NotImplementedException();
		}

		public Task<T> Call<T>(Core.Models.Function.Function function, params object[] parameters)
		{
			throw new System.NotImplementedException();
		}
	}
}
