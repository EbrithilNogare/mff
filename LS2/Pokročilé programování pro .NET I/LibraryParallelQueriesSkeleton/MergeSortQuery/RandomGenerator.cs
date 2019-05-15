using System;
using System.Text;

using LibraryModel;

class RandomGenerator {
	public int BookCount { get; set; }
	public int MaxCopyCount { get; set; }
	public int LoanCount { get; set; }
	public int ClientCount { get; set; }
	public int RandomSeed { get; set; }

	private Random randomInstance = null;

	private Random r {
		get {
			if (randomInstance == null) {
				randomInstance = new Random(RandomSeed);
			}
			return randomInstance;
		}
	}

	private string GetRandomNumberString(int numberCount) {
		var sb = new StringBuilder(numberCount);

		for (int i = 0; i < numberCount; i++) {
			sb.Append((char) ('0' + r.Next(10)));
		}

		return sb.ToString();
	}

	private string GetRandomEnglishUpperString(int letterCount) {
		var sb = new StringBuilder(letterCount);

		for (int i = 0; i < letterCount; i++) {
			sb.Append((char) ('A' + r.Next('Z' - 'A' + 1)));
		}

		return sb.ToString();
	}

	private string GetRandomName(int length) {
		var sb = new StringBuilder(length);

		int type0LastIdx = -1;
		int type1LastIdx = -1;

		sb.Append((char) ('A' + r.Next('Z' - 'A' + 1)));
		for (int i = 1; i < length; i++) {
			var type = r.Next(6);
			if (type == 0 && i != type0LastIdx) {
				sb.Append((char) (0x300 + r.Next(0x0A)));
				type0LastIdx = i;
			} else if (type == 1 && i != type1LastIdx) {
				sb.Append((char) (0x326 + r.Next(0x32B - 0x326 + 1)));
				type1LastIdx = i;
			} else {
				sb.Append((char) ('a' + r.Next('z' - 'a' + 1)));
			}
		}

		return sb.ToString();
	}

	public void FillLibrary(Library library) {
		for (int i = 0; i < BookCount; i++) {
			var b = new Book();
			b.Author = GetRandomName(5) + " " + GetRandomName(10);
			b.Title = GetRandomName(30);
			var l1 = r.Next(7) + 2;
			var l2 = r.Next(l1 - 1) + 1;
			b.Isbn = GetRandomNumberString(l2) + "-" + GetRandomNumberString(9 - l1) + "-" + GetRandomNumberString(l1 - l2) + "-" + GetRandomNumberString(1);
			b.DatePublished = new DateTime(1950 + r.Next(50), 1, 1);
			b.Shelf = GetRandomNumberString(2) + GetRandomEnglishUpperString(1);

			int copies = r.Next(MaxCopyCount) + 1;
			for (int j = 0; j < copies; j++) {
				var c = new Copy();
				c.Book = b;
				c.Id = GetRandomNumberString(2) + GetRandomEnglishUpperString(2) + GetRandomNumberString(4);
				c.State = CopyState.InShelf;

				b.Copies.Add(c);
				library.Copies.Add(c);
			}

			library.Books.Add(b);
		}

		for (int i = 0; i < ClientCount; i++) {
			var c = new Client();
			c.FirstName = GetRandomName(7);
			c.LastName = GetRandomName(12);
			library.Clients.Add(c);
		}

		var today = DateTime.Today;
		for (int i = 0; i < LoanCount; i++) {
			var copy = library.Copies[r.Next(library.Copies.Count)];
			if (copy.State == CopyState.OnLoan) continue;

			var l = new Loan();
			l.Client = library.Clients[r.Next(ClientCount)];
			l.Copy = copy;
			l.DueDate = today.AddDays(r.Next(31));

			l.Copy.OnLoan = l;
			l.Copy.State = CopyState.OnLoan;

			library.Loans.Add(l);
		}
	}
}