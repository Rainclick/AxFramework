using System.Collections.Generic;

namespace Common
{
   public class UserAccess
    {
        public int UserId { get; set; }
        public HashSet<string> Keys { get; set; }
    }
}
