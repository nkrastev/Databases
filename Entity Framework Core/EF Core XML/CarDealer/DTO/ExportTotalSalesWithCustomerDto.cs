using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DTO
{
    [XmlType("customer")]
    public class ExportTotalSalesWithCustomerDto
    {
        [XmlAttribute("full-name")]
        public string CustomerName { get; set; }

        [XmlAttribute("bought-cars")]
        public int BoughtCars { get; set; }

        [XmlAttribute("spent-money")]
        public decimal SpentMoney { get; set; }
    }

    //CustomerName = x.Name,
    //                BoughtCars = x.Sales.Count(),
    //                SpentMoney=x.Sales.Sum(y=>y.Car.PartCars.Sum(z=>z.Part.Price))
}
