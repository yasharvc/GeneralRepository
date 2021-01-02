using Core.Enums;
using Core.Models.DataStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.StructureDefinitionTests
{
	public class TypeToStructureTests
	{
		class ClassWithString
		{
			public string Name { get; set; }
		}

		[Fact]
		public void CTOR_WithSimpleStringInType_ShouldCreateField()
		{
			var structure = new StructureDefinition(typeof(ClassWithString));
			
			Assert.Single(structure.Fields);
			Assert.Equal("Name", structure.Fields.First().Name);
			Assert.Equal(DataTypeEnum.String, structure.Fields.First().DataType);
		}
	}
}
