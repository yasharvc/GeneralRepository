using Core.Enums;
using System;
using System.Linq;

namespace Function
{
	public static class CallPathMaker
	{

		//function://[Url to dll]@[Class FullName] => For function call
		public static string MakePath(FunctionPathTypeEnum callType, string path)
		{
			if (string.IsNullOrEmpty(path) || !IsCorectPath(path))
				throw new ArgumentException(nameof(path));
			switch (callType)
			{
				case FunctionPathTypeEnum.Function:
					return $"function://{path}";
				case FunctionPathTypeEnum.HTTP:
					return $"http://{path}";
				case FunctionPathTypeEnum.HTTPS:
					return $"https://{path}";
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static bool IsCorectPath(string path)
		{
			var excepts = "\\/%&@_+=:?.";
			return path.All(m => !char.IsPunctuation(m) || excepts.Contains(m));
		}

		public static FunctionPathTypeEnum GetFunctionPathType(this string path)
		{
			try
			{
				var str = path.Substring(0,path.IndexOf(":"));
				if (str.Equals("function", StringComparison.OrdinalIgnoreCase))
					return FunctionPathTypeEnum.Function;
				else if (str.Equals("http", StringComparison.OrdinalIgnoreCase))
					return FunctionPathTypeEnum.HTTP;
				else if (str.Equals("https", StringComparison.OrdinalIgnoreCase))
					return FunctionPathTypeEnum.HTTPS;
			}
			catch
			{
			}
			throw new ArgumentException();
		}

		public static string GetPath(this string path)
		{
			try
			{
				var type = path.GetFunctionPathType();
				if (type == FunctionPathTypeEnum.Function) 
				{
					return path.Substring(path.IndexOf(":") + 3);
				}
				else if (type == FunctionPathTypeEnum.HTTP || type == FunctionPathTypeEnum.HTTPS)
				{
					return path;
				}
			}
			catch { }
			throw new ArgumentException();
		}
	}
}
