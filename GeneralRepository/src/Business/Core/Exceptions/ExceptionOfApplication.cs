using System;

namespace Core.Exceptions
{
	public abstract class ExceptionOfApplication : Exception
	{
		public int Code { get; set; }
	}
}