﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Items
{
	public class Theme : Item
	{
		public ICollection<Keyword> Keywords { get; set; }
	}
}
