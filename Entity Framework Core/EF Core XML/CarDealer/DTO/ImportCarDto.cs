using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DTO
{
    [XmlType("Car")]
    public class ImportCarDto
    {
        [XmlElement("make")]
        public string Make { get; set; }

        [XmlElement("model")]
        public string Model { get; set; }

        [XmlElement("TraveledDistance")]
        public long TravelledDistance { get; set; }

        [XmlArray("parts")]
        public PartsDto[] Parts { get; set; }
    }

    //  <Car>
    //  <make>Opel</make>
    //  <model>Astra</model>
    //  <TraveledDistance>516628215</TraveledDistance>
    //  <parts>
    //    <partId id = "39" />
    //    < partId id="62"/>
    //    <partId id = "72" />
    //  </ parts >
    //</ Car >
}
