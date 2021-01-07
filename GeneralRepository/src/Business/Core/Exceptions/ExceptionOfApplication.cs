using System;

namespace Core.Exceptions
{
	public abstract class ExceptionOfApplication : Exception
	{
		public virtual int Code { get; }
	}
}