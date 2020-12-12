using Core.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Models.DataStructure
{
	public class StructureDefinition : Model
	{
		public string Name { get; set; }
		public List<Field> Fields { get; set; }
		public List<Validator> Validators { get; set; }

		public async Task<bool> ValidateJsonStructure(string input) => 
			(await StructureDefinitionValidator.Validate(this, input)).Count() == 0;

		public async Task<bool> ValidateJsonStructure(object input) =>
			await ValidateJsonStructure(JsonSerializer.Serialize(input));
	}
}