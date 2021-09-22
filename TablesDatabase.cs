using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Rezerwacja_stolików
{
	class TablesDatabase
	{
		AllLists allLists = new AllLists();
		
		
		private const string databasePath = "D://Szymon//Programowanie//Rezerwacja stolików//Rezerwacja stolików//Tables.json";

		public TablesDatabase()
		{
			string json = string.Empty;
			try
			{
				json = File.ReadAllText(databasePath);
			}
			catch { }
			allLists = JsonConvert.DeserializeObject<AllLists>(json) ?? new AllLists();
		}

		public IEnumerable<Table> TablesList()
		{
			return allLists.Tables;
		}		

		public void AddTable(Table table)
		{
			table.ID = allLists.Tables.Select(t => t.ID).DefaultIfEmpty().Max() + 1;
			allLists.Tables.Add(table);
		}		

		public void Save()
		{
			var json = JsonConvert.SerializeObject(allLists, Formatting.Indented);
			if (File.Exists(databasePath))
			{
				File.Delete(databasePath);
			}
			File.WriteAllText(databasePath, json);
		}

		public void RemoveTable(int id)
		{
			allLists.Tables.RemoveAll(t => id == t.ID);
		}	

		public Table GetTableById(int id)
		{
			return allLists.Tables.FirstOrDefault(a => id == a.ID);
		}
		

		public IEnumerable<Table> SortTableAsc()
		{
			return allLists.Tables.OrderBy(x => x.Capacity);
		}

		public IEnumerable<Table> SortTableDesc()
		{
			return allLists.Tables.OrderByDescending(x => x.Capacity);
		}
	}
}