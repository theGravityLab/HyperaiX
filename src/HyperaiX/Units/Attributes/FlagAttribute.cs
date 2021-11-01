using System;

namespace HyperaiX.Units.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class FlagAttribute : Attribute
    {
        public char Short { get; set; }
        public string Long { get; set; }
    }
}