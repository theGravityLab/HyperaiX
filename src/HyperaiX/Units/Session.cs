using System.Collections.Generic;

namespace HyperaiX.Units;

public class Session
{
    internal Session()
    {
        EndOfLife = false;
    }

    public bool EndOfLife { get; private set; }
    public IDictionary<string, object> Data { get; set; }

    public void Finish()
    {
        EndOfLife = true;
    }
}