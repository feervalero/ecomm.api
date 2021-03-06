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
    
    public partial class Payment
    {
        public System.Guid Id { get; set; }
        public System.Guid OrderId { get; set; }
        public System.Guid PaymentMethodId { get; set; }
        public string PaymentIndicator { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
        public string Installments { get; set; }
        public bool Active { get; set; }
        public byte[] RowVersion { get; set; }
    
        public virtual Order Order { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
    }
}
