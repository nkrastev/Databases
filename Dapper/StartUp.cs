using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BookShop.Data;
using BookShop.Models;
using Dapper;
using System.Text;

namespace BookShop
{
    
    public class StartUp
    {
        static void Main()
        {
            try
            {
                //Connection to DB
                using IDbConnection connection = new SqlConnection(Configuration.ConnectionString);
             
                //Book Titles with Category              
                Console.WriteLine(GetBooksByCategory(connection));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            
        }

        public static string GetBooksByCategory(IDbConnection context)
        {           
            var sql = @"SELECT b.BookId, b.Title, bc.CategoryId FROM Books AS b 
                        INNER JOIN
                        BooksCategories AS bc ON bc.BookId = B.BookId ORDER BY b.BookId";
            var books = context.Query<Book, BookCategory, Book>(sql,
                (book, mappingItem) => {
                    book.bookCategories.Add(mappingItem);
                    return book;
                },
                splitOn: "CategoryId");

            foreach (var book in books)
            {                
                Console.Write("BookId: "+book.BookId+" Title: "+book.Title+" | Category Ids > ");
                foreach (var item in book.bookCategories)
                {
                    Console.Write(item.CategoryId+", ");                    
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            return null;
        }

        public static string GetBooksByAgeRestriction(IDbConnection context, string command)
        {
            StringBuilder sb = new StringBuilder();
            var books = context.Query<Book>("SELECT * FROM Books")
                .Where(x => x.AgeRestriction.ToString().ToLower() == command.ToLower())
                .Select(x=> new
                {
                    Title=x.Title
                })
                .OrderBy(x=>x.Title)
                .ToList();

            foreach (var item in books)
            {
                sb.AppendLine(item.Title);
            }
            return sb.ToString().TrimEnd();
        }

    }  
    
}
