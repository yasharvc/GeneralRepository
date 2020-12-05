using Core.Enums;
using System.Collections.Generic;

namespace Core.Models.DataStructure
{
	public class Field : Model
	{
		public BasicDataTypeEnum DataType { get; set; }
		public RelationTypeEnum RelationType { get; set; }
		public string Name { get; set; }
		public string FullName { get; set; }//tbl1.items.name => items is a array [1-n] for tb1 and each of them has name
		public List<Validator> Validators { get; set; }
		public virtual bool IsVoid { get; } = false;
	}
}
