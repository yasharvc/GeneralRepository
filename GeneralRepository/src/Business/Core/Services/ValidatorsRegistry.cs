using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
	public class ValidatorsRegistry : IValidatorsRegistry
	{
		Dictionary<string,List<IDataValidator>> UniqueIdentifiersToValidators { get; set; }

		//Network Listener
		//Assembly Listener
	}
}
