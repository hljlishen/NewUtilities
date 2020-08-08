using System;

namespace Utilities.IniBind.InterfaceInterception
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NotIniKeyAttribute : Attribute
    {
    }
}
