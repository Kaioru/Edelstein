using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Objects.Users.Messages;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Game.Objects.Users;

public static class IFieldUserExtensions
{
    public static Task Message(this IFieldUser user, string chat)
        => user.Dispatch(new Message
        {
            Type = MessageType.SystemMessage,
            Info = new StructuredMessageInfoSystemMessage
            {
                Chat = new LPString(chat)
            }
        });
}
