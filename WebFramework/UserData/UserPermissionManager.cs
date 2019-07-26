using System.Collections.Generic;
using System.Linq;
using Common;

namespace WebFramework.UserData
{
    public static class UserPermissionManager
    {
        private static readonly List<UserAccess> UserAccesses = new List<UserAccess>();

        public static void AddUserAccesses(int userId, HashSet<string> keys)
        {
            if (UserAccesses.All(x => x.UserId != userId))
            {
                UserAccesses.Add(new UserAccess { UserId = userId, Keys = keys });
            }
        }

        public static HashSet<string> GetKeys(int userId)
        {
            var keys = UserAccesses.FirstOrDefault(x => x.UserId == userId)?.Keys;
            return keys;
        }
    }

}
