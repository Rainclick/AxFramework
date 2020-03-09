using System.Threading;
using System.Threading.Tasks;
using Common;
using Entities.Framework;

namespace Data.Repositories.UserRepositories
{
    public interface IUserRepository : IBaseRepository<AxUser>, IScopedDependency
    {
        Task<AxUser> GetByUserAndPass(string username, string password, CancellationToken cancellationToken);
        Task AddAsync(AxUser user, string password, CancellationToken cancellationToken);
        Task UpdateLastLoginDateAsync(AxUser user, CancellationToken cancellationToken);
    }
}