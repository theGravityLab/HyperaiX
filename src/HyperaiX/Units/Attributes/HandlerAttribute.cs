using System;
using System.Text.RegularExpressions;

namespace HyperaiX.Units.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HandlerAttribute : Attribute
    {
        public HandlerAttribute(string pattern)
        {
            Pattern = pattern;

            // 支持多选区
            //pattern: !echo {chain} and {image:Image}
            //compile: ^!echo\ (?<chain>.+)\ and\ (?<image>\{[a-zA-Z0-9]+:Image\})$
            //flatten: !echo Hello and {1:Image}
            //               -----     ---------

            // 不加类型限定就是 chain 就是按照 MessageChain 去匹配，按照方法的形参类型进行转换
            //pattern: !echo {chain}
            //flatten: !echo Hello {1:Image}
            //               ---------------

            var escaped = Regex.Escape(pattern);
            // NOTE: 因为原文经过转义 {image:Image} 会变成 \{image:Image} 前面多个斜杠
            var argReg = new Regex(@"\\\{(?<name>[a-zA-Z0-9_]+)(:(?<format>[a-zA-Z0-9]+))?\}");
            var compiled = argReg.Replace(escaped, match =>
                match.Groups["format"].Success
                    ? $@"(?<{match.Groups["name"]}>\{{[a-zA-Z0-9]+:{match.Groups["format"]}\}})"
                    : $@"(?<{match.Groups["name"]}>.*)"
            );
            Compiled = new Regex(compiled);
        }

        public string Pattern { get; set; }

        internal Regex Compiled { get; private set; }
    }
}