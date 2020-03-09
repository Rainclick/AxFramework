using System.Threading.Tasks;
using Entities.Framework;

namespace Services.Services
{
    namespace Services
    {
        public interface IJwtService
        {
            Task<AccessToken> GenerateAsync(AxUser user);
        }
    }
}
