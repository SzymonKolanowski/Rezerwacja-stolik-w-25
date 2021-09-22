using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace Rezerwacja_stolików
{
	class Program
	{
		private static TablesDatabase database = new TablesDatabase();
		static void Main(string[] args)
		{
			string command = string.Empty;
			do
			{
				Console.WriteLine(" If you want add table write 'AddTable'. If you want see actual list of tables write 'TablesList'");
				Console.WriteLine("If you want remove table write 'RemoveTable'. If you want see some table write 'ShowTable'");
				Console.WriteLine("If you want sort table ascending write 'SortTableAsc'. If you want sort table descending write 'SortTableDesc'.");
				Console.WriteLine("If you want sort Table by Id write 'SortTableById'.If you want add reservation write 'AddReservation'");
				Console.WriteLine("If you want remove reservation write 'RemoveReservation'");
				Console.WriteLine("If you want see all reservation with some day write 'ShowDayliReservation'");
				Console.WriteLine("If you want see tables which meet your requirements write 'ShowMatchingTables'");
				Console.WriteLine("If you want leave program write 'Exit' ");
				command = Console.ReadLine();

				switch (command)
				{
					case "AddTable":
						AddTable();
						break;
					case "TablesList":
						TablesList();
						break;
					case "RemoveTable":
						RemoveTable();
						break;
					case "ShowTable":
						ShowTable();
						break;
					case "SortTableAsc":
						SortTableAsc();
						break;
					case "SortTableDesc":
						SortTableDesc();
						break;
					case "AddReservation":
						AddReservation();
						break;
					case "RemoveReservation":
						RemoveReservation();
						break;
					case "ShowDayliReservation":
						ShowDayliReservation();
						break;
					case "ShowMatchingTables":
						ShowMatchingTables();
						break;
				}
			} while (command != "Exit");
			Console.WriteLine("Exiting program");
			database.Save();
		}
		private static void TablesList()
		{
			var tables = database.TablesList();
			WriteJson(tables);
		}

		private static void AddTable()
		{
			Console.WriteLine("Capacity:");
			var capacity = GetIntParameter();

			Console.WriteLine("Title");
			var title = Console.ReadLine();

			var table = new Table
			{
				Capacity = capacity,
				Title = title,
			};

			database.AddTable(table);
		}

		private static int GetIntParameter()
		{
			var idInput = Console.ReadLine();
			var id = int.TryParse(idInput, out var parsedID)
				 ? parsedID
				 : 0;

			return id;
		}

		private static void WriteJson(object obj)
		{
			var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
			Console.WriteLine(json);
		}

		private static void RemoveTable()
		{
			Console.WriteLine("Choose table which you want remove");
			var idtable = GetIntParameter();
			database.RemoveTable(idtable);
		}

		private static void ShowTable()
		{
			Console.WriteLine("Choose table which you want to see");
			var idtable = GetIntParameter();
			Table table = database.GetTableById(idtable);

			var tableViewModel = new
			{
				table.ID,
				table.Title,
				table.Capacity,
				table.ReservationList
			};
			WriteJson(tableViewModel);
		}

		private static void SortTableAsc()
		{
			var tables = database.SortTableAsc();
			WriteJson(tables);
		}

		private static void SortTableDesc()
		{
			var tables = database.SortTableDesc();
			WriteJson(tables);
		}

		private static void AddReservation()
		{
			Console.WriteLine("choose id of table");
			var idTable = GetIntParameter();

			Console.WriteLine("choose capacity of the table which you need");
			var capacityTable = GetIntParameter();

			var table = database.GetTableById(idTable);
			table.ReservationList ??= new List<Reservation>();

			if (capacityTable > table.Capacity)
			{
				Console.WriteLine("Capacity of this table is not enough");
				return;
			}

			Console.WriteLine("Choose date and hour of  start reservation");
			var dateInput = Console.ReadLine();
			var date = DateTime.TryParse(dateInput, out var parsedDate)
				? parsedDate
				: default(DateTime?);

			Console.WriteLine("choose date and hour of end reservation");
			var dateInput2 = Console.ReadLine();
			var date2 = DateTime.TryParse(dateInput2, out var parsedDate2)
				? parsedDate2
				: default(DateTime?);			

			var tableViewModel = new
			{
				table.ID,
				table.Title,
				table.Capacity,
				table.ReservationList
			};

			var reservation = new Reservation
			{
				ID = table.ReservationList.Select(d => d.ID).DefaultIfEmpty().Max() + 1,
				StartReservation = parsedDate,
				EndReservation = parsedDate2
			};

			List<Reservation> reservations = new List<Reservation>();

			if (date >= date2)
			{
				Console.WriteLine("this reservation have not sense");
				return;
			}

			if (ReservationValidator.IsValid(reservation, table.ReservationList))
			{
				table.ReservationList.Add(reservation);
			}

			else
			{
				Console.WriteLine("reservation is overlaping existing once");
			}
		}

		private static void RemoveReservation()
		{
			Console.WriteLine("Choose id of Table with your reservation");
			var idTable = GetIntParameter();

			Console.WriteLine("Choose id of reservation which you want remove");
			var idReservation = GetIntParameter();

			var table = database.GetTableById(idTable);
			var reservation = table.ReservationList.FirstOrDefault(r => idReservation == r.ID);

			table.ReservationList.Remove(reservation);
		}

		private static void ShowDayliReservation()
		{
			Console.WriteLine("Choose day when you want to see all reservation in restaurant");
			var dateInput = Console.ReadLine();

			var date = DateTime.TryParse(dateInput, out var parsedDate)
				? parsedDate
				: default(DateTime?);

			foreach (var table in database.TablesList())
			{
				Console.WriteLine(table.ID);

				var matchingReservations = table.ReservationList
					.Where(res => res.StartReservation.Date == date);

				WriteJson(matchingReservations);
			}
		}

		static void ShowMatchingTables()
		{
			Console.WriteLine("choose capacity of the table which you need");
			var capacityTable = GetIntParameter();

			Console.WriteLine("Choose date and hour of  start reservation");
			var dateInput = Console.ReadLine();
			var date = DateTime.TryParse(dateInput, out var parsedDate)
				? parsedDate
				: default(DateTime?);

			Console.WriteLine("choose date and hour of end reservation");
			var dateInput2 = Console.ReadLine();
			var date2 = DateTime.TryParse(dateInput2, out var parsedDate2)
				? parsedDate2
				: default(DateTime?);

			var reservation = new Reservation
			{
				StartReservation = parsedDate,
				EndReservation = parsedDate2
			};
				
			List<Reservation> reservations = new List<Reservation>();

			foreach (var table in database.TablesList())
			{

				if (table.Capacity >= capacityTable && ReservationValidator.IsValid(reservation, table.ReservationList))

				{
					Console.WriteLine("Table ID");
					Console.WriteLine(table.ID);
					Console.WriteLine("Table Capacity");
					Console.WriteLine(table.Capacity);
				}			
			}
		}
	}
}


