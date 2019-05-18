using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

using LibraryModel;

namespace MergeSortQuery {
	class MergeSortQuery {
		public Library Library { get; set; }
		public int ThreadCount { get; set; }
        Sorter sorter = new Sorter();

        public List<Copy> ExecuteQuery() {
            List<Copy> copies = new List<Copy>();
            foreach (var item in Library.Copies)
            {
                if (item.State == CopyState.OnLoan && item.Book.Shelf[2] < 'R')
                {
                    copies.Add(item);
                }
            }


            MergeSorte(copies, (int)Math.Floor(Math.Log(ThreadCount, 2)), 0, copies.Count);
            

            return copies;
        }
        private void MergeSorte(List<Copy> copies, int depth, int start, int count)
        {
            if (depth<=0)
            {
                copies.Sort(start, count, sorter);
            }
            else
            {
                Task task1 = Task.Factory.StartNew(() => MergeSorte(copies, depth - 1, start, count / 2));
                MergeSorte(copies, depth - 1, start + count / 2, count - count / 2);
                Task.WaitAll(task1);

                var newList = new List<Copy>();
                newList.AddRange(copies.GetRange(start, count / 2));
                newList.AddRange(copies.GetRange(start + count / 2, count - count / 2));

                for (int i = 0; i < count; i++)
                {
                    copies[start + i] = newList[i];
                }
            }
        }
    }


    class Sorter : IComparer<Copy>
    {
        public int Compare(Copy x, Copy y)
        {
            int result;
            result = x.OnLoan.DueDate.CompareTo(y.OnLoan.DueDate);
            if (result != 0)
                return result;

            result = x.OnLoan.Client.LastName.CompareTo(y.OnLoan.Client.LastName);
            if (result != 0)
                return result;

            result = x.OnLoan.Client.FirstName.CompareTo(y.OnLoan.Client.FirstName);
            if (result != 0)
                return result;

            result = x.Book.Shelf.CompareTo(y.Book.Shelf);
            if (result != 0)
                return result;

            result = x.Id.CompareTo(y.Id);
            if (result != 0)
                return result;

            return result;
            
        }
    }


    class ResultVisualizer {
		public static void PrintCopy(StreamWriter writer, Copy c) {
			writer.WriteLine("{0} {1}: {2} loaned to {3}, {4}.",
                c.OnLoan.DueDate.ToShortDateString(),
                c.Book.Shelf,
                c.Id,
                c.OnLoan.Client.LastName,
                System.Globalization.StringInfo.GetNextTextElement(c.OnLoan.Client.FirstName));
		}
	}
}
