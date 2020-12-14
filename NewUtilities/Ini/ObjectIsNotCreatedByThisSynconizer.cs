using System;

namespace Utilities.Ini
{
    public class ObjectIsNotCreatedByThisSynconizer : Exception
    {
        public ObjectIsNotCreatedByThisSynconizer(string message) : base(message)
        {
        }
    }
}
