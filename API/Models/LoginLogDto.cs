using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Common.Utilities;
using Entities.Framework;
using WebFramework.Api;
using WebFramework.CustomMapping;

namespace API.Models
{
    public class LoginLogDto : BaseDto<LoginLogDto, LoginLog, int>
    {
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        [Display(Name = "رمز عبور اشتباه")]
        public string InvalidPassword { get; set; }
        [Display(Name = "مرورگر")]
        public string Browser { get; set; }
        [Display(Name = "آی پی")]
        public string Ip { get; set; }
        [Display(Name = "نام دستگاه")]
        public string MachineName { get; set; }
        [Display(Name = "زمان")]
        public string DateTime { get; set; }

        public override void CustomMappings(IMappingExpression<LoginLog, LoginLogDto> mapping)
        {
            mapping.ForMember(
                dest => dest.DateTime,
                config => config.MapFrom(src => src.InsertDateTime.ToPerDateTimeString("yyyy/MM/dd HH:mm:ss"))
            );
        }
    }

    public class LoginLogChartDto
    {
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        [Display(Name = "آی پی")]
        public string Ip { get; set; }
        [Display(Name = "رمز عبور اشتباه")]
        public string InvalidPassword { get; set; }
    }
}
