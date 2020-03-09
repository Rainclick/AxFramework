using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Exception;
using Common.Utilities;
using Entities.Framework;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.UserRepositories
{
    public class UserRepository : BaseRepository<AxUser>, IUserRepository
    {
        public UserRepository(DataContext dbContext) : base(dbContext)
        {
        }

        public Task<AxUser> GetByUserAndPass(string username, string password, CancellationToken cancellationToken)
        {
            var passwordHash = SecurityHelper.GetSha256Hash(password);
            return Table.Where(p => p.UserName == username && p.Password == passwordHash).SingleOrDefaultAsync(cancellationToken);
        }

        public Task UpdateLastLoginDateAsync(AxUser user, CancellationToken cancellationToken)
        {
            user.LastLoginDate = DateTimeOffset.Now;
            return UpdateAsync(user, cancellationToken);
        }

        public async Task AddAsync(AxUser user, string password, CancellationToken cancellationToken)
        {
            var exists = await TableNoTracking.AnyAsync(p => p.UserName == user.UserName);
            if (exists)
                throw new BadRequestException("نام کاربری تکراری است");

            var passwordHash = SecurityHelper.GetSha256Hash(password);
            user.Password = passwordHash;
            await base.AddAsync(user, cancellationToken);
        }
    }
}