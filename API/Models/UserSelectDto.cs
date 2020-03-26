using System;
using AutoMapper;
using Entities.Framework;
using WebFramework.Api;

namespace API.Models
{
    public class UserSelectDto: BaseDto<UserSelectDto, User, int>
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset? LastLoginDate { get; set; }

     
        public override void CustomMappings(IMappingExpression<User, UserSelectDto> mapping)
        {
            mapping.ForMember(
                dest => dest.FullName,
                config => config.MapFrom(src => $"{src.FirstName} {src.LastName}")
            );
        }
    }
}
