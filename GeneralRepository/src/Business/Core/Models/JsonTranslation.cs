using System.Collections.Generic;
using System.Text.Json;

namespace Core.Models
{
	public class JsonTranslation
	{
		public IDictionary<string, object> Elements { get; set; }
		public IDictionary<string, JsonValueKind> ElementValueKind { get; set; }
	}
}