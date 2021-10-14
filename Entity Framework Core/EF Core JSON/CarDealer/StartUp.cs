using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext db = new CarDealerContext();
            
            //ResetDatabase(db);

            string inputJsonSuppliers = File.ReadAllText("..\\..\\..\\Datasets\\suppliers.json");
            string inputJsonCars = File.ReadAllText("..\\..\\..\\Datasets\\cars.json");
            string inputJsonCustomers = File.ReadAllText("..\\..\\..\\Datasets\\customers.json");
            string inputJsonParts = File.ReadAllText("..\\..\\..\\Datasets\\parts.json");
            string inputJsonSales = File.ReadAllText("..\\..\\..\\Datasets\\sales.json");

            //task 9 - 13 import
            /*Console.WriteLine(ImportSuppliers(db, inputJsonSuppliers));
            Console.WriteLine(ImportParts(db, inputJsonParts));
            Console.WriteLine(ImportCars(db, inputJsonCars));
            Console.WriteLine(ImportCustomers(db, inputJsonCustomers));            
            Console.WriteLine(ImportSales(db, inputJsonSales));*/

            //task 14-19 export
            //Console.WriteLine(GetOrderedCustomers(db));
            //Console.WriteLine(GetCarsFromMakeToyota(db));
            //Console.WriteLine(GetLocalSuppliers(db));
            //Console.WriteLine(GetCarsWithTheirListOfParts(db));
            //Console.WriteLine(GetTotalSalesByCustomer(db));
            Console.WriteLine(GetSalesWithAppliedDiscount(db));






        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context
                .Sales
                .Take(10)
                .Select(x => new
                {
                    car = new { Make = x.Car.Make, Model = x.Car.Model, TravelledDistance = x.Car.TravelledDistance },
                    customerName = x.Customer.Name,
                    Discount = x.Discount.ToString("f2"),
                    price = (x.Car.Sales.Sum(y => y.Car.PartCars.Sum(z => z.Part.Price))).ToString("f2"),
                    priceWithDiscount = 
                    (x.Car.Sales.Sum(y => y.Car.PartCars.Sum(z => z.Part.Price))-x.Car.Sales.Sum(y => y.Car.PartCars.Sum(z => z.Part.Price))*(x.Discount/100)).ToString("f2")
                })
                .ToList();

            string output = JsonConvert.SerializeObject(sales, Formatting.Indented);
            return output;
        }

        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context
                .Customers
                .Where(x => x.Sales.Any(y=>y.CustomerId==x.Id))
                .Select(x => new
                {
                    fullName=x.Name,
                    boughtCars=x.Sales.Count,
                    spentMoney=x.Sales.Sum(y=>y.Car.PartCars.Sum(z=>z.Part.Price))
                })
                .OrderByDescending(x=>x.spentMoney)
                .ThenByDescending(x=>x.boughtCars)
                .ToList();

            string output = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return output;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context
                .Cars                
                .Select(x=> new
                {
                    car = new {Make= x.Make, Model =x.Model, TravelledDistance =x.TravelledDistance},
                    parts = x.PartCars.Select(y=> new { Name =y.Part.Name, Price = $"{y.Part.Price:f2}"})

                })
                .ToList();

            string output = JsonConvert.SerializeObject(cars, Formatting.Indented);
            return output;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context
                .Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count
                })
                .ToList();

            string output = JsonConvert.SerializeObject(suppliers, Formatting.Indented);
            return output;
        }

        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var toyotas = context
                .Cars
                .Where(x => x.Make == "Toyota")
                .Select(x => new
                {
                    Id = x.Id,
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance
                })
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .ToList();

            string output = JsonConvert.SerializeObject(toyotas, Formatting.Indented);
            return output;
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context
                .Customers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.IsYoungDriver)
                .Select(x => new
                {
                    Name = x.Name,
                    BirthDate = x.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = x.IsYoungDriver
                })                
                .ToList();
            string output = JsonConvert.SerializeObject(customers, Formatting.Indented);
            return output;
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            List<Sale> sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);
            context.Sales.AddRange(sales);
            context.SaveChanges();
            return $"Successfully imported {sales.Count}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            List<Part> parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);
            
            var suppliers = context.Suppliers.Select(s => s.Id);
            parts = parts.Where(p => suppliers.Any(s => s == p.SupplierId)).ToList();
            
            context.Parts.AddRange(parts);
            context.SaveChanges();
            return $"Successfully imported {parts.Count}.";
        }

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson);
            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Count}.";
        }

        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carsInput = JsonConvert.DeserializeObject<List<ImportCarDTO>>(inputJson);

            List<Car> listOfcars = new List<Car>();
            foreach (var carJson in carsInput)
            {
                Car car = new Car()
                {
                    Make = carJson.Make,
                    Model = carJson.Model,
                    TravelledDistance = carJson.TravelledDistance
                };
                foreach (var partId in carJson.PartsId.Distinct())
                {
                    car.PartCars.Add(new PartCar()
                    {
                        Car = car,
                        PartId = partId
                    });
                }
                listOfcars.Add(car);
            }

            context.Cars.AddRange(listOfcars);
            context.SaveChanges();
            return $"Successfully imported {listOfcars.Count()}.";

        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            List<Supplier> suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers.Count()}.";
        }



        private static void ResetDatabase(CarDealerContext db)
        {
            db.Database.EnsureDeleted();
            Console.WriteLine("Database was successfully deleted!");

            db.Database.EnsureCreated();
            Console.WriteLine("Database was successfully created!");
        }        
    }
}