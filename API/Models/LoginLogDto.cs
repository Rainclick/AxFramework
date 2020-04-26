using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Common.Utilities;
using Entities.Framework;
using Microsoft.VisualBasic.CompilerServices;
using WebFramework.Api;

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

    public class LoginLogChartDto : BaseDto<LoginLogChartDto, LoginLog, int>
    {
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }
        [Display(Name = "آی پی")]
        public string Ip { get; set; }

        [Display(Name = "زمان", Description = "DateTime")]
        public string DateTime { get; set; }

        public override void CustomMappings(IMappingExpression<LoginLog, LoginLogChartDto> mapping)
        {
            mapping.ForMember(
                dest => dest.DateTime,
                config => config.MapFrom(src => src.InsertDateTime.ToPerDateTimeString("yyyy/MM/dd HH:mm:ss"))
            );
            mapping.ForMember(dest => dest.Id, config => config.Ignore());
        }
    }
}
