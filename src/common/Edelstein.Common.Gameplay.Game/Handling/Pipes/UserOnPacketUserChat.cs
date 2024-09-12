using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Network.Packets.Types;
using Edelstein.Protocol.Utilities.Pipelines;
using UserChatRecv = Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv.UserChat;
using UserChatSend = Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send.UserChat;
using UserChatNLCPQSend = Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send.UserChatNLCPQ;

namespace Edelstein.Common.Gameplay.Game.Handling.Pipes;

public class UserOnPacketUserChat : AbstractUserOnPacketInFieldPipe<UserChatRecv>
{
    protected override async Task HandleAfter(IPipelineContext ctx, PipedFieldPacketMessage<UserChatRecv> message)
    {
        if (message.User.Field == null) return;
        if (message.User.FieldSplit == null) return;
        
        await message.User.FieldSplit.Dispatch(new UserChatSend
        {
            ObjectID = message.User.ObjectID ?? 0,
            IsAdminChat = message.User.Account.GradeCode > 0,
            Text = message.Packet.Text,
            OnlyBalloon = message.Packet.OnlyBalloon
        });

        if (message.Packet.OnlyBalloon) return;
        
        await Task.WhenAll(message.User.Field.GetObjects()
            .OfType<IFieldUser>()
            .Except(message.User.FieldSplit.GetObservers())
            .Select(u => u.Dispatch(new UserChatNLCPQSend
            {
                ObjectID = message.User.ObjectID ?? 0,
                IsAdminChat = message.User.Account.GradeCode > 0,
                Text = message.Packet.Text,
                OnlyBalloon = message.Packet.OnlyBalloon,
                Name = new LPString(message.User.Character.Name)
            })));
    }
}
