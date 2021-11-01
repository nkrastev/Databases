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
            //Connection to DB
            using IDbConnection connection = new SqlConnection(Configuration.ConnectionString);

            //Age Restriction            
            /*string input = Console.ReadLine();
            Console.WriteLine(GetBooksByAgeRestriction(connection, input));*/

            //Book Titles by Category
            string input = Console.ReadLine();
            Console.WriteLine(GetBooksByAgeRestriction(connection, input));

        }

        public static string GetBooksByCategory(IDbConnection context, string input)
        {
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
