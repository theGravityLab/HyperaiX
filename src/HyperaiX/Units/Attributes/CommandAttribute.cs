using System;
using System.Text.RegularExpressions;

namespace HyperaiX.Units.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public CommandAttribute(string text, string prefix = "!")
        {
            Text = text;
            Prefix = prefix;
            throw new NotImplementedException();
        }

        public string Text { get; set; }
        public string Prefix { get; set; }
    }
}