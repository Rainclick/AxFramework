using System;
using System.ComponentModel.DataAnnotations.Schema;
using Entities.Framework;
using FluentValidation;

namespace Entities.Tracking
{
    public class ProductInstance : BaseEntity
    {
        public long Code { get; set; }
        public int UserId { get; set; }
        public int PersonnelId { get; set; }
        [ForeignKey("PersonnelId")]
        public Personnel Personnel { get; set; }
        public int OpId { get; set; }
        [ForeignKey("OpId")]
        public OperationStation OperationStation { get; set; }
        public int ProductLineId { get; set; }
        [ForeignKey("ProductLineId")]
        public ProductLine ProductLine { get; set; }
    }

    public class ProductInstanceValidator : AbstractValidator<ProductInstance>
    {
        public ProductInstanceValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
