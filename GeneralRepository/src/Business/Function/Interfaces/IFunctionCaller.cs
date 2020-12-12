using Core.Models;
using System.Threading.Tasks;

namespace Function.Interfaces
{
	public interface IFunctionCaller
	{
		Task<T> Call<T>(Core.Models.Function.Function function, string input);
		Task<T> Call<T>(Core.Models.Function.Function function, params object[] parameters);
	}
}