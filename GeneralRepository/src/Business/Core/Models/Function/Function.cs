using Core.Models.DataStructure;
using System.Collections.Generic;

namespace Core.Models.Function
{
	public class Function : Model
	{
		public string Name { get; set; }
		public StructureDefinition ReturnType { get; set; }
		public StructureDefinition Parameters { get; set; }
		public string CallPath { get; set; }
	}
}