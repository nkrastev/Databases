using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DTO
{
    //partitial DTO for import cars with array of Parts
    [XmlType("partId")]
    public class PartsDto
    {
        [XmlAttribute("id")]
        public int PartId { get; set; }
    }
}
