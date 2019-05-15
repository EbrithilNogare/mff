using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryModel {

	public class Book {
		public string Isbn { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public string Shelf { get; set; }
		public DateTime DatePublished { get; set; }
		public List<Copy> Copies { get; private set; }

		public Book() {
			Copies = new List<Copy>();
		}
	}

	public enum CopyState {
		InShelf, OnLoan, Lost
	};

	public class Copy {
		public string Id { get; set; }
		public Book Book { get; set; }
		public CopyState State { get; set; }
		public Loan OnLoan { get; set; }
	}

	public class Loan {
		public Copy Copy { get; set; }
		public Client Client { get; set; }
		public DateTime DueDate { get; set; }
	}

	public class Client {
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}

	public class Library {
		public List<Book> Books { get; private set; }
		public List<Copy> Copies { get; private set; }
		public List<Loan> Loans { get; private set; }
		public List<Client> Clients { get; private set; }

		public Library() {
			Books = new List<Book>();
			Copies = new List<Copy>();
			Loans = new List<Loan>();
			Clients = new List<Client>();
		}
	}

}
