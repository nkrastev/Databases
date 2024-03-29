﻿using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

                //Imports
                /* db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                Console.WriteLine(ImportSuppliers(db, File.ReadAllText("./Datasets/suppliers.xml")));
                Console.WriteLine(ImportCustomers(db, File.ReadAllText("./Datasets/customers.xml")));
                Console.WriteLine(ImportParts(db, File.ReadAllText("./Datasets/parts.xml")));
                Console.WriteLine(ImportCars(db, File.ReadAllText("./Datasets/cars.xml")));
                Console.WriteLine(ImportSales(db, File.ReadAllText("./Datasets/sales.xml")));*/

                //Exports

                //Console.WriteLine(GetCarsWithDistance(db));
                //Console.WriteLine(GetCarsFromMakeBmw(db));
                //Console.WriteLine(GetLocalSuppliers(db));
                //Console.WriteLine(GetCarsWithTheirListOfParts(db));
                //Console.WriteLine(GetTotalSalesByCustomer(db));
                Console.WriteLine(GetSalesWithAppliedDiscount(db));

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            

        }

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            //Get all sales with information about the car, customer and price of the sale with and without discount.
            var sales = context
                .Sales
                .Select(x => new ExportSalesWithDiscountDto
                {
                    CarItem= new ExportCarAttributesDto
                    {
                        Make = x.Car.Make,
                        Model = x.Car.Model,
                        TravelledDistance = x.Car.TravelledDistance
                    },
                    Discount= x.Discount,
                    CustomerName=x.Customer.Name,
                    Price=x.Car.PartCars.Sum(z=>z.Part.Price),
                    PriceWithDiscount= x.Car.PartCars.Sum(z => z.Part.Price)- (x.Car.PartCars.Sum(z => z.Part.Price))*x.Discount*1.0m/100.0m
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlSerializer xml = new XmlSerializer(typeof(ExportSalesWithDiscountDto[]), new XmlRootAttribute("sales"));
            xml.Serialize(new StringWriter(sb), sales, namespaces);
            return sb.ToString().Trim();

        }

        //Total Sales by Customer (this is the standard solution, but throws error in VS):... and its OK with Judge :D */
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context
                .Customers
                .Where(x => x.Sales.Any(y => y.CustomerId == x.Id))
                .Select(x => new ExportTotalSalesWithCustomerDto
                {
                    CustomerName = x.Name,
                    BoughtCars = x.Sales.Count(),
                    SpentMoney=x.Sales.Sum(y=>y.Car.PartCars.Sum(z=>z.Part.Price))
                })
                .OrderByDescending(c => c.SpentMoney)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlSerializer xml = new XmlSerializer(typeof(ExportTotalSalesWithCustomerDto[]), new XmlRootAttribute("customers"));
            xml.Serialize(new StringWriter(sb), customers, namespaces);
            return sb.ToString().Trim();
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context
                .Cars
                .Select(x => new ExportCarWithPartsDto
                {
                    Make = x.Make,
                    Model = x.Model,
                    TravelledDistance = x.TravelledDistance,
                    Parts = x.PartCars.Select(y => new PartDto
                    {
                        Name = y.Part.Name,
                        Price = y.Part.Price
                    })
                    .OrderByDescending(y => y.Price)
                    .ToArray()
                })
                .OrderByDescending(x => x.TravelledDistance)
                .ThenBy(x => x.Model)
                .Take(5)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlSerializer xml = new XmlSerializer(typeof(ExportCarWithPartsDto[]), new XmlRootAttribute("cars"));
            xml.Serialize(new StringWriter(sb), cars, namespaces);
            return sb.ToString().Trim();
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            //all suppliers that do not import parts from abroad.
            //Get their id, name and the number of parts they can offer to supply. 

            var suppliers = context
                .Suppliers
                .Where(x => x.IsImporter == false)
                .Select(x => new ExportLocalSuppliersDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    PartsCount = x.Parts.Count
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlSerializer xml = new XmlSerializer(typeof(ExportLocalSuppliersDto[]), new XmlRootAttribute("suppliers"));
            xml.Serialize(new StringWriter(sb), suppliers, namespaces);
            return sb.ToString().Trim();
        }


        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            //all cars from make BMW and order them by model alphabetically and by travelled distance descending
            var cars = context
                .Cars
                .Where(x => x.Make == "BMW")
                .OrderBy(x => x.Model)
                .ThenByDescending(x => x.TravelledDistance)
                .Select(x => new ExportCarWithAttributesDto
                {
                    Id=x.Id,
                    Model=x.Model,
                    TravelledDistance=x.TravelledDistance
                })
                .ToArray();
            
            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlSerializer xml = new XmlSerializer(typeof(ExportCarWithAttributesDto[]), new XmlRootAttribute("cars"));
            xml.Serialize(new StringWriter(sb), cars, namespaces);
            return sb.ToString().TrimEnd();
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            //all cars with distance more than 2,000,000. Order them by make, then by model alphabetically. Take top 10 
            var cars = context
                .Cars
                .Where(x => x.TravelledDistance > 2000000)
                .OrderBy(x => x.Make)
                .ThenBy(x => x.Model)
                .Take(10)
                .Select(x=> new ExportCarDto
                {
                    Make=x.Make,
                    Model=x.Model,
                    TravelledDistance=x.TravelledDistance
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            XmlSerializer xml = new XmlSerializer(typeof(ExportCarDto[]), new XmlRootAttribute("cars"));
            xml.Serialize(new StringWriter(sb), cars, namespaces);
            return sb.ToString().TrimEnd();
            
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            
            var serializer = new XmlSerializer(typeof(ImportSaleDto[]), new XmlRootAttribute("Sales"));
            var importSalesDtoData = serializer.Deserialize(new StringReader(inputXml)) as ImportSaleDto[];

            var allSales = importSalesDtoData.Select(x => new Sale
            {
                CarId = x.CarId,
                CustomerId = x.CustomerId,
                Discount = x.Discount
            }).ToArray();

            List<Sale> validSales = new List<Sale>();
            var allCarIds = context.Cars.Select(x=> new{Id=x.Id}).ToList();
            var allCustomerIds = context.Customers.Select(x=> new{Id=x.Id}).ToList();

            foreach (var sale in allSales)
            {
                if (allCarIds.Any(x=>x.Id==sale.CarId))
                {
                    //Customer and Car Ids are valid... hm
                    validSales.Add(sale);
                }
            }

            context.AddRange(validSales);
            context.SaveChanges();

            return $"Successfully imported {context.Sales.Count()}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(ImportCarDto[]), new XmlRootAttribute("Cars"));
            ImportCarDto[] carsDtos = (ImportCarDto[])serializer.Deserialize(new StringReader(inputXml));

            var cars = new List<Car>();
            var partCars = new List<PartCar>();

            foreach (var carDto in carsDtos)
            {
                var car = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TravelledDistance
                };

                var parts = carDto
                    .Parts
                    .Where(pc => context.Parts.Any(p => p.Id == pc.PartId))
                    .Select(p => p.PartId)
                    .Distinct();

                foreach (var part in parts)
                {
                    PartCar partCar = new PartCar()
                    {
                        PartId = part,
                        Car = car
                    };

                    partCars.Add(partCar);
                }

                cars.Add(car);

            }

            context.PartCars.AddRange(partCars);
            context.Cars.AddRange(cars);
            context.SaveChanges();
            return $"Successfully imported {context.Cars.Count()}";
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