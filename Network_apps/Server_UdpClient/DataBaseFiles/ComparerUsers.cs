using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_UdpClient.DataBaseFiles
{
    internal class ComparerUsers : IEqualityComparer<User>
    {
        public bool Equals(User? x, User? y)
        {
            //if (x.Id == y.Id)
            //    return true;
            //return false;
            if (x.Name.Equals(y.Name))
                return true;
            return false;
        }

        public int GetHashCode([DisallowNull] User obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
