using System.ComponentModel.DataAnnotations.Schema;
using Entities.Framework;
using FluentValidation;

namespace Entities.Tracking
{
    public abstract class ProductionBaseEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }

    }

    public class Factory : ProductionBaseEntity
    {
        public bool IsMother { get; set; }
        public string Address { get; set; }
    }

    public class FactoryValidator : AbstractValidator<Factory>
    {
        public FactoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public class ProductLine : ProductionBaseEntity
    {
        public int FactoryId { get; set; }
        [ForeignKey("FactoryId")]
        public Factory Factory { get; set; }
        public bool IsMother { get; set; }
    }
    public class ProductLineValidator : AbstractValidator<ProductLine>
    {
        public ProductLineValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public class OperationStation : ProductionBaseEntity
    {
        public int ProductLineId { get; set; }
        [ForeignKey("ProductLineId")]
        public ProductLine ProductLine { get; set; }
        public int Order { get; set; }
        public float Vas { get; set; }
    }
    public class OperationStationValidator : AbstractValidator<OperationStation>
    {
        public OperationStationValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }

    public class Machine : ProductionBaseEntity
    {
        public int OperationStationId { get; set; }
        [ForeignKey("OperationStationId")]
        public OperationStation OperationStation { get; set; }
    }
    public class MachineValidator : AbstractValidator<Machine>
    {
        public MachineValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
