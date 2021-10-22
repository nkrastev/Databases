using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var db = new CarDealerContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Console.WriteLine(ImportSuppliers(db, File.ReadAllText("./Datasets/suppliers.xml")));
            Console.WriteLine(ImportCustomers(db, File.ReadAllText("./Datasets/customers.xml")));
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCustomerDto[]), new XmlRootAttribute("Customers"));
            var textRead = new StringReader(inputXml);
            var importCustomerDtoData = serializer.Deserialize(textRead) as ImportCustomerDto[];

            var customers = importCustomerDtoData.Select(x => new Customer
            {
                Name = x.Name,
                BirthDate = x.BirthDate,
                IsYoungDriver=x.IsYoungDriver

            }).ToArray();

            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count()}";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportSupplierDto[]), new XmlRootAttribute("Suppliers"));
            var textRead = new StringReader(inputXml);
            var importSupplierDtoData = serializer.Deserialize(textRead) as ImportSupplierDto[];

            var suppliers = importSupplierDtoData.Select(x => new Supplier
            {
                Name = x.Name,
                IsImporter = x.IsImporter,                
            }).ToArray();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers.Count()}";
        }
    }
}