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
    
    public partial class RefreshToken
    {
        public System.Guid Id { get; set; }
        public System.Guid UserId { get; set; }
        public System.Guid AudienceId { get; set; }
        public System.DateTime IssuedUtc { get; set; }
        public System.DateTime ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; }
        public bool Active { get; set; }
        public byte[] RowVersion { get; set; }
    
        public virtual Audience Audience { get; set; }
        public virtual User User { get; set; }
    }
}
