using Core.Models.Service;
using System.Threading.Tasks;

namespace Function.Interfaces
{
	public interface IFunctionCaller
	{
		Task<GeneralResult> Call(Core.Models.Function.Function function, string input);
	}
}