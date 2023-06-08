using System;

namespace ITBWOQ
{
    public static class UniqueObject
    {
        public static string New()
        {
            return Guid.NewGuid().GetHashCode().ToString();
        }
    }
}
