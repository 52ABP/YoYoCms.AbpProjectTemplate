using System;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities;

namespace YoYoCms.AbpProjectTemplate.Storage
{
     public class BinaryObject : Entity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        [Required]
        public virtual byte[] Bytes { get; set; }

        public BinaryObject()
        {
            Id = Guid.NewGuid();
        }

        public BinaryObject(int? tenantId, byte[] bytes)
            : this()
        {
            TenantId = tenantId;
            Bytes = bytes;
        }
    }
}
