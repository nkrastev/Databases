using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DTO
{
    [XmlType("Supplier")]
    public class ImportSupplierDto
    {
        [XmlElement("name")]
        public string Name { get; set; }      

        [XmlElement("isImporter")]
        public bool IsImporter { get; set; }


    //    <Supplier>
    //    <name>3M Company</name>
    //    <isImporter>true</isImporter>
    //</Supplier>
    }
}
