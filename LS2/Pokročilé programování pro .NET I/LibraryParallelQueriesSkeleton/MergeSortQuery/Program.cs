using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LibraryModel;

namespace MergeSortQuery {
	class Program {
		static void Main(string[] args) {
			Library library = new Library();

            // Requires ~ 10 MB of RAM
            // RandomGenerator rg = new RandomGenerator { RandomSeed = 42, BookCount = 2501000 / 16, ClientCount = 1000, LoanCount = 50 * 1000, MaxCopyCount = 10 };

            // Requires ~ 800 MB of RAM
             RandomGenerator rg = new RandomGenerator { RandomSeed = 42, BookCount = 2500000 / 4, ClientCount = 10000, LoanCount = 500000 * 5, MaxCopyCount = 10 };

            // Requires ~ 1500 MB of RAM
            // RandomGenerator rg = new RandomGenerator { RandomSeed = 42, BookCount = 2500000 / 2, ClientCount = 10000, LoanCount = 500000 * 5, MaxCopyCount = 10 };

            // Requires ~ 5000 MB of RAM	
            // RandomGenerator rg = new RandomGenerator { RandomSeed = 42, BookCount = 2500000 * 2, ClientCount = 10000, LoanCount = 500000 * 10, MaxCopyCount = 10 };
            Console.WriteLine("Generating library content ...");
			rg.FillLibrary(library);

			QueryTester.RunQuery("reference single threaded", ReferenceLibraryQueries.Queries.SingleThreadedQuery, ReferenceLibraryQueries.Queries.PrintCopy, library);

			QueryTester.RunQuery("reference parallel", ReferenceLibraryQueries.Queries.ParallelQuery, ReferenceLibraryQueries.Queries.PrintCopy, library);

            foreach (var threads in new int[] { 1,2,4,8})
            {
                QueryTester.RunQuery(
                    string.Format("MergeSort {0:D2} threads", threads),
                    lib => new MergeSortQuery { Library = lib, ThreadCount = threads }.ExecuteQuery(),
                    ResultVisualizer.PrintCopy,
                    library
                );
            }

            Console.ReadKey();
		}
	}
}
