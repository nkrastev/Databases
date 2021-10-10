namespace BookShop
{
    using Data;
    using Initializer;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //task 1
            /*Console.Write("Enum age restriction: ");
            var input = Console.ReadLine();
            Console.WriteLine(GetBooksByAgeRestriction(db, input));*/

            //task 2
            /*Console.WriteLine(GetGoldenBooks(db));*/

            //task 3
            /*Console.WriteLine(GetBooksByPrice(db));*/

            //task 4
            /*int year = int.Parse(Console.ReadLine());//TODO check input
            Console.WriteLine(GetBooksNotReleasedIn(db, year));*/

            //task 5
            /*string input = Console.ReadLine();
            Console.WriteLine(GetBooksByCategory(db, input));*/

            //task 6
            /*string input = Console.ReadLine(); //12-04-1992 validation?
            Console.WriteLine(GetBooksReleasedBefore(db, input));*/

            //task 7
            /*string input = Console.ReadLine();
            Console.WriteLine(GetAuthorNamesEndingIn(db, input));*/

            //task 8
            /*string input = Console.ReadLine();
            Console.WriteLine(GetBookTitlesContaining(db, input));*/

            //task 9
            /*string input = Console.ReadLine();
            Console.WriteLine(GetBooksByAuthor(db, input));*/

            //task 10
            /*int lengthCheck = int.Parse(Console.ReadLine());
            Console.WriteLine(CountBooks(db, lengthCheck));*/

            //task 11
            /*Console.WriteLine(CountCopiesByAuthor(db));*/

            //task 12
            /*Console.WriteLine(GetTotalProfitByCategory(db));*/

            //task 13
            /*Console.WriteLine(GetMostRecentBooks(db));*/

            //task 14
            /*IncreasePrices(db);*/

            //task 15
            Console.WriteLine(RemoveBooks(db));


        }


        public static int RemoveBooks(BookShopContext context)
        {
            //Remove all books, which have less than 4200 copies
            var books = context.Books.Where(x => x.Copies < 4200).ToList();
            context.Books.RemoveRange(books);
            context.SaveChanges();
            return books.Count;
        }

        public static void IncreasePrices(BookShopContext context)
        {
            // Increase the prices of all books released before 2010 by 5.
            var books = context.Books.Where(x => x.ReleaseDate.Value.Year < 2010).ToList();
            foreach (var item in books)
            {
                item.Price += 5;
            }
            context.SaveChanges();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var data = context
                .Categories
                .Select(x => new
                {
                    Category = x.Name,
                    Top3Books = x.CategoryBooks.OrderByDescending(x => x.Book.ReleaseDate).Take(3).Select(y => new
                    {
                        BookTitle = y.Book.Title,
                        BookDate = y.Book.ReleaseDate.Value.Year
                    })
                })
                .OrderBy(x=>x.Category)
                .ToList();
            
            
            StringBuilder sb = new StringBuilder();
            foreach (var item in data)
            {
                sb.AppendLine("--"+item.Category);
                foreach (var subItem in item.Top3Books)
                {
                    sb.AppendLine($"{subItem.BookTitle} ({subItem.BookDate})");
                }                
            }
            return sb.ToString().TrimEnd();
        }


        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var data = context
                .Categories
                .Select(x => new
                {
                    Category = x.Name,                    
                    Profit = x.CategoryBooks
                       
                       .Select(y => new
                       {
                           ProfitPerCategory = y.Book.Copies * y.Book.Price
                       }).Sum(x=>x.ProfitPerCategory)
                })
                .OrderByDescending(x=>x.Profit)
                .ThenBy(x=>x.Category)
                .ToList();


            StringBuilder sb = new StringBuilder();
            foreach (var item in data)
            {
                sb.AppendLine($"{item.Category} ${item.Profit:f2}");
            }
            return sb.ToString().TrimEnd();
        }


        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context
                .Authors
                .Select(x => new
                {
                    AuthorId = x.AuthorId,
                    AuthorName = x.FirstName + " " + x.LastName,
                    BookCopies = x.Books.Where(y => y.AuthorId == x.AuthorId).Select(z=>z.Copies)
                })
                .OrderByDescending(x=>x.BookCopies.Sum())
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var author in authors)
            {
                sb.AppendLine($"{author.AuthorName} - {author.BookCopies.Sum()}");
            }
            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {            
            return context
                .Books
                .Where(x => x.Title.Length > lengthCheck)
                .Count();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context
                .Books
                .Where(x => x.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(x => new
                {
                    Id = x.BookId,
                    Title = x.Title,
                    AuthorNames = x.Author.FirstName + " " + x.Author.LastName
                })
                .OrderBy(x => x.Id)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} ({book.AuthorNames})");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context
                .Books
                .Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .Select(x => new { Title = x.Title })
                .OrderBy(x=>x.Title)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context
                .Authors
                .Where(x => x.FirstName.EndsWith(input))
                .Select(x => new
                {
                    FullName = x.FirstName + " " + x.LastName
                })
                .OrderBy(x => x.FullName)
                .ToList();
            
            StringBuilder sb = new StringBuilder();
            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FullName}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime dateTime = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context
                .Books
                .Where(x => x.ReleaseDate < dateTime)
                .Select(x=> new
                {
                    Title=x.Title,
                    EditionType=x.EditionType,
                    ReleaseDate=x.ReleaseDate,
                    Price=x.Price
                })
                .OrderByDescending(x=>x.ReleaseDate)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categoriesList = input.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
            categoriesList = categoriesList.ConvertAll(d => d.ToLower());


            var booksWithCategories = context
                .Books
                .Select(x=> new
                {
                    Title=x.Title,
                    Categories=x.BookCategories.Select(y=>y.Category.Name.ToLower()).ToList()
                })
                .ToList();
            //all books with categrories
            var result = new List<String>();
            StringBuilder sb = new StringBuilder();

            foreach (var book in booksWithCategories)
            {
                foreach (var item in categoriesList)
                {
                    if (book.Categories.Contains(item))
                    {
                        result.Add(book.Title);
                    }
                }
            }

            result.Sort();

            foreach (var book in result)
            {
                sb.AppendLine(book);
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            DateTime startDate = new DateTime(year, 1, 1, 0, 0, 0);
            DateTime endDate = new DateTime(year, 12, 31, 23, 59, 59);           

            var books = context
                .Books
                .Where(x => !(x.ReleaseDate >= startDate && x.ReleaseDate <= endDate))
                .Select(x=> new
                    {
                        Id=x.BookId,
                        Title=x.Title
                    })
                .OrderBy(x=>x.Id)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title}");
            }
            return sb.ToString().TrimEnd();
            
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context
                .Books
                .Where(x => x.Price > 40)
                .Select(x => new
                {
                    Title = x.Title,
                    Price = x.Price
                })
                .OrderByDescending(x=>x.Price)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:f2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context
                .Books
                .AsEnumerable()
                .Where(x => x.EditionType.ToString() == "Gold" && x.Copies<5000)
                .Select(x=> new 
                    {
                        BookId=x.BookId,
                        BookTitle=x.Title
                    })
                .OrderBy(x=>x.BookId)
                .ToList();

            
            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine(book.BookTitle);
            }
            return sb.ToString().TrimEnd();            
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var books = context
                .Books
                .AsEnumerable() 
                .Where(x => x.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(x => new
                    {
                        BookTitle = x.Title
                    })
                .OrderBy(x=>x.BookTitle)
                .ToList();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine(book.BookTitle);
            }
            return sb.ToString().TrimEnd();
        }
    }
}
