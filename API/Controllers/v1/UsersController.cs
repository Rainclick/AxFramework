using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            dynamic data = new { un = username, pwd = password };
            Model<Token> resInvoke = WebServiceConsumer.Invoke<Model<Token>>("http://185.211.57.94/api/auth/ServiceLogin3", data, MethodType.Post, null);

            if (!resInvoke.succeed)
                return new ApiResult<AccessToken>(false, ApiResultStatusCode.UnAuthorized, null, resInvoke.error.userMessage);
            Model<string> barsaUser;
            try
            {
                var options = new RequestOptions
                {
                    Authorization = resInvoke.data.token,
                    Headers = new Dictionary<string, string> { { "sth", resInvoke.data.sth } }
                };
                barsaUser = WebServiceConsumer.Invoke<Model<string>>($"http://185.211.57.94/api/custom/checkUser?username={username}&password={password}", null, MethodType.Get, options);
            }
            catch
            {
                return new ApiResult<AccessToken>(false, ApiResultStatusCode.UnAuthorized, null, "خطا در SSO برسا نوین رای");
            }

            if (barsaUser == null)
                return new ApiResult<AccessToken>(false, ApiResultStatusCode.UnAuthorized, null, "نام کاربری و یا رمز عبور اشتباه است");

            var axUser = new AxUser { UserName = username, Password = password };
            using var qe = new QueryExecutor();
            var tokenBag = qe.Connection.QueryFirstOrDefault<TokenBag>("Select * from AxToken where username = @username", new { username });

            var requestOptions = new RequestOptions
            {
                Authorization = resInvoke.data.token,
                Headers = new Dictionary<string, string> { { "sth", resInvoke.data.sth } }
            };

            var res = WebServiceConsumer.Invoke<Model<dynamic>>("http://185.211.57.94/api/auth/logout", null, MethodType.Post, requestOptions);
            if (!res.succeed)
                return new ApiResult<AccessToken>(false, ApiResultStatusCode.UnAuthorized, null, "مشکل از logout برسا");

            if (tokenBag != null && !isAgain)
            {
         
                return new ApiResult<AccessToken>(false, ApiResultStatusCode.LogicError, null,
                    $"کاربر گرامی {tokenBag.UserName} عزیز دستگاه {tokenBag.Device} قبلا با کاربری شما وارد سیستم شده است در صورت ادامه آن دستگاه از سیستم خارج می گردد,آیا ادامه می دهید؟");
            }

            if (isAgain)
            {
                await DeleteToken(username);
            }

            var token = await _jwtService.GenerateAsync(axUser);
            var tbId = qe.Connection.ExecuteScalar<long>("SELECT NEXT VALUE FOR [dbo].idseq_$1203113500000000106");
            var tb = new TokenBag
            {
                Id = tbId,
                Token = token.access_token,
                CreateDateTime = DateTime.Now,
                ExpireDateTime = DateTime.Now.AddSeconds(token.expires_in),
                UserName = username,
                Device = device
            };
            qe.Connection.Insert(tb);
            token.userId = long.Parse(barsaUser.data);
            return token;
        }

        [HttpGet("[action]/{userId}")]
        public async Task<ApiResult<UserInfo>> GetUserInfo(long userId)
        {
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

            userInfo.MsgCount = 2;
            return userInfo;
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
        public async Task<ApiResult<bool>> SignOut()
        {
            var username = User.Identity.GetUserName();
            await DeleteToken(username);
            return true;
        }

        private static async Task DeleteToken(string username)
        {
            using var qe = new QueryExecutor();
            await qe.Connection.ExecuteAsync("Delete from AxToken where username = @username", new {username});
        }
    }


}