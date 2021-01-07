using Core.Enums;
using Core.Models.DataStructure;
using System;
using System.Linq;
using Xunit;
using Core.Extensions;
using System.Collections.Generic;

namespace UnitTests.StructureDefinitionTests
{
	public class TypeToStructureTests
	{
		class TestClass
		{
			public string Name { get; set; }
			public int Age { get; set; }
			public DateTime DOB { get; set; }
			public TimeSpan Time { get; set; }
			public bool Married { get; set; }
			public double Average { get; set; }
			public Guid GUID { get; set; }
			public byte[] File { get; set; }
			public InnerTestClass innerObject { get; set; }
			public InnerTestClass[] ListOfObjects { get; set; }
		}

		class InnerTestClass
		{
			public string InnerName { get; set; }
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

		[Fact]
		public void CTOR_WithDateTimeInType_ShouldCreateField()
		{
			var structure = new StructureDefinition(typeof(TestClass));
			const string nameOfField = nameof(TestClass.DOB);

			Assert.True(structure.Fields.Count > 0);
			Assert.NotNull(structure.Fields.SingleOrDefault(m =>m.Name.Equals(nameOfField)));
			Field ageField = structure.Fields.Single(m => m.Name.Equals(nameOfField));
			Assert.Equal(nameOfField, ageField.Name);
			Assert.Equal(DataTypeEnum.DateTime, ageField.DataType);
		}

		[Fact]
		public void CTOR_WithTimeSpanInType_ShouldCreateField()
		{
			var structure = new StructureDefinition(typeof(TestClass));
			const string nameOfField = nameof(TestClass.Time);

			Assert.True(structure.Fields.Count > 0);
			Assert.NotNull(structure.Fields.SingleOrDefault(m => m.Name.Equals(nameOfField)));
			Field ageField = structure.Fields.Single(m => m.Name.Equals(nameOfField));
			Assert.Equal(nameOfField, ageField.Name);
			Assert.Equal(DataTypeEnum.Time, ageField.DataType);
		}
		[Fact]
		public void CTOR_WithBooleanInType_ShouldCreateField()
		{
			var structure = new StructureDefinition(typeof(TestClass));
			const string nameOfField = nameof(TestClass.Married);

			Assert.True(structure.Fields.Count > 0);
			Assert.NotNull(structure.Fields.SingleOrDefault(m => m.Name.Equals(nameOfField)));
			Field ageField = structure.Fields.Single(m => m.Name.Equals(nameOfField));
			Assert.Equal(nameOfField, ageField.Name);
			Assert.Equal(DataTypeEnum.Booelan, ageField.DataType);
		}
		[Fact]
		public void CTOR_WithDoubleInType_ShouldCreateField()
		{
			var structure = new StructureDefinition(typeof(TestClass));
			const string nameOfField = nameof(TestClass.Average);

			Assert.True(structure.Fields.Count > 0);
			Assert.NotNull(structure.Fields.SingleOrDefault(m => m.Name.Equals(nameOfField)));
			Field ageField = structure.Fields.Single(m => m.Name.Equals(nameOfField));
			Assert.Equal(nameOfField, ageField.Name);
			Assert.Equal(DataTypeEnum.Float, ageField.DataType);
		}
		[Fact]
		public void CTOR_WithGUIDInType_ShouldCreateField()
		{
			var structure = new StructureDefinition(typeof(TestClass));
			const string nameOfField = nameof(TestClass.GUID);

			Assert.True(structure.Fields.Count > 0);
			Assert.NotNull(structure.Fields.SingleOrDefault(m => m.Name.Equals(nameOfField)));
			Field ageField = structure.Fields.Single(m => m.Name.Equals(nameOfField));
			Assert.Equal(nameOfField, ageField.Name);
			Assert.Equal(DataTypeEnum.GUID, ageField.DataType);
		}
		[Fact]
		public void CTOR_WithByteArrayInType_ShouldCreateField()
		{
			var structure = new StructureDefinition(typeof(TestClass));
			const string nameOfField = nameof(TestClass.File);

			Assert.True(structure.Fields.Count > 0);
			Assert.NotNull(structure.Fields.SingleOrDefault(m => m.Name.Equals(nameOfField)));
			Field fileField = structure.Fields.Single(m => m.Name.Equals(nameOfField));
			Assert.Equal(nameOfField, fileField.Name);
			Assert.Equal(DataTypeEnum.Binary, fileField.DataType);
		}
		[Fact]
		public void CTOR_WithObjectInType_ShouldCreateField()
		{
			var structure = new StructureDefinition(typeof(TestClass));
			const string nameOfField = nameof(TestClass.innerObject);

			Assert.True(structure.Fields.Count > 0);
			Assert.NotNull(structure.Fields.SingleOrDefault(m => m.Name.Equals(nameOfField)));
			Field objectField = structure.Fields.Single(m => m.Name.Equals(nameOfField));
			Assert.Equal(nameOfField, objectField.Name);
			Assert.Equal(DataTypeEnum.Object, objectField.DataType);
			Assert.Contains(objectField.Structure.Fields, m => m.Name.EqualsIgnoreCase(nameof(InnerTestClass.InnerName)));
		}
	}
}