namespace HyperaiX.Abstractions.Units.Filters;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class FilterAttribute : Attribute
{
    public virtual bool IsMatch(MessageContext context) => false;

    public virtual void Process(MessageContext context)
    {
        // TODO: 往 Duffet.BankBuilder 里丢东西
    }
}