using Core.Models.DataStructure;

namespace Core.Models.Function
{
	public class Parameter : Model
	{
		public string Name { get; set; }
		public Field Field { get; set; }
	}
}