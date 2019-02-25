using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace eshop
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var sw = new StreamWriter("NezarkaTest.html"))
            {
                TextReader textReader = Console.In;
                TextWriter textWriter = Console.Out;

                //textWriter = sw; //for debug

                //load store data
                ModelStore store = ModelStore.LoadFrom(textReader);
                if (store == null)
                {
                    Console.WriteLine("Data error.");
                    Environment.Exit(0);
                    return;
                }
                HTMLBuilder htmlBuilder = new HTMLBuilder(textWriter);
                GetParser getParser = new GetParser(htmlBuilder, store);

                //commands execution
                while (true)
                {
                    string getCommand = textReader.ReadLine();
                    if (getCommand == null)
                    {
                        break;
                    }
                    getParser.parse(getCommand);
                }




            }

        }
    }

    class GetParser
    {
        private HTMLBuilder htmlBuilder;
        private ModelStore store;
        public GetParser(HTMLBuilder htmlBuilder, ModelStore store)
        {
            this.htmlBuilder = htmlBuilder;
            this.store = store;
        }

        public void parse(string request)
        {
            string[] get = request.Split(' ');
            if (!validateGet(get))
            {
                htmlBuilder.createDump();
                return;
            }
            string[] url = get[2].Split('/');
            if (!validateUrl(url))
            {
                htmlBuilder.createDump();
                return;
            }
            string subsectionType = "";
            string custId = get[1];
            int bookIdInt = 0;
            int custIdInt = 0;

            subsectionType = getSubsectionType(url, out string bookId);
            if (!validateSubsectionType(subsectionType, url))
            {
                htmlBuilder.createDump();
                return;
            }
            
            if (!(int.TryParse(bookId, out bookIdInt) && int.TryParse(custId, out custIdInt)))
            {
                htmlBuilder.createDump();
                return;
            }
            Customer customer = store.GetCustomer(custIdInt);
            Book book = store.GetBook(bookIdInt);
            if (customer == null || (book == null && bookIdInt != -5769765))
            {
                htmlBuilder.createDump();
                return;
            }

            switch (subsectionType)
            {
                case "Add":
                    if (!addItemToCart(store, bookIdInt, store.GetCustomer(custIdInt).ShoppingCart))
                    {
                        htmlBuilder.createDump();
                        return;
                    }
                    break;

                case "Remove":
                    if (!removeItemFromCart(store, bookIdInt, store.GetCustomer(custIdInt).ShoppingCart))
                    {
                        htmlBuilder.createDump();
                        return;
                    }
                    break;

                case "":
                    htmlBuilder.createDump();
                    return;
            }

            htmlBuilder.createWebPage(subsectionType, bookIdInt, int.Parse(custId), store);



        }
        /// <summary>
        /// validate get command
        /// </summary>
        /// <param name="get">get command</param>
        /// <returns>true for validade data</returns>
        private bool validateGet(string[] get)
        {
            if (get.Length != 3 || get[0] != "GET" || !int.TryParse(get[1], out int bin))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// validate url
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>true for valide data</returns>
        private bool validateUrl(string[] url)
        {
            if (!(
                    (url.Length == 4 || url.Length == 5 || url.Length == 6) &&
                    (url[0] == "http:" && url[1] == "" && url[2] == "www.nezarka.net")
                ))
                return false;
            else
                return true;
        }
        /// <summary>
        /// returns subSection type
        /// </summary>
        /// <param name="url">url to check</param>
        /// <param name="bookId">also return book Id</param>
        /// <returns>true for validate data</returns>
        private string getSubsectionType(string[] url, out string bookId)
        {
            string subsectionType = "";
            bookId = "-5769765";
            if (url[3] == "Books")
            {
                subsectionType = "Books";
                if (url.Length > 4 && url[4] == "Detail")
                {
                    subsectionType = "Detail";
                    bookId = url[5];
                }
            }

            if (url[3] == "ShoppingCart")
            {
                subsectionType = "ShoppingCart";
                if (url.Length > 4 && url[4] == "Add")
                {
                    subsectionType = "Add";
                    bookId = url[5];
                }

                if (url.Length > 4 && url[4] == "Remove")
                {
                    subsectionType = "Remove";
                    bookId = url[5];
                }
            }
            return subsectionType;
        }
        /// <summary>
        /// validate type of subSection
        /// </summary>
        /// <param name="subsectionType">checked type</param>
        /// <param name="url">for check its length</param>
        /// <returns>true for valide data</returns>
        private bool validateSubsectionType(string subsectionType, string[] url)
        {
            if (!(
                    (subsectionType == "Books" && url.Length == 4) ||
                    (subsectionType == "Detail" && url.Length == 6) ||
                    (subsectionType == "ShoppingCart" && url.Length == 4) ||
                    (subsectionType == "Add" && url.Length == 6) ||
                    (subsectionType == "Remove" && url.Length == 6)
                ))
                return false;
            else
                return true;
        }
        /// <summary>
        /// add item to cart
        /// </summary>
        /// <param name="store"></param>
        /// <param name="bookId"></param>
        /// <param name="shoppingCart"></param>
        /// <returns>true on succues</returns>
        private bool addItemToCart(ModelStore store, int bookId, ShoppingCart shoppingCart)
        {
            ShoppingCartItem shoppingCartItem = shoppingCart.Items.Find(r => r.BookId == bookId);
            if (shoppingCartItem == null)
            {
                shoppingCart.Items.Add(new ShoppingCartItem
                {
                    BookId = bookId,
                    Count = 1
                });
            }
            else
            {
                shoppingCartItem.Count++;
            }
            return true;
        }
        /// <summary>
        /// remove one piece from cart or remove it if there left only one
        /// </summary>
        /// <param name="store"></param>
        /// <param name="bookId"></param>
        /// <param name="shoppingCart"></param>
        /// <returns>true on succues</returns>
        private bool removeItemFromCart(ModelStore store, int bookId, ShoppingCart shoppingCart)
        {
            ShoppingCartItem shoppingCartItem = shoppingCart.Items.Find(r => r.BookId == bookId);
            if (shoppingCartItem == null)
            {
                return false;
            }
            if (shoppingCartItem.Count == 1)
            {
                shoppingCart.Items.Remove(shoppingCartItem);
            }
            if (shoppingCartItem.Count < 1)
            {
                return false;
            }
            else
            {
                shoppingCartItem.Count--;
            }
            return true;
        }
    }

    class HTMLBuilder
    {
        private TextWriter writer;
        public HTMLBuilder(TextWriter writer)
        {
            this.writer = writer;
        }
        public void createWebPage(string subsectionType, int bookId, int custId, ModelStore store)
        {
            createHtml();
            createHead();
            createBody(subsectionType, bookId, custId, store);
            endHTML();
        }

        private void createHtml()
        {
            writer.WriteLine("<!DOCTYPE html>");
            writer.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
        }
        private void createHead()
        {
            writer.WriteLine("<head>");
            writer.WriteLine(ind(1) + "<meta charset=\"utf-8\" />");
            writer.WriteLine(ind(1) + "<title>Nezarka.net: Online Shopping for Books</title>");
            writer.WriteLine("</head>");
        }

        private void createBody(string subsectionType, int bookId, int custId, ModelStore store)
        {
            var name = store.GetCustomer(custId).FirstName;

            writer.WriteLine("<body>");

            writer.WriteLine(ind(1) + "<style type=\"text/css\">");
            writer.WriteLine(ind(2) + "table, th, td {");
            writer.WriteLine(ind(3) + "border: 1px solid black;");
            writer.WriteLine(ind(3) + "border-collapse: collapse;");
            writer.WriteLine(ind(2) + "}");
            writer.WriteLine(ind(2) + "table {");
            writer.WriteLine(ind(3) + "margin-bottom: 10px;");
            writer.WriteLine(ind(2) + "}");
            writer.WriteLine(ind(2) + "pre {");
            writer.WriteLine(ind(3) + "line-height: 70%;");
            writer.WriteLine(ind(2) + "}");
            writer.WriteLine(ind(1) + "</style>");

            writer.WriteLine(ind(1) + "<h1><pre>  v,<br />Nezarka.NET: Online Shopping for Books</pre></h1>");

            writer.WriteLine(ind(1) + "{0}, here is your menu:", name);
            writer.WriteLine(ind(1) + "<table>");
            writer.WriteLine(ind(2) + "<tr>");
            writer.WriteLine(ind(3) + "<td><a href=\"/Books\">Books</a></td>");
            writer.WriteLine(ind(3) + "<td><a href=\"/ShoppingCart\">Cart ({0})</a></td>", store.GetCustomer(custId).ShoppingCart.Items.Count);
            writer.WriteLine(ind(2) + "</tr>");
            writer.WriteLine(ind(1) + "</table>");

            createSubsection(subsectionType, bookId, custId, store);

            writer.WriteLine("</body>");


        }

        void createSubsection(string subsectionType, int bookId, int custId, ModelStore store)
        {
            switch (subsectionType)
            {
                case "Books":
                    showBooks(store.GetBooks());
                    break;
                case "Detail":
                    showDetail(store.GetBook(bookId));
                    break;
                case "Add":
                case "Remove":
                case "ShoppingCart":
                    showShoppingCart(store.GetCustomer(custId).ShoppingCart, store);
                    break;
            }
        }
        private void showBooks(IList<Book> books)
        {
            writer.WriteLine(ind(1) + "Our books for you:");


            writer.WriteLine(ind(1) + "<table>");
            for (int i = 0; i < books.Count; i++)
            {
                if (i % 3 == 0)
                {
                    writer.WriteLine(ind(2) + "<tr>");
                }

                writer.WriteLine(ind(3) + "<td style=\"padding: 10px;\">");
                writer.WriteLine(ind(4) + "<a href=\"/Books/Detail/{1}\">{0}</a><br />", books[i].Title, books[i].Id);
                writer.WriteLine(ind(4) + "Author: {0}<br />", books[i].Author);
                writer.WriteLine(ind(4) + "Price: {0} EUR &lt;<a href=\"/ShoppingCart/Add/{1}\">Buy</a>&gt;", books[i].Price, books[i].Id);
                writer.WriteLine(ind(3) + "</td>");

                if (i % 3 == 2 || i + 1 == books.Count)
                {
                    writer.WriteLine(ind(2) + "</tr>");
                }
            }
            writer.WriteLine(ind(1) + "</table>");

        }
        private void showDetail(Book book)
        {
            writer.WriteLine(ind(1) + "Book details:");
            writer.WriteLine(ind(1) + "<h2>{0}</h2>", book.Title);
            writer.WriteLine(ind(1) + "<p style=\"margin-left: 20px\">");
            writer.WriteLine(ind(1) + "Author: {0}<br />", book.Author);
            writer.WriteLine(ind(1) + "Price: {0} EUR<br />", book.Price);
            writer.WriteLine(ind(1) + "</p>");
            writer.WriteLine(ind(1) + "<h3>&lt;<a href=\"/ShoppingCart/Add/{0}\">Buy this book</a>&gt;</h3>", book.Id);
        }
        private void showShoppingCart(ShoppingCart shoppingCart, ModelStore store)
        {
            if (shoppingCart.Items.Count == 0)
            {
                writer.WriteLine(ind(1) + "Your shopping cart is EMPTY.");
                return;
            }



            int price = 0;

            writer.WriteLine(ind(1) + "Your shopping cart:");
            writer.WriteLine(ind(1) + "<table>");
            writer.WriteLine(ind(2) + "<tr>");
            writer.WriteLine(ind(3) + "<th>Title</th>");
            writer.WriteLine(ind(3) + "<th>Count</th>");
            writer.WriteLine(ind(3) + "<th>Price</th>");
            writer.WriteLine(ind(3) + "<th>Actions</th>");
            writer.WriteLine(ind(2) + "</tr>");

            for (int i = 0; i < shoppingCart.Items.Count; i++)
            {
                int bookId = shoppingCart.Items[i].BookId;
                int count = shoppingCart.Items[i].Count;
                Book book = store.GetBook(bookId);
                int totalPrice = count * book.Price;

                writer.WriteLine(ind(2) + "<tr>");
                writer.WriteLine(ind(3) + "<td><a href=\"/Books/Detail/{0}\">{1}</a></td>", book.Id, book.Title);
                writer.WriteLine(ind(3) + "<td>{0}</td>", count);
                if (count == 1)
                {
                    writer.WriteLine(ind(3) + "<td>{0} EUR</td>", book.Price);
                }
                else
                {
                    writer.WriteLine(ind(3) + "<td>{0} * {1} = {2} EUR</td>", count, book.Price, totalPrice);
                }
                writer.WriteLine(ind(3) + "<td>&lt;<a href=\"/ShoppingCart/Remove/{0}\">Remove</a>&gt;</td>", book.Id);
                writer.WriteLine(ind(2) + "</tr>");

                price += totalPrice;
            }




            writer.WriteLine(ind(1) + "</table>");
            writer.WriteLine(ind(1) + "Total price of all items: {0} EUR", price);


        }
        private void endHTML()
        {
            writer.WriteLine("</html>");
            writer.WriteLine("====");
        }

        /// <summary>
        /// return number of spaces for indenting HTML
        /// </summary>
        /// <param name="i">height of indent</param>
        /// <returns>tab for each indent</returns>
        private string ind(int i)
        {
            return new string('	', i);
        }
        /// <summary>
        /// is called on invalid request, contain html, head and body
        /// </summary>
        public void createDump()
        {

            createHtml();
            createHead();
            writer.WriteLine("<body>");
            writer.WriteLine("<p>Invalid request.</p>");
            writer.WriteLine("</body>");
            endHTML();
        }
    }

    /// <summary>
    /// do not tuch things below this comment !
    /// </summary>
    class ModelStore
    {
        private List<Book> books = new List<Book>();
        private List<Customer> customers = new List<Customer>();

        public IList<Book> GetBooks()
        {
            return books;
        }

        public Book GetBook(int id)
        {
            return books.Find(b => b.Id == id);
        }

        public Customer GetCustomer(int id)
        {
            return customers.Find(c => c.Id == id);
        }

        public static ModelStore LoadFrom(TextReader reader)
        {
            var store = new ModelStore();

            try
            {
                if (reader.ReadLine() != "DATA-BEGIN")
                {
                    return null;
                }
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        return null;
                    }
                    else if (line == "DATA-END")
                    {
                        break;
                    }

                    string[] tokens = line.Split(';');
                    switch (tokens[0])
                    {
                        case "BOOK":
                            store.books.Add(new Book
                            {
                                Id = int.Parse(tokens[1]),
                                Title = tokens[2],
                                Author = tokens[3],
                                Price = int.Parse(tokens[4])
                            });
                            break;
                        case "CUSTOMER":
                            store.customers.Add(new Customer
                            {
                                Id = int.Parse(tokens[1]),
                                FirstName = tokens[2],
                                LastName = tokens[3]
                            });
                            break;
                        case "CART-ITEM":
                            var customer = store.GetCustomer(int.Parse(tokens[1]));
                            if (customer == null)
                            {
                                return null;
                            }
                            customer.ShoppingCart.Items.Add(new ShoppingCartItem
                            {
                                BookId = int.Parse(tokens[2]),
                                Count = int.Parse(tokens[3])
                            });
                            break;
                        default:
                            return null;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is IndexOutOfRangeException)
                {
                    return null;
                }
                throw;
            }

            return store;
        }
    }

    class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Price { get; set; }
    }

    class Customer
    {
        private ShoppingCart shoppingCart;

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ShoppingCart ShoppingCart
        {
            get
            {
                if (shoppingCart == null)
                {
                    shoppingCart = new ShoppingCart();
                }
                return shoppingCart;
            }
            set
            {
                shoppingCart = value;
            }
        }
    }

    class ShoppingCartItem
    {
        public int BookId { get; set; }
        public int Count { get; set; }
    }

    class ShoppingCart
    {
        public int CustomerId { get; set; }
        public List<ShoppingCartItem> Items = new List<ShoppingCartItem>();
    }
}
