using System;
using HyperaiX.Abstractions.Events;

namespace HyperaiX.Units
{
    public class UnitMiddleware
    {
        private readonly UnitMiddlewareConfiguration _configuration;
        public UnitMiddleware(UnitMiddlewareConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Execute(GenericEventArgs args, Action next)
        {
            //TODO: process unit
            next();
        }
    }
}