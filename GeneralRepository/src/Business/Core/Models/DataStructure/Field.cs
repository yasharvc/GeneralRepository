using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DataStructure
{
	public class Field : Model
	{
		public BasicDataTypeEnum DataType { get; set; }
		public string MyProperty { get; set; }
	}
}
