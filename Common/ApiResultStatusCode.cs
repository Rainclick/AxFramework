using System.ComponentModel.DataAnnotations;

namespace Common
{
    public enum ApiResultStatusCode
    {
        [Display(Name = "عملیات با موفقیت انجام شد")]
        Success = 1,
        [Display(Name = "خطایی در سرور رخ داده است")]
        ServerError = 2,
        [Display(Name = "درخواست صحیح نیست")]
        BadRequest = 3,
        [Display(Name = "داده ای یافت نشد")]
        NotFound = 4,
        [Display(Name = "نتیجه خالی است")]
        Empty = 5,
        [Display(Name = "خطای منطقی رخ داده است")]
        LogicError = 6,
        [Display(Name = "خطای احراز دسترسی")]
        UnAuthorized = 7,
        [Display(Name = "خطای احراز هویت")]
        UnAuthenticated = 8
    }
}
