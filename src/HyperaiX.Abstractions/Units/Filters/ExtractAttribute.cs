using System.Text;
using Duffet.Builders;
using HyperaiX.Abstractions.Messages.Payloads.Elements;

namespace HyperaiX.Abstractions.Units.Filters;

public class ExtractAttribute : FilterAttribute
{
    private abstract record MatcherBase();

    private record PlainMatcher(string Plain) : MatcherBase();

    private record TextMatcher(string? Key) : MatcherBase();

    private record EaterMatcher() : MatcherBase;

    private record EaterAllMatcher() : MatcherBase;

    private record ElementMatcher(string? Key, string TypeName) : MatcherBase();

    private readonly IReadOnlyList<MatcherBase> _pattern;

    public ExtractAttribute(string pattern, bool trimmingSpaces = false)
    {
        _pattern = BuildPattern(pattern);
    }

    private IReadOnlyList<MatcherBase> BuildPattern(string pattern)
    {
        var list = new List<MatcherBase>();

        var plain = new StringBuilder();
        var last = 0;
        var curr = 0;

        while (curr < pattern.Length)
        {
            switch (pattern[curr])
            {
                case '\\':
                    if (curr + 1 < pattern.Length)
                    {
                        switch (pattern[curr + 1])
                        {
                            case '\\' or '*' or '{':
                                // 是转义，丢弃反斜杠，加入转义符号到待处理 plain
                                // 可以连续出现的转义符号，如果要取消转义需要每个转义符号前单独添加转义反斜杠
                                // 如果是捕获器的反转义，则只取消捕获器起始符号使整个捕获器失灵
                                // 由于发生了文本丢弃，需要先 collect 之前的 plain 再重置 plain 进度
                                if (last < curr)
                                {
                                    plain.Append(pattern[last..curr]);
                                }

                                // 这里不直接把转义符号加入到 plain，而是把上一个指针移动到转义符号前，以此丢弃反斜杠
                                last = curr + 1;

                                // 跳过被取消转义的符号，直接去匹配下下个字符
                                curr += 2;
                                break;
                            default:
                                // 毫无意义凭空出现的反斜杠，不报错，直接当作正常反斜杠而非转义符号
                                // 将游标调整到转义符号后面一位，继续做普通匹配
                                curr += 1;
                                break;
                        }
                    }
                    else
                    {
                        // 反转义符号位于末尾，当作毫无意义凭空出现的反斜杠
                        // 游标正常移动，触发收集并结束
                        curr += 1;
                    }

                    break;
                case '*':
                    if (last < curr)
                    {
                        plain.Append(pattern[last..curr]);
                        list.Add(new PlainMatcher(plain.ToString()));
                        plain.Clear();
                    }

                    if (curr + 1 < pattern.Length && pattern[curr + 1] == '*')
                    {
                        list.Add(new EaterAllMatcher());
                        curr += 2;
                    }
                    else
                    {
                        // 后面不尾随一个 *，是局部匹配
                        list.Add(new EaterMatcher());
                        curr += 1;
                    }

                    last = curr;

                    break;
                case '{':
                {
                    // 未来允许匹配 {At(@Self)} 或 {_:At(id)} 这种。不过这种 DSL 实在过于复杂了
                    // 例如 At(id) 就被理解为 AnyBinding("id")，At(@Self) 则被理解为 SelfBinding()
                    // ElementMatcher<At>(IElementBinding? binding): MatcherBase
                    // ElementMatcher<T: IMessageElement>.Capture(...) 会把 T 的第一个主构造参数作为 binding 对象
                    // 上述其他都能做，唯独解析 binding 需要把 raw string 交给 Matcher 让其自行构造，这样做麻烦
                    // 而且匹配 IMessageElement 内部的东西没啥必要，遂不予实现
                    // PS: binding 可以干脆取消，直接传递 string Parameter，让 Matcher 自己判断，不过还是多余

                    // 寻找未被反转义的捕获器结尾
                    var start = curr + 1;
                    var end = start;
                    var nameLength = 0;
                    var nameFirst = true;
                    var typeFirst = true;
                    while (end < pattern.Length)
                    {
                        if (pattern[end] == '}')
                        {
                            // 是结尾，结束并跳出
                            if (nameLength == 0)
                            {
                                // 只有名字无类型
                                nameLength = end - start;
                            }

                            break;
                        }

                        if (nameLength == 0)
                        {
                            if (pattern[end] == ':')
                            {
                                // 变量名分割符号
                                nameLength = end - start;
                                end += 1;
                            }
                            else if (char.IsAsciiLetter(pattern[end]) || pattern[end] == '_' || !nameFirst ||
                                     char.IsDigit(pattern[end]))
                            {
                                nameFirst = false;
                                end += 1;
                            }
                            else
                            {
                                // 匹配名字出错，直接失败并退出
                                end = pattern.Length;
                            }
                        }
                        else
                        {
                            if (char.IsAsciiLetter(pattern[end]) || pattern[end] == '_' || !typeFirst ||
                                char.IsDigit(pattern[end]))
                            {
                                typeFirst = false;
                                end += 1;
                            }
                            else
                            {
                                // 匹配类型出错，直接失败并退出
                                end = pattern.Length;
                            }
                        }
                    }

                    if (pattern[end] == '}')
                    {
                        // 正常封闭跳出

                        // 收集之前的作为 plain 并调整游标到结尾
                        if (last < curr)
                        {
                            plain.Append(pattern[last..curr]);
                            list.Add(new PlainMatcher(plain.ToString()));
                            plain.Clear();
                        }

                        curr = end + 1;
                        last = curr;

                        var name = pattern[start..(start + nameLength)];
                        var actualName = name switch
                        {
                            "_" or "" => null,
                            _ => name
                        };
                        // {a123}
                        // {:Abc}
                        // {_x:Y}
                        // {abc:}
                        // 012345
                        var typeLength = end - start - nameLength - 1;
                        if (typeLength == 0)
                        {
                            // 第四种
                            list.Add(new TextMatcher(actualName));
                        }
                        else if (typeLength > 0)
                        {
                            // 第二种或第三种
                            var typeName = pattern[(end - typeLength)..end];
                            list.Add(new ElementMatcher(actualName, typeName));
                        }
                        else
                        {
                            // 第一种，此时是 -1， 因为根本没出现分号
                            list.Add(new TextMatcher(actualName));
                        }
                    }

                    break;
                }
                default:
                    curr += 1;
                    break;
            }
        }

        if (last < pattern.Length - 1)
        {
            plain.Append(pattern[last..]);
            list.Add(new PlainMatcher(plain.ToString()));
            plain.Clear();
        }

        return list;
    }
}