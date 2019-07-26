using System.Threading;
using System.Threading.Tasks;
using Common;
using Entities.Framework;

namespace Data.Repositories.UserRepositories
{
    public interface IUserRepository : IBaseRepository<User>, IScopedDependency
    {
        Task<User> GetByUserAndPass(string username, string password, CancellationToken cancellationToken);
        Task AddAsync(User user, string password, CancellationToken cancellationToken);
        Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
    }
}