using Core.Models;
using System.Threading.Tasks;

namespace Core.Interfaces
{
	public interface IDataValidator
	{
		Task<bool> UniqueIdentifier { get; }
		Task<bool> IsEligableValidator(Validator validator);
		Task<bool> Validate(string input);
	}
}