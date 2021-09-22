using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Rezerwacja_stolików
{
	public class Table
	{
		public int ID { get; set; }
		public string Title { get; set; }
		public int Capacity { get; set; }
		public List<Reservation> ReservationList { get; set; }
		public Table()
		{
			ReservationList = new List<Reservation>();
		}
	}

	public class Reservation
	{
		public int ID { get; set; }
		public DateTime StartReservation { get; set; }
		public DateTime EndReservation { get; set; }


	}
}

	