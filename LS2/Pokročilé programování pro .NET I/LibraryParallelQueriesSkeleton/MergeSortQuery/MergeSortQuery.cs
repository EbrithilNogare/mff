using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using System.IO;

using LibraryModel;

namespace MergeSortQuery {
	class MergeSortQuery {
		public Library Library { get; set; }
		public int ThreadCount { get; set; }

		public List<Copy> ExecuteQuery() {
			if (ThreadCount == 0) throw new InvalidOperationException("Threads property not set and default value 0 is not valid.");

			return new List<Copy>();
		}
	}

	class ResultVisualizer {
		public static void PrintCopy(StreamWriter writer, Copy c) {
			writer.WriteLine("{0} {1}: {2} loaned to {3}, {4}.", c.OnLoan.DueDate.ToShortDateString(), c.Book.Shelf, c.Id, c.OnLoan.Client.LastName, System.Globalization.StringInfo.GetNextTextElement(c.OnLoan.Client.FirstName));
		}
	}
}
