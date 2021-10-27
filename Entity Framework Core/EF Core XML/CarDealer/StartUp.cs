using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            try
            {
                var db = new CarDealerContext();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                Console.WriteLine(ImportSuppliers(db, File.ReadAllText("./Datasets/suppliers.xml")));
                Console.WriteLine(ImportCustomers(db, File.ReadAllText("./Datasets/customers.xml")));
                Console.WriteLine(ImportParts(db, File.ReadAllText("./Datasets/parts.xml")));
                Console.WriteLine(ImportCars(db, File.ReadAllText("./Datasets/cars.xml")));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            

        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            return null;
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportPartDto[]), new XmlRootAttribute("Parts"));            
            var importPartDtoData = serializer.Deserialize(new StringReader(inputXml)) as ImportPartDto[];

            var parts = importPartDtoData.Select(x => new Part
            {
                Name = x.Name,
                Price = x.Price,
                Quantity=x.Quantity,
                SupplierId=x.SupplierId

            }).ToArray();

            //get all suppliers and check if supplier exists in part data
            List<int> allSuppliersIds = context.Suppliers.Select(x =>  x.Id ).ToList();

            //filter parts
            var validParts = new List<Part>();
            foreach (var partItem in parts)
            {
                if (allSuppliersIds.Contains(partItem.SupplierId))
                {
                    validParts.Add(partItem);
                }                
            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();
            return $"Successfully imported {validParts.Count()}";
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