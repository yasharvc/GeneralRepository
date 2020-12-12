using System;
using System.Text;

namespace Core.Extensions
{
	public static class StringExtensions
	{
		public static byte[] ToBytes(this string str,Encoding encoding = null)
		{
			encoding = encoding ?? Encoding.UTF8;
			return encoding.GetBytes(str);
		}

		public static string ToBase64(this string str, Encoding encoding = null) => Convert.ToBase64String(str.ToBytes(encoding));
	}
}