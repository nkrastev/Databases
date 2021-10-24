using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DTO
{
    [XmlType("Part")]
    public class ImportPartDto
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("price")]
        public decimal Price { get; set; }

        [XmlElement("quantity")]
        public int Quantity { get; set; }

        [XmlElement("supplierId")]
        public int SupplierId { get; set; }
    }

    //<Part>
    //    <name>Unexposed bumper</name>
    //    <price>1003.34</price>
    //    <quantity>10</quantity>
    //    <supplierId>12</supplierId>
    //</Part>
}
