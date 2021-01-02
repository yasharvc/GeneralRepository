using Core.Enums;
using Core.Models.DataStructure;
using System.Linq;
using Xunit;

namespace UnitTests.StructureDefinitionTests
{
	public class TypeToStructureTests
	{
		class TestClass
		{
			public string Name { get; set; }
			public int Age { get; set; }
		}

		[Fact]
		public void CTOR_WithStringInType_ShouldCreateField()
		{
			var structure = new StructureDefinition(typeof(TestClass));
			
			Assert.True(structure.Fields.Count > 0);
			Assert.NotNull(structure.Fields.SingleOrDefault(m => m.Name.Equals(nameof(TestClass.Name))));
			Assert.Equal("Name", structure.Fields.Single(m=>m.Name.Equals(nameof(TestClass.Name))).Name);
			Assert.Equal(DataTypeEnum.String, structure.Fields.Single(m => m.Name.Equals(nameof(TestClass.Name))).DataType);
		}

		[Fact]
		public void CTOR_WithIntegerInType_ShouldCreateField()
		{
			var structure = new StructureDefinition(typeof(TestClass));

			Assert.True(structure.Fields.Count > 0);
			Assert.NotNull(structure.Fields.SingleOrDefault(m => m.Name.Equals(nameof(TestClass.Age))));
			Field ageField = structure.Fields.Single(m => m.Name.Equals(nameof(TestClass.Age)));
			Assert.Equal("Age", ageField.Name);
			Assert.Equal(DataTypeEnum.Integer, ageField.DataType);
		}
	}
}