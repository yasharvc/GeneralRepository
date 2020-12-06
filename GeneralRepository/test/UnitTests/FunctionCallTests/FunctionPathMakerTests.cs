using Core.Enums;
using System;
using Xunit;
using Function;

namespace UnitTests.FunctionCallTests
{
	public class FunctionPathMakerTests
	{
		[Fact]
		public void MakePath_WithFunctionType_ShouldThrowException()
		{
			Assert.Throws<ArgumentException>(() => CallPathMaker.MakePath(FunctionPathTypeEnum.Function, "", ""));
		}

		[Fact]
		public void MakePath_WithFunctionTypeAndPathWithoutFuncName_ShouldThrowException()
		{
			Assert.Throws<ArgumentException>(() => CallPathMaker.MakePath(FunctionPathTypeEnum.Function, "x/y", ""));
		}

		[Fact]
		public void MakePath_WithFunctionTypeAndWrongPathAndFuncName_ShouldThrowException()
		{
			Assert.Throws<ArgumentException>(() => CallPathMaker.MakePath(FunctionPathTypeEnum.Function, "x)y", ""));
		}

		[Fact]
		public void GetPath_WithFunctionTypeAndPathAndFuncName_ShouldReturnPath()
		{
			var path = CallPathMaker.MakePath(FunctionPathTypeEnum.Function, "test.dll", "add");
			var expected = "test.dll";

			Assert.Equal(expected, path.GetPath());
		}


	}
}
