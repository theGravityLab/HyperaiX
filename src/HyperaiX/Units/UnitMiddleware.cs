using System;
using HyperaiX.Abstractions;
using HyperaiX.Abstractions.Events;

namespace HyperaiX.Units
{
    public class UnitMiddleware
    {
        private readonly UnitService _service;
        private readonly IApiClient _client;
        public UnitMiddleware(UnitService service, IApiClient client)
        {
            _service = service;
            _client = client;
        }
        public void Execute(GenericEventArgs args, Action next)
        {
            var context = args switch
            {
                GroupMessageEventArgs it => new MessageContext(it.Message, MessageEventType.Group, it.Sender, it.Group, _client),
                FriendMessageEventArgs it => new MessageContext(it.Message, MessageEventType.Friend, it.Sender, null, _client),
                _ => null
            };
            if(context != null) _service.Push(context);
            next();
        }
    }
}