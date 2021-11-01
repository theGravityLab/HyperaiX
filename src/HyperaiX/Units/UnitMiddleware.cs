using System;
using HyperaiX.Abstractions.Events;

namespace HyperaiX.Units
{
    public class UnitMiddleware
    {
        private readonly UnitService _service;
        public UnitMiddleware(UnitService service)
        {
            _service = service;
        }
        public void Execute(GenericEventArgs args, Action next)
        {
            _service.Push(args);
            next();
        }
    }
}