namespace HyperaiX.Abstractions.Relations;

public class Signature
{
    public Signature(string expression)
    {
        Expression = expression;
    }

    public string Expression { get; set; }
    public long Destination { get; set; }


    public bool Match(User user)
    {
        switch (user)
        {
            case Friend it:
            {
                var prefix = Expression.Substring(0, Expression.IndexOf(':'));
                var postfix = Expression.Substring(prefix.Length + 1);

                if (prefix == "*") return false;
                if (prefix == "_")
                {
                    if (long.TryParse(postfix, out var result)) return result == it.Identity;

                    return postfix == "*";
                }

                return false;
            }
            case Member it:
            {
                var prefix = Expression.Substring(0, Expression.IndexOf(':'));
                var postfix = Expression.Substring(prefix.Length + 1);

                if (prefix == "*")
                {
                    if (long.TryParse(postfix, out var result)) return result == it.Identity;

                    return false;
                }

                return prefix == it.GroupIdentity.ToString() && (postfix == "*" ||
                                                                 (it.GroupIdentity.ToString() ==
                                                                  prefix &&
                                                                  it.Identity.ToString() == postfix));
            }
            default:
                return false;
        }
    }

    public override string ToString()
    {
        return Expression;
    }

    public static Signature FromGroup(long groupId)
    {
        return new Signature($"{groupId}:*");
    }

    public static Signature FromMember(long groupId, long memberId)
    {
        return new Signature($"{groupId}:{memberId}");
    }

    public static Signature FromAnyGroup(long userId)
    {
        return new Signature($"*:{userId}");
    }

    public static Signature FromAnyGroupAnyMember()
    {
        return new Signature("*:*");
    }

    public static Signature FromFriend(long friendId)
    {
        return new Signature($"_:{friendId}");
    }

    public static Signature FromAnyFriend()
    {
        return new Signature("_:*");
    }
}