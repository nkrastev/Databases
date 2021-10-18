using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.DTO.Output
{
    [XmlType("Product")]
    public class ExportProductItemSold
    {
        
        [XmlElement("name")]
        public string Name { get; set; }
        
        [XmlElement("price")]
        public decimal Price { get; set; }
        
    }
}
