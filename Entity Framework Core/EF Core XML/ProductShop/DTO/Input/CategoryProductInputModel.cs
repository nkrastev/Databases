using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.DTO.Input
{
    [XmlType("CategoryProduct")]
    public class CategoryProductInputModel
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
    }

    //<CategoryProduct>
    //    <CategoryId>6</CategoryId>
    //    <ProductId>3</ProductId>
    //</CategoryProduct>
}
