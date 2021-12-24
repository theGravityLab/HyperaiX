using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HyperaiX.Abstractions.Messages;

namespace HyperaiX.Units.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class RegexAttribute : ActionExtractorAttribute
{
    public RegexAttribute(Regex regex)
    {
        Compiled = regex;
    }

    public RegexAttribute(string regex) : this(new Regex(regex))
    {
    }

    public RegexAttribute()
    {
    }

    public Regex Compiled { get; set; }


    public override bool Match(MessageContext context, out IReadOnlyDictionary<string, MessageChain> properties)
    {
        var flatten = context.Message.Flatten();
        var match = Compiled.Match(flatten);
        var props = new Dictionary<string, MessageChain>();
        if (match.Success)
            foreach (Group group in match.Groups)
                if (@group.Success && @group.Name != string.Empty)
                {
                    var key = @group.Name;
                    var value = @group.Value;

                    props.Add(key, context.Message.Extract(value));
                }

        properties = props;
        return match.Success;
    }
}