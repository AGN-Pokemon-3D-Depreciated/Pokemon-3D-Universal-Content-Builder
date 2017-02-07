using System;
using System.Collections.Generic;

namespace Pokemon_3D_Server_Core.Modules.System.Collections.Generic
{
    public class NonCaseSensitiveHelper : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(string obj)
        {
            return base.GetHashCode();
        }
    }
}