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
    
    public partial class RoleRight
    {
        public System.Guid Id { get; set; }
        public System.Guid RoleId { get; set; }
        public System.Guid ResourceId { get; set; }
        public bool Active { get; set; }
        public byte[] RowVersion { get; set; }
    
        public virtual Resource Resource { get; set; }
        public virtual Role Role { get; set; }
    }
}
