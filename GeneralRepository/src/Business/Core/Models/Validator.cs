using System.Collections.Generic;

namespace Core.Models
{
	public class Validator : Model
	{
		public List<string> RequiredFields { get; set; }
		public string UniqueName { get; set; }
	}
}