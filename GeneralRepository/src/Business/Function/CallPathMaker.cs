using Core.Enums;
using System;

namespace Function
{
	public static class CallPathMaker
	{
		public static string MakePath(FunctionPathTypeEnum callType, string path,string funcName,string extraPathToFunc = "")
		{
			if (!string.IsNullOrEmpty(extraPathToFunc))
				extraPathToFunc = $"{(path.EndsWith("/") ? "" : "/")}{extraPathToFunc}";
			switch (callType)
			{
				case FunctionPathTypeEnum.Function:
					return $"function://{path}{extraPathToFunc}#{funcName}";
				case FunctionPathTypeEnum.HTTP:
					return $"http://{path}{extraPathToFunc}/{funcName}";
				case FunctionPathTypeEnum.HTTPS:
					return $"https://{path}{extraPathToFunc}/{funcName}";
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static FunctionPathTypeEnum GetFunctionPathType(this string path)
		{
			try
			{
				var str = path.Substring(path.IndexOf(":"));
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
					var temp = path.Substring(path.IndexOf(":") + 3);
					return temp.Substring(0, temp.LastIndexOf("#"));
				}
				else if (type == FunctionPathTypeEnum.HTTP || type == FunctionPathTypeEnum.HTTPS)
				{
					var temp = path.Substring(path.IndexOf(":") + 3);
					return temp.Substring(0, temp.LastIndexOf("/"));
				}
			}
			catch { }
			throw new ArgumentException();
		}

		public static string GetFunctionName(this string path)
		{
			try
			{
				var type = path.GetFunctionPathType();
				if (type == FunctionPathTypeEnum.Function)
				{
					return path.Substring(path.LastIndexOf("#") + 1);
				}
				else if (type == FunctionPathTypeEnum.HTTP || type == FunctionPathTypeEnum.HTTPS)
				{
					return path.Substring(path.LastIndexOf("/") + 1);
				}
			}
			catch { }
			throw new ArgumentException();
		}
	}
}
