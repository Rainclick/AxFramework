using System;
using System.Collections.Generic;
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
        public AuditType HistoryType { get; set; }
        public string Value { get; set; }
        public ICollection<AuditDetail> AuditDetails { get; set; }
    }

    public class AuditDetail : BaseEntity
    {
        public string ColumnName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public int AuditId { get; set; }
        [ForeignKey("AuditId")]
        public Audit Audit { get; set; }
    }

    public enum AuditType
    {
        Add = 1,
        Edit = 2,
        Remove = 3
    }
}
