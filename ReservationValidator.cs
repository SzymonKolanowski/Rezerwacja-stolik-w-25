using System;
using System.Collections.Generic;
using System.Text;

namespace Rezerwacja_stolików
{
	static class ReservationValidator
	{
		public static bool IsValid(Reservation newReservation, List<Reservation> existingReservations)
		{
			foreach (var existingReservation in existingReservations)
			{
				if (IsOverLap(newReservation,existingReservation))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsOverLap(Reservation first, Reservation second)
		{
			if (first.StartReservation > second.StartReservation || first.StartReservation < second.EndReservation ||
				
				first.EndReservation > second.StartReservation || first.EndReservation < second.EndReservation)
			{
				return true;
			}
			return false;
		}
	}
}
