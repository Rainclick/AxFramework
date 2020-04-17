using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Framework
{
    public class AuditTable : BaseEntity
    {
        public string TableName { get; set; }
        public bool Active { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }

    public class Audit : BaseEntity
    {
        public string TableName { get; set; }
        public int PrimaryKey { get; set; }
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public AuditType AuditType { get; set; }
        public string Value { get; set; }
        public DateTime EntityInsertDateTime { get; set; }
    }

    public enum AuditType
    {
        Add = 1,
        Update = 2,
        Delete = 3
    }
}
