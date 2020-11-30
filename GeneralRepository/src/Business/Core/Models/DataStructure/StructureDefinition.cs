﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.DataStructure
{
	public class StructureDefinition : Model
	{
		public string Name { get; set; }
		public List<Field> Fields { get; set; }
	}
}
