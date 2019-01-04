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
    
    public partial class Resource
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Resource()
        {
            this.Resource1 = new HashSet<Resource>();
            this.RoleRight = new HashSet<RoleRight>();
        }
    
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> ParentId { get; set; }
        public System.Guid ModuleId { get; set; }
        public System.Guid ResourceTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public byte[] RowVersion { get; set; }
    
        public virtual Module Module { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Resource> Resource1 { get; set; }
        public virtual Resource Resource2 { get; set; }
        public virtual ResourceType ResourceType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoleRight> RoleRight { get; set; }
    }
}
