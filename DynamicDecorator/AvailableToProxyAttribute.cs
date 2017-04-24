using System;

namespace DynamicDecorator
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AvailableToProxyAttribute : Attribute
    {
    }
}