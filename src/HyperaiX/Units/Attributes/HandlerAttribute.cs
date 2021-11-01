using System;

namespace HyperaiX.Units.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HandlerAttribute : Attribute
    {
        public HandlerAttribute(string pattern)
        {
            Pattern = pattern;
        }

        public string Pattern { get; set; }
    }
}