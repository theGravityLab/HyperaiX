using System;

namespace HyperaiX.Units.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class OptionAttribute: Attribute
    {
        public OptionAttribute(string l0ng, char sh0rt, string key)
        {
            Short = sh0rt;
            Long = l0ng;
            Key = key;
        }

        public char Short {get;set;}
        public string Long {get;set;}
        public string Key {get;set;}
    }
}