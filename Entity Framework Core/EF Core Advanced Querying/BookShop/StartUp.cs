namespace BookShop
{
    using Data;
    using Initializer;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            Console.Write("Enum age restriction: ");
            var input = Console.ReadLine();
            Console.WriteLine(GetBooksByAgeRestriction(db, input));

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
