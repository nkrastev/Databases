using AutoMapper;
using ProductShop.Data;
using ProductShop.DTO.Input;
using ProductShop.DTO.Output;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            var db = new ProductShopContext();

            //db.Database.EnsureDeleted();
            //db.Database.EnsureCreated();

            //var usersXml = File.ReadAllText("./Datasets/users.xml");
            //var productsXml = File.ReadAllText("./Datasets/products.xml");
            //var categoriesXml = File.ReadAllText("./Datasets/categories.xml");
            //var categoriesProductsXml= File.ReadAllText("./Datasets/categories-products.xml");


            //Console.WriteLine(ImportUsers(db, usersXml));
            //Console.WriteLine(ImportProducts(db, productsXml));
            //Console.WriteLine(ImportCategories(db, categoriesXml));
            //Console.WriteLine(ImportCategoryProducts(db, categoriesProductsXml));

            //Console.WriteLine(GetProductsInRange(db));
            Console.WriteLine(GetSoldProducts(db));


        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var users = context
                .Users
                .Where(x => x.ProductsSold.Count > 0)
                .Select(x => new ExportUserSoldProducts
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold.Select(y => new ExportProductItemSold
                    {
                         Name = y.Name,
                         Price = y.Price
                     }).ToArray()
                })
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Take(5)
                .ToArray();

            //foreach (var item in users)
            //{
            //    Console.WriteLine(item);
            //}

            XmlSerializer xml = new XmlSerializer(typeof(ExportUserSoldProducts[]), new XmlRootAttribute("Users"));
            xml.Serialize(new StringWriter(sb), users, namespaces);
            return sb.ToString().TrimEnd();
            //return null;
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var products = context
                .Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .Select(x => new ExportProductsInRange
                {
                    Name = x.Name,
                    Price = x.Price,
                    Buyer = x.Buyer.FirstName + " "+x.Buyer.LastName
                })
                .OrderBy(x => x.Price)
                .Take(10)
                .ToArray();

            XmlSerializer xml = new XmlSerializer(typeof(ExportProductsInRange[]), new XmlRootAttribute("Products"));
            xml.Serialize(new StringWriter(sb), products, namespaces);
            return sb.ToString().TrimEnd();
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CategoryProductInputModel[]), new XmlRootAttribute("CategoryProducts"));
            var categoryProductsDtos = (CategoryProductInputModel[])xmlSerializer.Deserialize(new StringReader(inputXml));

            
            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();

            foreach (var categoryProductDto in categoryProductsDtos)
            {
                if (context.Categories.Any(c => c.Id == categoryProductDto.CategoryId) &&
                    context.Products.Any(p => p.Id == categoryProductDto.ProductId))
                {                   
                    CategoryProduct categoryProduct = new CategoryProduct()
                    {
                        CategoryId = categoryProductDto.CategoryId,
                        ProductId = categoryProductDto.ProductId
                    };
                    categoryProducts.Add(categoryProduct);
                }

            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(CategoryInputModel[]), new XmlRootAttribute("Categories"));
            var textRead = new StringReader(inputXml);
            var categoriesDto = serializer.Deserialize(textRead) as CategoryInputModel[];

            var categories = categoriesDto.Select(x => new Category
            {
                Name = x.Name,                
            }).ToList();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ProductInputModel[]), new XmlRootAttribute("Products"));
            var textRead = new StringReader(inputXml);
            var productsDto = serializer.Deserialize(textRead) as ProductInputModel[];

            List<Product> listOfProducts = new List<Product>();

            foreach (var productDto in productsDto)
            {

                Product product = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    SellerId = productDto.SellerId
                };
                //check
                if (productDto.BuyerId != 0)
                {
                    product.BuyerId = productDto.BuyerId;
                }
                listOfProducts.Add(product);
            }

            context.Products.AddRange(listOfProducts);
            context.SaveChanges();
            return $"Successfully imported {listOfProducts.Count()}";
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var serializer= new XmlSerializer(typeof(UserInputModel[]), new XmlRootAttribute("Users"));
            var textRead = new StringReader(inputXml);
            var usersDto = serializer.Deserialize(textRead) as UserInputModel[];

            var users = usersDto.Select(x => new User
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Age = x.Age
            }).ToList();

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count()}";
        }

        private static void InitializeMapper()
        {
            Mapper.Initialize(cfg => { cfg.AddProfile<ProductShopProfile>(); });
        }

    }
}