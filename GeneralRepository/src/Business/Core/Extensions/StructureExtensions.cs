using Core.Models.DataStructure;

namespace Core.Extensions
{
	public static class StructureExtensions
	{
		public static string ToJsonSchema(this StructureDefinition structureDefinition)
		{
			return "";
		}

		class StructureToJsonSchemaStringConverter
		{
			StructureDefinition Structure { get; }



			public StructureToJsonSchemaStringConverter(StructureDefinition structure)
			{
				Structure = structure;
			}

		}
	}
}