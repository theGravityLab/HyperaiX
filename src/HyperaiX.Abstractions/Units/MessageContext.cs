using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Roles;

namespace HyperaiX.Abstractions.Units;

public class MessageContext(IEndClient client, IChat chat, IUser user, MessageEntity message)
{
    public IEndClient Client => client;
    public IChat Chat => chat;
    public IUser User => user;
    public MessageEntity Message => message;
}