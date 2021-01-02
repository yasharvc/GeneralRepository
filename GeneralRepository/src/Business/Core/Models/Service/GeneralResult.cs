using Core.Enums;
using Core.Exceptions;
using System.Collections.Generic;

namespace Core.Models.Service
{
	public class GeneralResult : Model
	{
		public CallResultEnum CallResult { get; set; }
		public object Result { get; set; }
		public IEnumerable<ExceptionOfApplication> Exceptions { get; set; }
	}
}