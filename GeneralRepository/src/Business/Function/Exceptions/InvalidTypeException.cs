using Core.Exceptions;

namespace Function.Exceptions
{
	public class InvalidTypeException : ExceptionOfApplication
	{
		public override int Code { get => -10000; }
	}
}
