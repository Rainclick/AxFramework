using System;
using System.Collections.Generic;
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
        public async Task<ApiResult<AccessToken>> AxToken(string username, string password, string device)
        {
            dynamic data = new { un = username, pwd = password };
            Model<Token> resInvoke = WebServiceConsumer.Invoke<Model<Token>>("http://185.211.57.94/api/auth/ServiceLogin3", data, MethodType.Post, null);

            if (!resInvoke.succeed)
                return new ApiResult<AccessToken>(false, ApiResultStatusCode.UnAuthorized, null, resInvoke.error.userMessage);
            string barsaUser;
            try
            {
                var options = new RequestOptions
                {
                    Authorization = resInvoke.data.token,
                    Headers = new Dictionary<string, string> {{"sth", resInvoke.data.sth}}
                };
                barsaUser = WebServiceConsumer.Invoke<string>($"http://185.211.57.94/api/custom/checkUser?username={username}&password={password}", null, MethodType.Get, options);
            }
            catch
            {
                return new ApiResult<AccessToken>(false, ApiResultStatusCode.UnAuthorized, null, "خطا در SSO برسا نوین رای");
            }

            if (string.IsNullOrWhiteSpace(barsaUser))
                return new ApiResult<AccessToken>(false, ApiResultStatusCode.UnAuthorized, null, "نام کاربری و یا رمز عبور اشتباه است");

            var axUser = new AxUser { UserName = username, Password = password };
            using var qe = new QueryExecutor();
            var tokenBag = qe.Connection.QueryFirstOrDefault<TokenBag>("Select * from AxToken where username = @username", new { username });

            if (tokenBag != null)
                return new ApiResult<AccessToken>(false, ApiResultStatusCode.LogicError, null, $"کاربر گرامی {tokenBag.UserName} عزیز دستگاه {tokenBag.Device} قبلا با کاربری شما وارد سیستم شده است در صورت ادامه آن دستگاه از سیستم خارج می گردد,آیا ادامه می دهید؟");

            var requestOptions = new RequestOptions
            {
                Authorization = resInvoke.data.token,
                Headers = new Dictionary<string, string> { { "sth", resInvoke.data.sth } }
            };
            var res = WebServiceConsumer.Invoke<Model<dynamic>>("http://185.211.57.94/api/auth/logout", null, MethodType.Post, requestOptions);
            if (!res.succeed)
                return new ApiResult<AccessToken>(false, ApiResultStatusCode.UnAuthorized, null, "مشکل از logout برسا");
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
            var split = barsaUser.Split(",,", StringSplitOptions.RemoveEmptyEntries);
            token.userId = long.Parse(split[0]);
            token.name = split[1];
            return token;
        }
    }
}