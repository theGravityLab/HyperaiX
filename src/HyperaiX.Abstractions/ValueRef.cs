namespace HyperaiX.Abstractions;

public class ValueRef<T>(string identity)
    where T : class
{
    public string Identity => identity;
}