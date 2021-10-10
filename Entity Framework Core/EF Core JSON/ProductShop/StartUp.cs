using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new ProductShopContext();

            /*db.Database.EnsureDeleted();
            Console.WriteLine("Database was successfully deleted!");
            db.Database.EnsureCreated();
            Console.WriteLine("Database was successfully created!");
            
            string inputJsonUsers= File.ReadAllText("..\\..\\..\\Datasets\\users.json");
            string inputJsonProducts = File.ReadAllText("..\\..\\..\\Datasets\\products.json");
            string inputJsonCategories = File.ReadAllText("..\\..\\..\\Datasets\\categories.json");
            string inputJsonCategoriesProducts = File.ReadAllText("..\\..\\..\\Datasets\\categories-products.json");

            //Data Import
            //task 1
            Console.WriteLine(ImportUsers(db, inputJsonUsers));
            //task 2
            Console.WriteLine(ImportProducts(db, inputJsonProducts));
            //task 3
            Console.WriteLine(ImportCategories(db, inputJsonCategories));
            //task 4
            Console.WriteLine(ImportCategoryProducts(db, inputJsonCategoriesProducts));*/


            //task 5
            //Console.WriteLine(GetProductsInRange(db));

            //task 6
            //Console.WriteLine(GetSoldProducts(db));

            //task 7
            //Console.WriteLine(GetCategoriesByProductsCount(db));

            //task 8
            Console.WriteLine(GetUsersWithProducts(db));

        }




        //Export Methods

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            /*System.InvalidCastException : 
             * Unable to cast object of type 'System.Linq.Expressions.NewExpression' to type 
             * 'System.Linq.Expressions.MethodCallExpression'.*/

            var dbUsers = context
                .Users                
                .Where(x => x.ProductsSold.Count()>0 && x.ProductsSold.Any(y=>y.Buyer!=null))                
                .Select(x => new
                {
                    lastName=x.LastName,
                    age=x.Age,
                    soldProducts = new
                    {
                        count=x.ProductsSold.Count,
                        products =x.ProductsSold.Select(y => new { name= y.Name, price =y.Price})
                    }

                }).ToArray()
                .OrderByDescending(x => x.soldProducts.count)
                .ToList();            

            var outputObject = new
            {
                usersCount = dbUsers.Count,
                users= dbUsers
            };

            

            var output = JsonConvert.SerializeObject(outputObject, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            return output;            
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context
                .Categories
                .Select(x => new
                {
                    category = x.Name,
                    productsCount = x.CategoryProducts.Count(),
                    averagePrice = $"{x.CategoryProducts.Average(y => y.Product.Price):f2}",
                    totalRevenue = $"{x.CategoryProducts.Sum(y => y.Product.Price):f2}"
                })
                .OrderByDescending(x => x.productsCount)
                .ToList();
            string output = JsonConvert.SerializeObject(categories, Formatting.Indented);
            return output;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context
                .Users
                .Where(x => x.ProductsSold.Any(ps => ps.Buyer != null))
                .Select(x => new
                {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    soldProducts = x.ProductsSold.Select(y => new
                    {
                        name=y.Name,
                        price=y.Price,
                        buyerFirstName=y.Buyer.FirstName,
                        buyerLastName=y.Buyer.LastName
                    })
                })
                .OrderBy(x=>x.lastName)
                .ThenBy(x=>x.firstName)
                .ToList();

            string output = JsonConvert.SerializeObject(users, Formatting.Indented);
            return output;
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context
                .Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new
                {                         
                    name = x.Name,
                    price = x.Price,
                    seller = $"{x.Seller.FirstName} {x.Seller.LastName}"                   
                })
                .OrderBy(x=>x.price)
                .ToList();

            string output = JsonConvert.SerializeObject(products, Formatting.Indented);
            return output;
        }


        //Import Methods
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            List<CategoryProduct> mappingCategoryProduct = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);
            context.CategoryProducts.AddRange(mappingCategoryProduct);
            context.SaveChanges();
            return $"Successfully imported {mappingCategoryProduct.Count()}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(inputJson).Where(x=>x.Name!=null).ToList();
            context.Categories.AddRange(categories);
            context.SaveChanges();
            return $"Successfully imported {categories.Count()}";
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            List<User> users = JsonConvert.DeserializeObject<List<User>>(inputJson);
            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(inputJson);
            context.Products.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Count()}";
        }
    }
}