using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Models;
using Common;
using Common.Utilities;
using Dapper;
using Dapper.Contrib.Extensions;
using Data.Repositories;
using Data.Repositories.UserRepositories;
using Entities.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Services.Services;
using WebFramework.Api;

namespace API.Controllers.v1
{
    [ApiVersion("1")]
    public class UsersController : AxController<UserDto, UserDto, AxUser, long>
    {
        private readonly IJwtService _jwtService;

        public UsersController(IUserRepository userRepository, IJwtService jwtService) : base(userRepository)
        {
            _jwtService = jwtService;
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<ApiResult<AccessToken>> AxToken(string username, string password, string device, bool isAgain)
        {
            using var qe = new QueryExecutor();
            password = password.Hash();
            var barsaUserId = await qe.Connection.ExecuteScalarAsync<long>("Select ID from Spl_Users WHERE USERNAME = @username and PASSWORD = @password", new { username, password });

            if (barsaUserId == 0)
                return new ApiResult<AccessToken>(false, ApiResultStatusCode.UnAuthorized, null, "نام کاربری و یا رمز عبور اشتباه است");

            var axUser = new AxUser { UserName = username, Password = password, Id = barsaUserId };

            var tokenBag = qe.Connection.QueryFirstOrDefault<TokenBag>("Select * from AxToken where UserId = @userId", new { userId = axUser.Id });

            if (tokenBag != null && !isAgain)
            {

                return new ApiResult<AccessToken>(false, ApiResultStatusCode.LogicError, null,
                    $"کاربر گرامی {username} عزیز دستگاه {tokenBag.Device} قبلا با کاربری شما وارد سیستم شده است در صورت ادامه آن دستگاه از سیستم خارج می گردد,آیا ادامه می دهید؟");
            }

            if (isAgain)
            {
                await DeleteToken(axUser.Id);
            }



            var token = await _jwtService.GenerateAsync(axUser);
            var tbId = qe.Connection.ExecuteScalar<long>("SELECT NEXT VALUE FOR [dbo].idseq_$1203113500000000106");
            var tb = new TokenBag
            {
                Id = tbId,
                Token = token.access_token,
                CreateDateTime = DateTime.Now,
                ExpireDateTime = DateTime.Now.AddSeconds(token.expires_in),
                UserId = axUser.Id,
                Device = device
            };
            qe.Connection.Insert(tb);
            token.userId = axUser.Id;
            return token;
        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<ApiResult<UserInfo>> GetUserInfo()
        {
            var userId = User.Identity.GetUserId<long>();
            var userInfo = new UserInfo();
            using var qe = new QueryExecutor();
            var user = await qe.Connection.QueryFirstOrDefaultAsync<UserDto>("Select DISPLAYNAME from Spl_Users where Id = @userId", new { userId });
            if (user != null)
            {
                userInfo.DisplayName = user.DisplayName;
            }
            else
                return new ApiResult<UserInfo>(false, ApiResultStatusCode.NotFound, null, "کاربری یافت نشد!");

            var loginLog = await qe.Connection.QueryFirstOrDefaultAsync<AxUserLoginLog>("Select * from AxUserLoginLogs where UserId=@userId ORDER BY [DateTime] DESC OFFSET (1) ROWS FETCH NEXT (1) ROWS ONLY", new { userId });
            if (loginLog != null)
                userInfo.LastLogin = loginLog.DateTime.ToString("yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture);

            var msgCount = await qe.Connection.ExecuteScalarAsync<int>("Select count(*) from AxNotification where UserId = @userId And IsSeen = 0", new { userId });
            userInfo.MsgCount = msgCount;
            userInfo.FileName = (string)await GetUserFile(userId, true);
            userInfo.MsgCount = msgCount;
            return userInfo;
        }

        private async Task<object> GetUserFile(long userId, bool name = false)
        {
            using var qe = new QueryExecutor();
            var personelId = await qe.Connection.ExecuteScalarAsync<long>("select Personnel from Res_PersonelRestaurantSetting WHERE UserId = @userId", new { userId });
            var attId = await qe.Connection.ExecuteScalarAsync<long>("select FILEATTACHMENTID from Shr_FileAttachmentObject WHERE OBJECTID = @personelId", new { personelId });
            var fileAttachment = await qe.Connection.QueryFirstOrDefaultAsync<FileAttachment>("select Size FROM Shr_FileAttachment WHERE id=@attId", new { attId });

            if (fileAttachment != null)
            {
                if (name)
                    return fileAttachment.Size + ".png";
                fileAttachment = await qe.Connection.QueryFirstOrDefaultAsync<FileAttachment>("select * FROM Shr_FileAttachment WHERE id=@attId", new { attId });
                if (fileAttachment?.Content?.Length > 0)
                    return fileAttachment.Content;
            }
            return null;
        }

        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<ApiResult<List<string>>> GetFooterTexts()
        {
            using var qe = new QueryExecutor();
            var data = await qe.Connection.QueryAsync<string>("Select Text from AxFooterText where Active = 1");
            var list = data.ToList();
            if (list.Any())
                return list.ToList();
            return new ApiResult<List<string>>(false, ApiResultStatusCode.NotFound, null, "کاربری یافت نشد!");

        }

        [HttpGet("[action]")]
        [Authorize]
        public async Task<ApiResult<bool>> SignOut()
        {
            var userId = User.Identity.GetUserId<long>();
            await DeleteToken(userId);
            return true;
        }

        private static async Task DeleteToken(long userId)
        {
            using var qe = new QueryExecutor();
            await qe.Connection.ExecuteAsync("Delete from AxToken where UserId = @userId", new { userId });
        }

        [HttpGet("[action]/{page}")]
        [Authorize]
        public async Task<ApiResult<IEnumerable<UserMessageDto>>> GetUserMessages(int page, CancellationToken cancellationToken)
        {
            var pageCount = 10;
            var userId = User.Identity.GetUserId<long>();
            var offset = page * pageCount;
            using var qe = new QueryExecutor();
            var data = await qe.Connection.QueryAsync<UserMessageDto>("Select * from AxNotification where UserId = @userId ORDER BY InsertDateTime DESC OFFSET (@offset) ROWS FETCH NEXT (@pageCount) ROWS ONLY", new { userId, offset, pageCount });

            return new ApiResult<IEnumerable<UserMessageDto>>(true, ApiResultStatusCode.Success, data);
        }

        [HttpGet("[action]/{msgId}/{device}")]
        [Authorize]
        public async Task<ApiResult<bool>> SetMsgSeen(long msgId, string device, CancellationToken cancellationToken)
        {
            var userId = User.Identity.GetUserId<long>();
            using var qe = new QueryExecutor();
            await qe.Connection.ExecuteAsync("  UPDATE AxNotification SET SeenDateTime = GETDATE() , IsSeen=1, SeenDevice = @device WHERE UserId = @userId and Id = @msgId", new
            {
                device,
                userId,
                msgId
            });

            return true;
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<ApiResult<bool>> SetUserGToken(Gtoken gtoken, CancellationToken cancellationToken)
        {
            var userId = User.Identity.GetUserId<long>();
            using var qe = new QueryExecutor();
            await qe.Connection.ExecuteAsync("UPDATE AxToken SET GToken = @token WHERE UserId = @userId", new { token = gtoken.Token, userId });

            return true;
        }


        [HttpGet("[action]")]
        [Authorize]
        public async Task<ApiResult<AxReserveSettings>> GetSettings(CancellationToken cancellationToken)
        {
            var userId = User.Identity.GetUserId<long>();
            using var qe = new QueryExecutor();
            var settings = await qe.Connection.QueryFirstOrDefaultAsync<AxReserveSettings>(@"SELECT
            A.Id,
            A.DefaultMealGroup,-- گروه وعده غذایی پیشفرض کاربر
            A_DefaultMealGroup.Title as FoodGroupTitle,
            A.DefaultFoodType,-- نوع غذای پیشفرض
            A_DefaultFoodType.Title as FoodTypeTitle,
            A.DefaultRestaurant,-- رستوران پیشفرض
            A_DefaultRestaurant.Title as FoodRestaurantTitle,
            A.DefaultServeMethod,-- نحوه سرو پیشفرض
            A_DefaultServeMethod.Title as ServeTypeTitle,
            A.GuestShareNum,-- سهمیه میهمان
            A.GustShareType,-- نوع محاسبه بازه سهمیه میهمان 
            A.FamilyNum, -- سهمیه اعضای خانواده
            A.FamilyShareType
                FROM
            Res_PersonelRestaurantSetting AS A left outer join Res_MealGroup A_DefaultMealGroup on (A_DefaultMealGroup.id=A.DefaultMealGroup) left outer join Res_FoodType A_DefaultFoodType on (A_DefaultFoodType.id=A.DefaultFoodType) left outer join Res_Restaurant A_DefaultRestaurant on (A_DefaultRestaurant.id=A.DefaultRestaurant) left outer join Res_ServeFoodPlace A_DefaultServeMethod on (A_DefaultServeMethod.id=A.DefaultServeMethod)
            WHERE
            A.UserId = @userId
            ", new { userId });

            if (settings != null)
                settings.Username = User.Identity.GetUserName();

            return settings;
        }

        [HttpGet("[action]/{fileName}")]
        [Authorize]
        public async Task<IActionResult> GetUserImg(string fileName)
        {
            var userId = User.Identity.GetUserId<long>();
            var content = (byte[])await GetUserFile(userId);
            if (content != null)
                return File(content, Util.GetContentType("a.png"), "a.png");
            return NotFound();
        }

    }


}