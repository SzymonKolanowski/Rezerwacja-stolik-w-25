using System;
using System.Collections.Generic;
using System.Text;

namespace Rezerwacja_stolików
{
	public class AllLists
	{
		public List<Table> Tables { get; set; }		

		public AllLists()
		{
			Tables = new List<Table>();						
		}		
	}
}
