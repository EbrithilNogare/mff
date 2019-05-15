using System;
using System.Collections.Generic;

using System.IO;
using System.Diagnostics;

using LibraryModel;

delegate List<Copy> QueryDelegate(Library library);

delegate void PrintCopyDelegate(StreamWriter writer, Copy copy);

class QueryTester {
	public static void RunQuery(string queryName, QueryDelegate query, PrintCopyDelegate printCopy, Library library) {
		Console.WriteLine("Executing {0} query ...", queryName);
		var swQuery = Stopwatch.StartNew();
		var result = query(library);
		swQuery.Stop();

		Console.WriteLine("Printing query results ...");

		var parts = queryName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
		var fileName = new System.Text.StringBuilder("Result");
		Array.ForEach(parts, p => {
			fileName.Append(char.ToUpperInvariant(p[0]));
			fileName.Append(p.Substring(1).ToLowerInvariant());
		});
		fileName.Append(".txt");
			
		Stopwatch swPrint = null;
		using (var writer = new System.IO.StreamWriter(fileName.ToString())) {
			swPrint = Stopwatch.StartNew();
			foreach (var c in result) {
				printCopy(writer, c);
			}
			swPrint.Stop();
		}

		Console.WriteLine("Query time: {0} s", swQuery.Elapsed.TotalSeconds);
		Console.WriteLine("Print time: {0} s", swPrint.Elapsed.TotalSeconds);
		Console.WriteLine();
	}
}

