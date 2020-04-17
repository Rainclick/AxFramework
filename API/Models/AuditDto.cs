using AutoMapper;
using Common.Utilities;
using Entities.Framework;
using WebFramework.Api;

namespace API.Models
{
    public class AuditDto : BaseDto<AuditDto, Audit, int>
    {
        public string TableName { get; set; }
        public int PrimaryKey { get; set; }
        public string UserDisplay { get; set; }
        public string AuditTypeDisplay { get; set; }


        public override void CustomMappings(IMappingExpression<Audit, AuditDto> mapping)
        {
            mapping.ForMember(
                dest => dest.UserDisplay,
                config => config.MapFrom(src => src.User.FirstName + " " + src.User.LastName)
            );
            mapping.ForMember(
                dest => dest.AuditTypeDisplay,
                config => config.MapFrom(src => src.AuditType.ToDisplay(DisplayProperty.Name))
            );
        }
    }
}
