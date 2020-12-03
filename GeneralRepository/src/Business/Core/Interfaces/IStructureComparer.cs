using Core.Models;
using Core.Models.DataStructure;

namespace Core.Interfaces
{
	public interface IStructureComparer
	{
		bool Compare(StructureDefinition structure, JsonTranslation jsonTranslation);
	}
}