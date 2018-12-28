//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ECommerceAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Inventory
    {
        public System.Guid Id { get; set; }
        public System.Guid ProductId { get; set; }
        public System.Guid StatusTypeId { get; set; }
        public Nullable<int> QuantityOnReserve { get; set; }
        public Nullable<int> QuantityAvailable { get; set; }
        public Nullable<int> MinimumQuantityAvailable { get; set; }
        public bool Active { get; set; }
        public byte[] RowVersion { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual StatusType StatusType { get; set; }
    }
}
