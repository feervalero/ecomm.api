//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApiTest1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetail
    {
        public System.Guid Id { get; set; }
        public System.Guid OrderId { get; set; }
        public System.Guid ProductId { get; set; }
        public int QuantityRequested { get; set; }
        public string Price { get; set; }
        public string Total { get; set; }
        public string ShipmentStatus { get; set; }
        public bool Active { get; set; }
        public byte[] RowVersion { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
