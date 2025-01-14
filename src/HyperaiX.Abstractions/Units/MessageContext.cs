using HyperaiX.Abstractions.Messages;
using HyperaiX.Abstractions.Roles;

namespace HyperaiX.Abstractions.Units;

public class MessageContext(IEndClient client, IChat chat, IUser sender, IUser self, MessageEntity message)
{
    public IEndClient Client => client;
    public IChat Chat => chat;
    public IUser Sender => sender;
    public IUser Self => self;
    public MessageEntity Message => message;
}