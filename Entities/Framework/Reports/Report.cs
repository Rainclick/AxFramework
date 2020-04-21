using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Framework.Reports
{
    public class Report : BaseEntity
    {
        public string Title { get; set; }

        [NotMapped]
        public Func< object> Action { get; set; }

        public T Execute<T>()
        {
            return (T)Action.Invoke();
        }
    }

    public class Column
    {
        public string Title { get; set; }
        public ColumnType ColumnType { get; set; }
    }

    public enum ColumnType
    {
        String,
        Datetime,
        Number,
        Boolean
    }
}
