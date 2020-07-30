using FluentValidation;

namespace Entities.Framework
{
    public class Geo : BaseEntity
    {
        public int? ParentId { get; set; }
        public Geo Parent { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
    
    }

    public class GeoValidator : AbstractValidator<Geo> { }
}
