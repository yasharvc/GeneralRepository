using Core.Models.DataStructure;
using System;
using System.Threading.Tasks;

namespace Core.Services
{
	public class StructureMapper
	{
		public StructureDefinition SourceStructure { get; set; }
		public StructureDefinition DestinationStructure { get; set; }
		public StructureMapping Mapping { get; set; }
		private string SourceSampleJson { get => SourceStructure?.ToSampleJson() ?? "{}"; }
		private string DestinationSampleJson
		{
			get => DestinationStructure?.ToSampleJson() ?? "{}";
		}

		public async Task<string> Map(string sourceJson) => throw new NotImplementedException();
	}
}
