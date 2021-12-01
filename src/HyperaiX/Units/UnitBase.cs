using System.Threading.Tasks;
using HyperaiX.Abstractions.Actions;
using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Receipts;
using HyperaiX.Abstractions.Relations;

namespace HyperaiX.Units
{
    public abstract class UnitBase
    {
        public MessageContext Context { get; internal set; }
    }
}