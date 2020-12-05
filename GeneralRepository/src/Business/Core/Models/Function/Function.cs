using Core.Models.DataStructure;
using System.Collections.Generic;

namespace Core.Models.Function
{
	public class Function : Model
	{
		public string Name { get; set; }
		public Field ReturnType { get; set; }
		public IEnumerable<Parameter> Parameters { get; set; }
	}
}