using P03_SalesDatabase.Data;
using P03_SalesDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace P03_SalesDatabase
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new SalesContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            FillData(context);
        }

        public static void FillData(SalesContext context)
        {
            var stores = context.Stores.ToList();
            Store store = new Store();
            store.Name = "Store";
            context.Stores.Add(store);

            var products = context.Products.ToList();
            Product product = new Product();
            product.Name = "Product";
            context.Products.Add(product);

            var customers = context.Customers.ToList();
            Customer customer = new Customer();
            customer.Name = "Dimitri4ko";
            customer.Email = "test";
            context.Customers.Add(customer);


            var sales = context.Sales.ToList();
            Sale sale = new Sale();
            sale.CustomerId = 1;
            sale.ProductId = 1;
            sale.StoreId = 1;

            context.Sales.Add(sale);

            context.SaveChanges();                      
        }
    }


}
